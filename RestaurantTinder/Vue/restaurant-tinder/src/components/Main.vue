<template>
 <div @click='documentClick'>
    <details-view :detail="detail" v-if="showDetails" v-on:show-details="doSomething"></details-view>
    <b-carousel
      id="carousel-1"
      :interval="0"
     
      style="text-shadow: 1px 1px 2px #333;"
    >
      <!-- Slide with blank fluid image to maintain slide aspect ratio -->
      <b-carousel-slide 
      v-for="item in justResults" :key="item.reference"
      >
       <img
          slot="img"
          class="main-img"
          :src="getPhoto(item.photos[0].photo_reference)"
          alt="image slot"
          @click="LoadDetails(item.reference)"
        >
        <h2 v-if="!showDetails" @click="LoadDetails(item.reference)">
          {{item.name}}
        </h2>
       <a href="#"  @click="reject"  class="carousel-control-prev"><span  class="carousel-control-prev-icon"></span><span class="sr-only">Previous Slide</span></a>
       <a href="#" @click="like" class="carousel-control-next"><span  class="carousel-control-next-icon"></span><span class="sr-only">Next Slide</span></a>
      </b-carousel-slide>
    </b-carousel>
  </div>
</template>

<script>
import auth from '../auth';
import tinder from '../tinder';
import DetailsView from '@/components/Details'
export default {
  name: 'Main',
  components: {
     DetailsView
  },
  data() {
    return {
      rejected: [],
      restaurant: [],
      liked: [], 
      showDetails: false,
      detail: {}
    };
  },
  methods: {
     doSomething(){
       this.showDetails = false;
     },
     documentClick(e){
        let el =  document.getElementById('modal-wrapper');
        if ( e.target === el ) {
          this.showDetails = false

        }
      },
    reject(event){
        let inputNode = document.getElementsByClassName("carousel-item active");
        let rejectedResturant = inputNode[0].innerText;
        tinder.destroyRejected();
        let testwords = tinder.getRejected();
         if(event){
           tinder.destroyRejected();
           if(!this.rejected.includes(rejectedResturant)){
           this.rejected.push(rejectedResturant)
           }
           let final = this.rejected.concat(testwords);
           tinder.saveRejected(final)
           inputNode[0].nextElementSibling.className = "carousel-item active";
           inputNode[0].className = "carousel-item";
          }
   },
   like(event){
      let inputNode = document.getElementsByClassName("carousel-item active");
      let likedResturant = inputNode[0].innerText;
      let testwords = tinder.getLiked();
         if(event){
           tinder.destroyLiked();
           if(!this.liked.includes(likedResturant)){
           this.liked.push(inputNode[0].innerText)
           }
           let final = this.liked.concat(testwords);
           tinder.saveLiked(final)
           inputNode[0].nextElementSibling.className = "carousel-item active";
           inputNode[0].className = "carousel-item";
          }
   },

    shuffle(array) {
        var currentIndex = array.length, temporaryValue, randomIndex;

        // While there remain elements to shuffle...
        while (0 !== currentIndex) {

          // Pick a remaining element...
          randomIndex = Math.floor(Math.random() * currentIndex);
          currentIndex -= 1;

          // And swap it with the current element.
          temporaryValue = array[currentIndex];
          array[currentIndex] = array[randomIndex];
          array[randomIndex] = temporaryValue;
     } 
      return array;
},
LoadDetails(vm){

  fetch(`${process.env.VUE_APP_REMOTE_API}/api/main/details/?placeId=${vm}`, {
        method: 'get',
        headers: {
          "Content-Type": "application/json",
          Authorization: 'Bearer ' + auth.getToken(),
        },
        credentials: 'same-origin',
      })
        .then((response) => {
          return response.json();
        })
        .then((data) => {
          this.detail = data.result;
          this.showDetails = true;
        })
        .catch((err) => console.error(err));
    },


   getPhoto(string){
     return `https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=${string}&key=AIzaSyDDHeRZd4LXtzzV41AN2CiZPXEA7R8Y3Tg`
  },
},
  computed: { 
      justResults(){

       let newarray = [];
       for(let i = 0; i < this.restaurant.length; i++){
           newarray = newarray.concat(this.restaurant[i].results)
         }
         let wordsToRemove = tinder.getRejected();
         if(wordsToRemove === null){
            return this.shuffle(newarray);
         }
         let filteredKeywords = newarray.filter((word) => !wordsToRemove.includes(word.name));
         return this.shuffle(filteredKeywords);
     }, 
  },
   created() {
  if(tinder.getRestaurant() === null){
    fetch(`${process.env.VUE_APP_REMOTE_API}/api/main/search`, {
      method: 'GET',
      headers: {
        Authorization: 'Bearer ' + auth.getToken(),
      },
      credentials: 'same-origin',
    })
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        this.restaurant = data;
        tinder.saveRestaurant(this.restaurant )
      })
      .catch((err) => console.error(err));
     }
     if(tinder.getRestaurant() != null){
       this.restaurant = tinder.getRestaurant();
     }
  },
  
}
</script>

<style scoped>
.carousel-control-next-icon {
  background-image: url("../assets/checkt.png")!important;
  width: 50px !important;
  height: 50px !important;
  opacity: 1;
}
.carousel-control-next {
  opacity: 1;
}
.carousel-control-prev {
  opacity: 1;
}
.carousel-control-prev-icon {
  background-image: url("../assets/xt.png")!important;
  width: 50px !important;
  height: 50px !important;
  opacity: 1;
}

.carousel-caption{
  background: #333333;
}
.main-img{
  width: 70% !important;
  margin-top: 1%;
  margin-left: 15% !important;
  height: 96% !important;
  cursor:pointer;
}
h2{
  cursor:pointer;
}

@media (max-width: 800px){
  .main-img{
    width: 70% !important;
    margin-left: 15% !important;
    margin-top: 10% !important;
    height: 60% !important;
    cursor: pointer;
    margin-top: 10% !important;
  }
}
.carousel {
  background: -webkit-gradient(linear, left bottom, left top, from(#ff6a00), to(#ee0979));
  background: linear-gradient(0deg, #ff6a00 0%, #ee0979 100%);
  background-repeat: no-repeat;
  background-position: center center;
  background-attachment: scroll;
  background-size: cover;
}
.carousel-item{
  height: 90vh;
}
h2{
  cursor: pointer;
}

</style>

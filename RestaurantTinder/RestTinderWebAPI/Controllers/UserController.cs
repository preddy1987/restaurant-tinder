﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantTinder.Interfaces;
using RestaurantTinder.Models;
using RestTinderWebAPI.Models;

namespace RestTinderWebAPI.Controllers
{
    public class UserController : AuthController
    {
        public UserController(IRestaurantService db, IHttpContextAccessor httpContext) : base(db, httpContext)
        {

        }
        
        [HttpGet]
        [Route("api/user")]
        public ActionResult<IEnumerable<UserItem>> Get()
        {
            var result = Json(_db.GetUserItems());
            return GetAuthenticatedJson(result, (Role.IsExecutive || Role.IsAdministrator));
        }

        [HttpPost]
        [Route("api/login")]
        public ActionResult<StatusViewModel> Login([FromBody] LoginViewModel info)
        {
            StatusViewModel result = new StatusViewModel();

            try
            {
                LoginUser(info.UserName, info.Password);
            }
            catch(Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        [Route("api/register")]
        public ActionResult<StatusViewModel> Register([FromBody] RegisterViewModel info)
        {
            StatusViewModel result = new StatusViewModel();

            try
            {
                var user = new User();
                user.ConfirmPassword = info.ConfirmPassword;
                user.Email = info.Email;
                user.FirstName = info.FirstName;
                user.LastName = info.LastName;
                user.Password = info.Password;
                user.Username = info.Username;
                user.ZipCode = info.ZipCode;

                RegisterUser(user);
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = ex.Message;
            }

            return Json(result);
        }
        [HttpGet]
        [Route("api/preference")]
        public ActionResult<IEnumerable<UserItem>> GetUserPref(UserItem user)
        {
            var result = Json(_db.GetPreferredFoodItems(user.Id));
            return GetAuthenticatedJson(result, (Role.IsCustomer));
        }
        [HttpPost]
        [Route("api/savepreference")]
        public ActionResult<IEnumerable<UserItem>> SaveUserPref(List<PreferredFoodItem> foodPreferences)
        {
            JsonResult result = null;

            foreach (var foodItem in foodPreferences)
            {
                _db.AddPreferredFoodItem(foodItem);               
            }
            return GetAuthenticatedJson(result, (Role.IsCustomer));
        }

    }
}
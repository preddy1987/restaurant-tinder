USE [master]
GO
/****** Object:  Database [RestaurantTinder]    Script Date: 4/8/2019 11:43:00 AM ******/
CREATE DATABASE [RestaurantTinder]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ResaurantTinder', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ResaurantTinder.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ResaurantTinder_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ResaurantTinder_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [RestaurantTinder] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RestaurantTinder].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RestaurantTinder] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RestaurantTinder] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RestaurantTinder] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RestaurantTinder] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RestaurantTinder] SET ARITHABORT OFF 
GO
ALTER DATABASE [RestaurantTinder] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RestaurantTinder] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RestaurantTinder] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RestaurantTinder] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RestaurantTinder] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RestaurantTinder] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RestaurantTinder] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RestaurantTinder] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RestaurantTinder] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RestaurantTinder] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RestaurantTinder] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RestaurantTinder] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RestaurantTinder] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RestaurantTinder] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RestaurantTinder] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RestaurantTinder] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RestaurantTinder] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RestaurantTinder] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RestaurantTinder] SET  MULTI_USER 
GO
ALTER DATABASE [RestaurantTinder] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RestaurantTinder] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RestaurantTinder] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RestaurantTinder] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RestaurantTinder] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RestaurantTinder] SET QUERY_STORE = OFF
GO
USE [RestaurantTinder]
GO
/****** Object:  Table [dbo].[Blacklist]    Script Date: 4/8/2019 11:43:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blacklist](
	[RestaurantId] [int] NOT NULL,
	[UserId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Favorites]    Script Date: 4/8/2019 11:43:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Favorites](
	[RestaurantId] [int] NOT NULL,
	[user_id] [int] NOT NULL,
 CONSTRAINT [PK_Favorites] PRIMARY KEY CLUSTERED 
(
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Restaurant]    Script Date: 4/8/2019 11:43:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Restaurant](
	[Id] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Restaurant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleItem]    Script Date: 4/8/2019 11:43:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleItem](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RoleItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 4/8/2019 11:43:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Hash] [varchar](50) NOT NULL,
	[RoleId] [int] NOT NULL,
	[Salt] [varchar](50) NOT NULL,
	[ZipCode] [int] NOT NULL,
 CONSTRAINT [PK_TinderUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Blacklist]  WITH CHECK ADD  CONSTRAINT [FK_Blacklist_Restaurant] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurant] ([Id])
GO
ALTER TABLE [dbo].[Blacklist] CHECK CONSTRAINT [FK_Blacklist_Restaurant]
GO
ALTER TABLE [dbo].[Blacklist]  WITH CHECK ADD  CONSTRAINT [FK_Blacklist_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Blacklist] CHECK CONSTRAINT [FK_Blacklist_User]
GO
ALTER TABLE [dbo].[Favorites]  WITH CHECK ADD  CONSTRAINT [FK_Favorites_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Favorites] CHECK CONSTRAINT [FK_Favorites_User]
GO
ALTER TABLE [dbo].[Restaurant]  WITH CHECK ADD  CONSTRAINT [FK_Restaurant_Favorites] FOREIGN KEY([Id])
REFERENCES [dbo].[Favorites] ([RestaurantId])
GO
ALTER TABLE [dbo].[Restaurant] CHECK CONSTRAINT [FK_Restaurant_Favorites]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleItem] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_User] FOREIGN KEY([Id])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_User]
GO
USE [master]
GO
ALTER DATABASE [RestaurantTinder] SET  READ_WRITE 
GO

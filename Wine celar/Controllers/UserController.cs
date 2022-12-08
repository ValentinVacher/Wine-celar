﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wine_cellar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Wine_cellar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserRepository UserRepository;
        readonly IWebHostEnvironment environment;
        public UserController(IUserRepository Repository, IWebHostEnvironment environment)
        {
            this.UserRepository = Repository;
            this.environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync()
        {
            return Ok(await UserRepository.GetAllUserAsync());
        }

        [HttpGet("{login}/{pwd}")]
        public async Task<IActionResult> LoginAsync(string login, string pwd)
        {
            var userConnected = await UserRepository.LoginUser(login, pwd);

            if (userConnected == null) return Problem($"Erreur lors du login, vérifiez le login ou mot de passe");

            List<Claim> claims = new List<Claim>();

            claims.Add(new(ClaimTypes.Email, userConnected.Email));
            claims.Add(new(ClaimTypes.Name, userConnected.LastName));
            claims.Add(new(ClaimTypes.GivenName, userConnected.FirstName));
            claims.Add(new(ClaimTypes.NameIdentifier, userConnected.UserId.ToString()));

            if (userConnected.IsAdmin)
            {
                claims.Add(new(ClaimTypes.Role, "admin"));
            }
            else
            {
                claims.Add(new(ClaimTypes.Role, ""));
            }

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok($"{userConnected.LastName} logged");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Ok("Logout");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserViewModel userView)
        {
            Regex regexPsw = new(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
            if(!regexPsw.Match(userView.Password).Success) return Problem("Mot de passe incorrect");

            Regex regexMail = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!regexMail.Match(userView.Email).Success) return Problem("Email invalide");

            User user = new()
            {
                FirstName= userView.FirstName,
                LastName= userView.LastName,
                DateOfBirth = userView.DateOfBirth,
                Email= userView.Email,
                Password= userView.Password
            };

            if(!user.IsOlder())
            {
                return Problem("L'alcool est interdit au moins de 18 ans");
            }

            var userCreated = await UserRepository.CreateUserAsync(user);

            if (userCreated == null)
            {
                return Problem("Erreur lors de la création de l'utilisateur");
            }

            return Ok(userCreated);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserViewModel userView)
        {
            User user = new()
            {
                UserId = userView.UserId,
                FirstName = userView.FirstName,
                LastName = userView.LastName,
                Email = userView.Email,
                Password = userView.Password
            };

            var userUpdate = await UserRepository.UpdateUserAsync(user);

            if (userUpdate == null)
            {
                return Problem("Erreur lors de la mise a jour de l'utilisateur");
            }

            return Ok(userUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool success = await UserRepository.DeleteUserAsync(id);

            if (success)
                return Ok($"L'utilisateur {id} a été supprimé");
            else
                return Problem($"Erreur lors de la suppression de l'utilisateur");
        }
    }
}

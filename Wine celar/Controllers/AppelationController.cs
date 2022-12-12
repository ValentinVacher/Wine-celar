﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using Wine_celar.Repositories;
using Wine_celar.ViewModel;
using Wine_cellar.Entities;
using Wine_cellar.IRepositories;
using Wine_cellar.Repositories;
using Wine_cellar.Tools;
using Wine_cellar.ViewModel;

namespace Wine_celar.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AppelationController : ControllerBase
    {
        readonly IAppelationRepository AppelationRepository;
        readonly IWebHostEnvironment environment;
        public AppelationController(IAppelationRepository Repository, IWebHostEnvironment environment)
        {
            this.AppelationRepository = Repository;
            this.environment = environment;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAppelation()
        {
            return Ok(await AppelationRepository.GetAllAppelationsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppelation(int id)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);

            int userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            var appel = await AppelationRepository.GetAppelationAsync(id, userId);

            if (appel == null) return NotFound(ErrorCode.AppelationNotFound);

            return Ok(appel);
        }

        [HttpGet]
        public async Task<ActionResult<List<Appelation>>> GetAppelationsByColor(WineColor color)
        {
            var appelations = await AppelationRepository.GetAppelationsByColoAsync(color);

            if (appelations == null) return NotFound(ErrorCode.AppelationNotFound);

            return Ok(appelations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppelation([FromForm] CreateAppelationViewModel appelViewModel)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);

            var appel = Convertor.CreateAppelation(appelViewModel);
            var AppelationCreated = await AppelationRepository.CreateAppelationAsync(appel);

            if (AppelationCreated == null) return BadRequest(ErrorCode.AppelationAlreadyExists);

            return Ok(AppelationCreated);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppelation([FromForm] UpdateAppelationViewModel appelation)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);

            var appelUpdate = await AppelationRepository.UpdateAppelationAsync(appelation);

            if (appelUpdate == 0) return NotFound(ErrorCode.AppelationNotFound);

            return Ok(appelUpdate);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAppelation(int appelationId)
        {
            var identity = User?.Identity as ClaimsIdentity;

            if (identity?.FindFirst(ClaimTypes.NameIdentifier) == null) return BadRequest(ErrorCode.UnLogError);
            if (identity?.FindFirst(ClaimTypes.Role).Value != "admin") return BadRequest(ErrorCode.NotAdminError);

            var success = await AppelationRepository.DeleteAppelationAsync(appelationId);

            if (success == 0) NotFound(ErrorCode.AppelationNotFound);

            return Ok(appelationId);
        }
    }
}

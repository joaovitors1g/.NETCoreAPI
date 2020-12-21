using System;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.DTOs;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class LoginController : ControllerBase
   {
      [AllowAnonymous]
      [HttpPost]
      public async Task<object> Login([FromBody] LoginDTO loginDto, [FromServices] ILoginService service)
      {
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         if (loginDto == null)
         {
            return BadRequest();
         }

         try
         {
            var user = await service.FindByLogin(loginDto);

            if (user != null)
            {
               return Ok(user);
            }
            else
            {
               return NotFound();
            }
         }
         catch (ArgumentException ex)
         {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
         }
      }
   }
}

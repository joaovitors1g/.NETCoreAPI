using System;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _service;
    public UsersController(IUserService service)
    {
      _service = service;
    }

    [HttpGet]
    public async Task<ActionResult> Index()
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        return Ok(await _service.GetAll());
      }
      catch (ArgumentException ex)
      {
        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
      }
    }

    [HttpGet]
    [Route("{id}", Name = "GetWithId")]
    public async Task<ActionResult> View(Guid id)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        return Ok(await _service.Get(id));
      }
      catch (ArgumentException ex)
      {
        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
      }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] UserEntity user)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        var result = await _service.Post(user);

        if (result != null)
        {
          return Created(new Uri(Url.Link("GetWithId", new { result.Id })), result);
        }
        else
        {
          return BadRequest();
        }
      }
      catch (Exception ex)
      {

        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
      }
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UserEntity user)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        var result = await _service.Put(user);

        if (result != null)
        {
          return Ok(result);
        }
        else
        {
          return BadRequest();
        }
      }
      catch (Exception ex)
      {

        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
      }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        return Ok(await _service.Delete(id));
      }
      catch (ArgumentException ex)
      {
        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
      }
    }
  }
}

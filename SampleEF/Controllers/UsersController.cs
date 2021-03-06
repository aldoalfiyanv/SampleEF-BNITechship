using Microsoft.AspNetCore.Mvc;
using SampleEF.Data;
using SampleEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleEF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUser _user;
        public UsersController(IUser user)
        {
            _user = user;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] User user)
        {
            try
            {
                await _user.Registration(user);
                return Ok("Proses Registrasi Berhasil");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 

        [HttpPost("Authentication")]
        public async Task<IActionResult> Authentication([FromBody] User userParam)
        {
            var user = await _user.Authenticate(userParam.Username, userParam.Password);
            if (user == null)
                return BadRequest("Username/Password incorrect");
            return Ok(user);
        }


        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

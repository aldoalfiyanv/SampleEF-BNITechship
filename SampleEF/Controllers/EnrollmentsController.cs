﻿using Microsoft.AspNetCore.Authorization;
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
    public class EnrollmentsController : ControllerBase
    {
        private IEnrollment _enrollment;
        public EnrollmentsController(IEnrollment enrollment)
        {
            _enrollment = enrollment;
        }

        // GET: api/<EnrollmentsController>
        
        [Authorize] //Autorisasi Enrollment
        [HttpGet]
        public async Task<IEnumerable<Enrollment>> Get()
        {
            var results = await _enrollment.GetAll();
            return results;
        }

        // GET api/<EnrollmentsController>/5
        [HttpGet("{id}")]
        public async Task<Enrollment> Get(int id)
        {
            var result = await _enrollment.GetById(id.ToString());
            return result;
        }

        // POST api/<EnrollmentsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment enrollment)
        {
            try
            {
                await _enrollment.Insert(enrollment);
                return Ok($"Data Grade Student {enrollment.Grade} berhasil ditambahkan");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<EnrollmentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Enrollment enrollment)
        {
            try
            {
                await _enrollment.Update(id.ToString(), enrollment);
                return Ok($"Data Grade Student ID {id} berhasil diupdate");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<EnrollmentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _enrollment.Delete(id.ToString());
                return Ok($"Data Grade student {id} berhasil didelete");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

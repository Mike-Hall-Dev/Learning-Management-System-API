﻿using Lms.Daos;
using Lms.Dtos.Request;
using Lms.Extensions;
using Lms.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lms.Controllers
{
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly TeacherDao _teacherDao;
        public TeacherController(TeacherDao teacherDao)
        {
            _teacherDao = teacherDao;
        }

        /// <summary>
        /// Gets teachers with optional query params. Returns max of 25 teachers.
        /// </summary>
        [HttpGet]
        [Route("teachers")]
        public async Task<IActionResult> GetStudentsWithParams([FromQuery] TeacherRequestForParams teacherParams)
        {
            try
            {
                var teachers = await _teacherDao.GetStudentsWithOptionalParams(teacherParams);

                if (teachers == null)
                {
                    return StatusCode(200, new { });
                }

                return Ok(teachers.ConvertToDtoList());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get Teacher by Id
        /// </summary>
        /// <param name="id">ID for a specific teacher</param>
        [HttpGet]
        [Route("teachers/{id}", Name = "GetTeacherById")]
        public async Task<IActionResult> GetTeacherById([FromRoute] Guid id)
        {
            try
            {
                var teacher = await _teacherDao.GetTeacherById(id);
                if (teacher == null)
                {
                    return StatusCode(200, new { });
                }
                return Ok(teacher.ConvertToDto());

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Create a new Teacher
        /// </summary>
        /// <param name="newTeacher">JSON object for creation of a new teacher</param>
        [HttpPost]
        [Route("teachers")]
        public async Task<IActionResult> CreateNewTeacher([FromBody] TeacherRequestDto newTeacher)
        {
            try
            {
                var createdTeacher = await _teacherDao.CreateTeacher(newTeacher);
                var createdTeacherDto = createdTeacher.ConvertToDto();

                return CreatedAtRoute(nameof(GetTeacherById), new { id = createdTeacherDto.Id }, createdTeacherDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Delete a Teacher by Id
        /// </summary>
        /// <param name="id">ID for a specific teacher</param>        
        [HttpDelete]
        [Route("teachers/{id}")]
        public async Task<IActionResult> DeleteTeacherById([FromRoute] Guid id)
        {
            try
            {
                var teacher = await _teacherDao.GetTeacherById(id);
                if (teacher == null)
                {
                    return ValidationProblem($"This teacher could not be found. No delete action has been taken.");
                }

                await _teacherDao.DeleteTeacherById(id);

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update a Teacher by Id
        /// </summary>
        /// <param name="id">ID for a specific teacher</param>
        /// <param name="updateRequest">JSON object with updated data for teacher</param>        
        [HttpPut]
        [Route("teachers/{id}")]
        public async Task<IActionResult> UpdateTeacherById([FromRoute] Guid id, [FromBody] TeacherRequestDto updateRequest)
        {
            try
            {
                var teacher = await _teacherDao.GetTeacherById(id);
                if (teacher == null)
                {
                    return ValidationProblem($"This teacher could not be found. No update action has been taken.");
                }

                await _teacherDao.UpdateTeacherById(id, updateRequest);
                return StatusCode(200);

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

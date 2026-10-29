using System;
using Microsoft.AspNetCore.Mvc;
using Employees.Services;
using Employees.Interfaces;
using Employees.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Employees.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Department>>> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(departments);
        }
    }
}

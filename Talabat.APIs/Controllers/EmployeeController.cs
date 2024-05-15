using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ApiBaseController
    {
        private readonly IGenericRepository<Employee> empRepo;

        public EmployeeController(IGenericRepository<Employee> empRepo)
        {
            this.empRepo = empRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Employee>>> GetEmployees()
        {
            var spec = new EmployeeSpecifications();
            var Employees = empRepo.GetAllWithSpaceAsync(spec);

            return Ok(Employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var spec = new EmployeeSpecifications();
            var Employees = empRepo.GetAllWithSpaceAsync(spec);

            return Ok(Employees);
        }



    }
}

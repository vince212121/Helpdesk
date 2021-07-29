/*
 * DepartmentControler.cs
 * Vincent Li
 * This is the department controller that takes the get all functionality
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelpdeskViewModels;


namespace CaseStudyWebsite.Controllers
{
    [Route("api/[controller]")] // tells its an API controller
    [ApiController] // how the outside world will call the local host
    public class DepartmentController : ControllerBase
    {
        // test by doing api/employee
        [HttpGet] // called with API/employee, the other get expects a parameter and since we are not supplying a parameter, it will know to use getall
        public IActionResult GetAll()
        {
            try
            {
                DepartmentViewModel viewmodel = new DepartmentViewModel();
                List<DepartmentViewModel> allStudents = viewmodel.GetAll();
                return Ok(allStudents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
    }
}
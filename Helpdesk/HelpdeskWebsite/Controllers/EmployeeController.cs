/*
\file:      EmployeeController
\author:    Vincent Li
\purpose:   This is the controller class used for things like PUT, GET, etc.
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
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpGet("{email}")] // in the REST article in Lab 6 https://www.sitepoint.com/what-does-restful-really-mean/
        public IActionResult GetByMail(string email)
        {
            try
            {
                EmployeeViewModel viewmodel = new EmployeeViewModel()
                {
                    Email = email
                };
                viewmodel.GetByMail();

                return Ok(viewmodel); // return the entire instance to the browser, OK means the https status thing (200 is the one that works)
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpPut] // used for lab 9 with the PUT part in the js
        // since the information is coming into the header, it will just be availible as a viewmodel so you don't need a parameter
        public ActionResult Put(EmployeeViewModel viewmodel)
        {
            try
            {
                int retVal = viewmodel.Update(); // will update here or try to
                return retVal switch
                {
                    1 => Ok(new { msg = "Employee " + viewmodel.Lastname + " updated!" }),
                    -1 => Ok(new { msg = "Employee " + viewmodel.Lastname + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for " + viewmodel.Lastname + ", Employee not updated!" }),
                    _ => Ok(new { msg = "Employee " + viewmodel.Lastname + " not updated!" }),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        // test by doing api/employee
        [HttpGet] // called with API/Employee, the other get expects a parameter and since we are not supplying a parameter, it will know to use getall
        public IActionResult GetAll()
        {
            try
            {
                EmployeeViewModel viewmodel = new EmployeeViewModel();
                List<EmployeeViewModel> allEmployees = viewmodel.GetAll();
                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpPost]
        public ActionResult Post(EmployeeViewModel viewmodel)
        {
            try
            {
                viewmodel.Add();
                return viewmodel.Id > 1
                    ? Ok(new { msg = "Employee " + viewmodel.Lastname + " added!" })
                    : Ok(new { msg = "Employee " + viewmodel.Lastname + " not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                EmployeeViewModel vm = new EmployeeViewModel { Id = id };
                return vm.Delete() == 1
                    ? Ok(new { msg = "Employee " + id + " deleted!" })
                    : Ok(new { msg = "Employee " + id + " not deleted!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
    }
}

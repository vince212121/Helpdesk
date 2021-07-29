/*
\file:      callcontroller
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
    public class CallController : ControllerBase
    {
        [HttpGet] // used to get all
        public IActionResult GetAll()
        {
            try
            {
                CallViewModel viewmodel = new CallViewModel();
                List<CallViewModel> allCalls = viewmodel.GetAll();
                return Ok(allCalls);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        // used for get by id
        [HttpGet("{id}")] // used to get all
        public IActionResult GetById(int id)
        {
            try
            {
                CallViewModel viewmodel = new CallViewModel()
                {
                    Id = id
                };
                viewmodel.GetById();
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        // used to put/update stuff
        [HttpPut]
        public ActionResult Put(CallViewModel viewmodel)
        {
            try
            {
                int retVal = viewmodel.Update(); // will update here or try to
                return retVal switch
                {
                    1 => Ok(new { msg = "Call " + viewmodel.Id + " updated!" }),
                    -1 => Ok(new { msg = "Call " + viewmodel.Id + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for " + viewmodel.Id + ", Call not updated!" }),
                    _ => Ok(new { msg = "Call " + viewmodel.Id + " not updated!" }),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        // used to add stuff
        [HttpPost]
        public ActionResult Post(CallViewModel viewmodel)
        {
            try
            {
                viewmodel.Add();
                return viewmodel.Id > 0
                    ? Ok(new { msg = "Call " + viewmodel.Id + " added!" })
                    : Ok(new { msg = "Call " + viewmodel.Id + " not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }


        // used to delete stuff
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                CallViewModel vm = new CallViewModel { Id = id };
                return vm.Delete() == 1
                    ? Ok(new { msg = "Call " + id + " deleted!" })
                    : Ok(new { msg = "Call " + id + " not deleted!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
    }
}

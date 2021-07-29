/*
\file:      problem controller
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
    public class ProblemController : ControllerBase
    {
        [HttpGet] // used to get all
        public IActionResult GetAll()
        {
            try
            {
                ProblemViewModel viewmodel = new ProblemViewModel();
                List<ProblemViewModel> allProblems = viewmodel.GetAll();
                return Ok(allProblems);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        // gets by description
        [HttpGet("{desc}")]
        public IActionResult GetByDescription(string desc)
        {
            try
            {
                ProblemViewModel viewmodel = new ProblemViewModel()
                {
                    Desc = desc
                };
                viewmodel.GetByDescription();

                return Ok(viewmodel); // return the entire instance to the browser, OK means the https status thing (200 is the one that works)
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
    }
}

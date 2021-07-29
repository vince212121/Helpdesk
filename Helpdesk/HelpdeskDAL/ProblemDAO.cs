/*
\file:      problemDAO
\author:    Vincent Li
\purpose:   This is the problem DAO class tier
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Buffers;
namespace HelpdeskDAL
{
    public class ProblemDAO
    {
        HelpdeskRepository<Problems> repo;

        public ProblemDAO()
        {
            repo = new HelpdeskRepository<Problems>(); // new repo
        }

        // Get by description
        public Problems GetByDescription(string desc)
        {
            try
            {
                // returns the problem 
                return repo.GetByExpression(prob => prob.Description == desc).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // get all
        public List<Problems> GetAll()
        {
            try
            {
                return repo.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
    }
}

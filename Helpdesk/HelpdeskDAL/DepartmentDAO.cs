/*
 * File: DepartmentDAO
 * Author: Vincent Li
 * Purpose: used to get all from the divisions
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;


namespace HelpdeskDAL
{
    public class DepartmentDAO
    {
        HelpdeskRepository<Departments> repo;
        public DepartmentDAO()
        {
            repo = new HelpdeskRepository<Departments>();
        }

        public List<Departments> GetAll()
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
/*
\file:      departmentViewModel
\author:    Vincent Li
\purpose:   This is the viewmodel for department
*/
using HelpdeskDAL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Text;

namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        readonly private DepartmentDAO _dao;
        public string Name { get; set; }
        public int Id { get; set; }

        public string Timer { get; set; }

        public DepartmentViewModel()
        {
            _dao = new DepartmentDAO();
        }

        // return all departments
        public List<DepartmentViewModel> GetAll()
        {
            List<DepartmentViewModel> allVms = new List<DepartmentViewModel>();
            try
            {
                List<Departments> allDepartments = _dao.GetAll();
                foreach (Departments div in allDepartments)
                {
                    DepartmentViewModel divVm = new DepartmentViewModel();

                    divVm.Name = div.DepartmentName;
                    divVm.Id = div.Id;
                    divVm.Timer = Convert.ToBase64String(div.Timer);
                    allVms.Add(divVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return allVms;
        }

    }
}
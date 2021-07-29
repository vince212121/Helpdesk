/*
\file:      ProblemViewModel
\author:    Vincent Li
\purpose:   This is the viewmodel for problem
*/
using HelpdeskDAL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Text;

namespace HelpdeskViewModels
{
    public class ProblemViewModel
    {
        readonly private ProblemDAO _dao;
        public string Desc { get; set; }
        public int Id { get; set; }

        public string Timer { get; set; }

        public string ErrorMessage { get; set; }

        public ProblemViewModel()
        {
            _dao = new ProblemDAO();
        }

        // return by description
        public void GetByDescription()
        {
            try
            {
                Problems pro = _dao.GetByDescription(Desc);
                Id = pro.Id;
                Desc = pro.Description;
                Timer = Convert.ToBase64String(pro.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Desc = "Not Found";
                ErrorMessage = "Not Found";
            }
            catch (Exception ex)
            {
                Desc = "Not Found";
                ErrorMessage = "Not Found";
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }


        // return all problems
        public List<ProblemViewModel> GetAll()
        {
            List<ProblemViewModel> allVms = new List<ProblemViewModel>();
            try
            {
                List<Problems> allDepartments = _dao.GetAll();
                foreach (Problems div in allDepartments)
                {
                    ProblemViewModel divVm = new ProblemViewModel();

                    divVm.Desc = div.Description;
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

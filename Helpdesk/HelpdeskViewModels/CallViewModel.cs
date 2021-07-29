/*
\file:      CallViewModel
\author:    Vincent Li
\purpose:   This is the viewmodel for Calls
*/
using HelpdeskDAL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Text;

namespace HelpdeskViewModels
{
    public class CallViewModel
    {
        private CallDAO _dao;

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int ProblemId { get; set; }
        public string ProblemDescription { get; set; }
        public int TechId { get; set; }
        public string TechName { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string Notes { get; set; }
        public string Timer { get; set; }

        public CallViewModel()
        {
            _dao = new CallDAO();
        }

        // get by id
        public void GetById()
        {
            try
            {
                Calls call = _dao.GetById(Id);
                Id = call.Id;
                EmployeeId = call.EmployeeId;
                ProblemId = call.ProblemId;
                TechId = call.TechId;
                DateOpened = call.DateOpened;
                DateClosed = call.DateClosed;
                OpenStatus = call.OpenStatus;
                Notes = call.Notes;
                Timer = Convert.ToBase64String(call.Timer);
            }
            catch (NullReferenceException nex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + nex.Message);
                throw nex;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // get all
        public List<CallViewModel> GetAll()
        {
            List<CallViewModel> allVms = new List<CallViewModel>();
            try
            {
                List<Calls> allCalls = _dao.GetAll();
                EmployeeDAO emp = new EmployeeDAO();

                foreach (Calls call in allCalls)
                {
                    CallViewModel callVm = new CallViewModel();

                    callVm.Id = call.Id;
                    callVm.EmployeeId = call.EmployeeId;
                    callVm.EmployeeName = emp.GetById(call.EmployeeId).LastName;
                    callVm.ProblemId = call.ProblemId;
                    callVm.ProblemDescription = call.Problem.Description;
                    callVm.TechId = call.TechId;
                    callVm.TechName = emp.GetById(call.TechId).LastName;
                    callVm.DateOpened = call.DateOpened;
                    callVm.DateClosed = call.DateClosed;
                    callVm.OpenStatus = call.OpenStatus;
                    callVm.Notes = call.Notes;
                    callVm.Timer = Convert.ToBase64String(call.Timer);

                    allVms.Add(callVm);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return allVms;
        }

        // add
        public void Add()
        {
            Id = -1;
            try
            {
                Calls call = new Calls();

                call.EmployeeId = EmployeeId;
                call.ProblemId = ProblemId;
                call.TechId = TechId;
                call.DateOpened = DateOpened;
                call.DateClosed = DateClosed;
                call.OpenStatus = OpenStatus;
                call.Notes = Notes;
                Id = _dao.Add(call);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // update
        public int Update()
        {
            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                Calls call = new Calls();

                call.EmployeeId = EmployeeId;
                call.ProblemId = ProblemId;
                call.TechId = TechId;
                call.DateOpened = DateOpened;
                call.DateClosed = DateClosed;
                call.OpenStatus = OpenStatus;
                call.Notes = Notes;
                call.Id = Id;
                call.Timer = Convert.FromBase64String(Timer);
                operationStatus = _dao.Update(call);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(operationStatus);
        }

        // delete
        public int Delete()
        {
            int callDeleted = -1;

            try
            {
                callDeleted = _dao.Delete(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return callDeleted;
        }
    }
}

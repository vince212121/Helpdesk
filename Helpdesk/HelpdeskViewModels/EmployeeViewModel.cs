/*
\file:      EmployeeViewModel
\author:    Vincent Li
\purpose:   This is the viewmodel used for the employee
*/

using System;
using HelpdeskDAL;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Text;

namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        readonly private EmployeeDAO _dao;

        // fields to fill
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public string Timer { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string FullName { get; set; }
        public int Id { get; set; }
        public string Picture64 { get; set; }
        public bool? IsTech { get; set; }
        public string ErrorMessage { get; set; }

        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }

        // find emp using email property

        public void GetByMail()
        {
            try
            {
                Employees stu = _dao.GetByMail(Email);
                Title = stu.Title;
                Firstname = stu.FirstName;
                Lastname = stu.LastName;
                Phoneno = stu.PhoneNo;
                Email = stu.Email;
                Id = stu.Id;
                DepartmentId = stu.DepartmentId;
                DepartmentName = stu.Department.DepartmentName;
                FullName = stu.Title + " " + stu.FirstName + " " + stu.LastName;
                if (stu.StaffPicture != null)
                {
                    Picture64 = Convert.ToBase64String(stu.StaffPicture); // using to64base bc it prevents data loss
                }
                Timer = Convert.ToBase64String(stu.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                ErrorMessage = "not found";
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                ErrorMessage = "not found";
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // gets by id
        public void GetById()
        {
            try
            {
                Employees stu = _dao.GetById(Id);
                Title = stu.Title;
                Firstname = stu.FirstName;
                Lastname = stu.LastName;
                Phoneno = stu.PhoneNo;
                Email = stu.Email;
                Id = stu.Id;
                DepartmentId = stu.DepartmentId;
                DepartmentName = stu.Department.DepartmentName;
                FullName = stu.Title + " " + stu.FirstName + " " + stu.LastName;
                if (stu.StaffPicture != null)
                {
                    Picture64 = Convert.ToBase64String(stu.StaffPicture); // using to64base bc it prevents data loss
                }
                Timer = Convert.ToBase64String(stu.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                ErrorMessage = "not found";
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                ErrorMessage = "not found";
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // gets all
        public List<EmployeeViewModel> GetAll()
        {
            List<EmployeeViewModel> allVms = new List<EmployeeViewModel>();
            try
            {
                List<Employees> allEmployees = _dao.GetAll();
                foreach (Employees stu in allEmployees)
                {
                    EmployeeViewModel stuVm = new EmployeeViewModel();

                    stuVm.Title = stu.Title;
                    stuVm.Firstname = stu.FirstName;
                    stuVm.Lastname = stu.LastName;
                    stuVm.Phoneno = stu.PhoneNo;
                    stuVm.Email = stu.Email;
                    stuVm.Id = stu.Id;
                    stuVm.DepartmentId = stu.DepartmentId;
                    stuVm.DepartmentName = stu.Department.DepartmentName;
                    stuVm.FullName = stu.Title + " " + stu.FirstName + " " + stu.LastName;
                    stuVm.Timer = Convert.ToBase64String(stu.Timer);
                    stuVm.IsTech = stu.IsTech; 

                    if (stu.StaffPicture != null)
                    {
                        stuVm.Picture64 = Convert.ToBase64String(stu.StaffPicture); // using to64base bc it prevents data loss
                    }

                    allVms.Add(stuVm);
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = "not found";
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
                Employees stu = new Employees();

                stu.Title = Title;
                stu.FirstName = Firstname;
                stu.LastName = Lastname;
                stu.PhoneNo = Phoneno;
                stu.Email = Email;
                stu.DepartmentId = DepartmentId;

                if (Picture64 != null)
                {
                    stu.StaffPicture = Convert.FromBase64String(Picture64);
                }
                Id = _dao.Add(stu);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // update
        public int Update()
        {
            UpdateStatus studentsUpdated = UpdateStatus.Failed;
            try
            {
                Employees stu = new Employees();

                stu.Title = Title;
                stu.FirstName = Firstname;
                stu.LastName = Lastname;
                stu.PhoneNo = Phoneno;
                stu.Email = Email;
                stu.Id = Id;
                stu.DepartmentId = DepartmentId;

                if (Picture64 != null)
                {
                    stu.StaffPicture = Convert.FromBase64String(Picture64);
                }
                stu.Timer = Convert.FromBase64String(Timer);
                studentsUpdated = _dao.Update(stu);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(studentsUpdated);
        }

        // delete
        public int Delete()
        {
            int studentsDeleted = -1;
            try
            {
                studentsDeleted = _dao.Delete(Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return studentsDeleted;
        }
    }
}
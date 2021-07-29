/*
\file:      EmployeeDAO
\author:    Vincent Li
\purpose:   This is the current employee DAO that uses the repository
*/
// Step 3 for making a repository, we call the new implementation
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
    public class EmployeeDAO
    {
        // makes new repo 
        readonly IRepository<Employees> repository;
        public EmployeeDAO()
        {
            repository = new HelpdeskRepository<Employees>();
        }
        
        // gets by lastname
        public Employees GetByLastName(string name)
        {
            try
            {
                return repository.GetByExpression(emp => emp.LastName == name).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // gets by email
        public Employees GetByMail(string email)
        {
            try
            {
                return repository.GetByExpression(emp => emp.Email == email).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // get by id
        public Employees GetById(int id)
        {
            try
            {
                return repository.GetByExpression(emp => emp.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // getall
        public List<Employees> GetAll()
        {
            try
            {
                return repository.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        //add new emp
        public int Add(Employees newEmployee)
        {
            try
            {
                newEmployee = repository.Add(newEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return newEmployee.Id;
        }

        // delete method
        public int Delete(int id)
        {
            int employeeDeleted = -1;
            try
            {
                employeeDeleted = repository.Delete(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return employeeDeleted;
        }

        // update method
        public UpdateStatus Update(Employees updatedEmployee)
        {
            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                operationStatus = repository.Update(updatedEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return operationStatus;
        }
    }
}

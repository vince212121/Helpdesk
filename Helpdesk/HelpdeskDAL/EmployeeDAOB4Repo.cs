/*
\file:      EmployeeDAOB4Repo, this is the original employee dao before the repository was implemented
\author:    Vincent Li
\purpose:   This is the 
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
//using HelpdeskDAL;

namespace HelpdeskDAL
{
    public class EmployeeDAOB4Repo
    {
        // gets employee by last name (even though it is supposed to use email...
        public Employees GetByLastName(string name)
        {
            Employees selectedEmployee = null;
            try
            {
                HelpDeskContext _db = new HelpDeskContext(); // all need to use the HelpDeskContext or we cannot access it
                selectedEmployee = _db.Employees.FirstOrDefault(emp => emp.LastName == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message); // gets the StudentDAO class then the GetByLastName Method
                throw ex; // bubble up
            }
            return selectedEmployee;
        }

        // gets employee by mail
        public Employees GetByMail(string name)
        {
            Employees selectedEmployee = null;
            try
            {
                HelpDeskContext _db = new HelpDeskContext(); // all need to use the HelpDeskContext or we cannot access it
                selectedEmployee = _db.Employees.FirstOrDefault(emp => emp.Email == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message); // gets the StudentDAO class then the GetByLastName Method
                throw ex; // bubble up
            }
            return selectedEmployee;
        }

        // gets employee by id
        public Employees GetById(int id)
        {
            Employees selectedEmployee = null;
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                selectedEmployee = _db.Employees.FirstOrDefault(emp => emp.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex; // bubble up
            }
            return selectedEmployee;
        }

        // gets all employees
        public List<Employees> GetAll()
        {
            List<Employees> allStudents = new List<Employees>();

            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                allStudents = _db.Employees.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex; // bubble up
            }
            return allStudents;
        }

        // add function
        // not doing any business logic to check if it is valid, just hoping/assuming it is correct
        public int Add(Employees newStudent)
        {
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                _db.Employees.Add(newStudent); // adds student
                _db.SaveChanges(); // updates database, without it, it won't commit to the database
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex; // bubble up
            }
            return newStudent.Id;
        }

        // update function
        public int Update(Employees updatedStudent)
        {
            int studentsUpdated = -1; // acts as a false 
            try
            {
                HelpDeskContext _db = new HelpDeskContext();

                // go get the current value first
                Employees currentStudent = _db.Employees.FirstOrDefault(emp => emp.Id == updatedStudent.Id);

                // overwrite that entity with whatever it is given
                _db.Entry(currentStudent).CurrentValues.SetValues(updatedStudent);
                studentsUpdated = _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex; // bubble up
            }
            return studentsUpdated;
        }

        
        // delete function
        public int Delete(int id) // should return 1 bc we are deleting 1 student
        {
            int studentsDeleted = -1;
            try
            {
                HelpDeskContext _db = new HelpDeskContext();
                Employees selectedEmployee = _db.Employees.FirstOrDefault(emp => emp.Id == id); // or GetById(id), only problem with this is that it will create another HelpDeskContext
                _db.Employees.Remove(selectedEmployee);
                studentsDeleted = _db.SaveChanges(); // returns # of rows removed
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex; // bubble up
            }
            return studentsDeleted;
        }
    }
}

/*
\file:      Employees
\author:    Vincent Li
\purpose:   This file is used as the attributes for the employee, and since it is inheriting from the entity, it does not need timer or id
*/
using System;
using System.Collections.Generic;

namespace HelpdeskDAL
{
    public partial class Employees : HelpdeskEntity // changed for lab 8
    {
        // added from lab 16, not needed
        //public Employees()
        //{
        //    CallsEmployee = new HashSet<Calls>();
        //    CallsTech = new HashSet<Calls>();
        //}

        //public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public bool? IsTech { get; set; }
        public byte[] StaffPicture { get; set; }
        //public byte[] Timer { get; set; }

        public virtual Departments Department { get; set; }

        // added from lab 16, not needed
        //public virtual ICollection<Calls> CallsEmployee { get; set; }
        //public virtual ICollection<Calls> CallsTech { get; set; }
    }
}

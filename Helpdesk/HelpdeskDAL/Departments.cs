/*
\file:      Departments
\author:    Vincent Li
\purpose:   This is the departments class file that inherits from the entity 
*/
using System;
using System.Collections.Generic;

namespace HelpdeskDAL
{
    public partial class Departments : HelpdeskEntity // changed for lab 8
    {
        public Departments()
        {
            Employees = new HashSet<Employees>();
        }

        //public int Id { get; set; }
        public string DepartmentName { get; set; }
        //public byte[] Timer { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}

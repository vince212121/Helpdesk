/*
\file:      problem
\author:    Vincent Li
\purpose:   This is used as the problems the customers have
*/
using System;
using System.Collections.Generic;

namespace HelpdeskDAL
{
    public partial class Problems : HelpdeskEntity // changed for lab 8
    {
        // added from lab 16, not needed
        //public Problems()
        //{
        //    Calls = new HashSet<Calls>();
        //}

        //public int Id { get; set; }
        public string Description { get; set; }
        //public byte[] Timer { get; set; }

        // added from lab 16, not needed
        //public virtual ICollection<Calls> Calls { get; set; }
    }
}

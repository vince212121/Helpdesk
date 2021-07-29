/*
\file:      employeelookup
\author:    Vincent Li
\purpose:   This is the lab 8 case study part that created different things to replace 1, -1, and -2 for update to readable things like
            Ok, Failed, and Stale. This is very useful for when there are more things added
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpdeskDAL
{
    public enum UpdateStatus // the enum lets you use the words here instead of numbers so it is easier to remember
    {
        Ok = 1, // checking for Ok instead of 1, Failed instead of -1, and so on
        Failed = -1,
        Stale = -2 // concurrency problem 
    }
}

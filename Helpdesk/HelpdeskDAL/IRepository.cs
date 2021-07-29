/*
\file:      IRepository
\author:    Vincent Li
\purpose:   This is the repository functions used for employees and stuff 
*/

// some notes about repositories:
// Any interface must have a captial I
// interfaces are just method signatures for what they will do, no implementation code
// Step 1 for creating a repository
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HelpdeskDAL
{
    public interface IRepository<T> // either passing a student type or division type
    {
        List<T> GetAll(); // the T is a generic that can recieve different types
        List<T> GetByExpression(Expression<Func<T, bool>> match);
        T Add(T entity);
        UpdateStatus Update(T entity);
        int Delete(int i);
    }
}

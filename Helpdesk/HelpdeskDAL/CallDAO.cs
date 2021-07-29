/*
\file:      callDAO
\author:    Vincent Li
\purpose:   This is the call DAO class tier
*/

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
    public class CallDAO
    {
        private IRepository<Calls> repo;

        public CallDAO()
        {
            repo = new HelpdeskRepository<Calls>();
        }

        // get by id
        public Calls GetById(int Id)
        {
            List<Calls> selectedCalls = null;

            try
            {
                selectedCalls = repo.GetByExpression(cll => cll.Id == Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return selectedCalls.FirstOrDefault();
        }

        // get all
        public List<Calls> GetAll()
        {
            List<Calls> allCalls = new List<Calls>();

            try
            {
                allCalls = repo.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return allCalls;
        }

        // add
        public int Add(Calls call)
        {
            try
            {
                call = repo.Add(call);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return call.Id;
        }

        //delete
        public int Delete(int id)
        {
            int callDeleted = -1;

            try
            {
                callDeleted = repo.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return callDeleted;
        }

        // update
        public UpdateStatus Update(Calls updatedCall)
        {
            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                operationStatus = repo.Update(updatedCall);
            }
            catch (DbUpdateConcurrencyException dex)
            {
                operationStatus = UpdateStatus.Stale;
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + dex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return operationStatus;
        }
    }
}

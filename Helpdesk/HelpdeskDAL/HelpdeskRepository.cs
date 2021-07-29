/*
\file:      HelpdeskRepository
\author:    Vincent Li
\purpose:   This is the file that actually implements the repository stuff from the IRepository file
*/
//Step 2 for repository (making the implementaion)
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace HelpdeskDAL
{
    public class HelpdeskRepository<T> : IRepository<T> where T : HelpdeskEntity
    {
        readonly private HelpDeskContext _db = null;

        // setting to new context
        public HelpdeskRepository(HelpDeskContext context = null)
        {
            _db = context ?? new HelpDeskContext();
        }

        // gets all
        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        // get by expression
        public List<T> GetByExpression(Expression<Func<T, bool>> match)
        {
            return _db.Set<T>().Where(match).ToList();
        }

        // add method
        public T Add(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
            return entity;
        }

        // update method
        public UpdateStatus Update(T updatedEntity)
        {
            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                HelpdeskEntity currentEntity = GetByExpression(ent => ent.Id == updatedEntity.Id).FirstOrDefault();
                _db.Entry(currentEntity).OriginalValues["Timer"] = updatedEntity.Timer;
                _db.Entry(currentEntity).CurrentValues.SetValues(updatedEntity);
                if (_db.SaveChanges() == 1) // should throw exception if stale
                {
                    operationStatus = UpdateStatus.Ok;
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // the new catch when the timers are not the same
                // basically when someone else beats you to updating, it will be stale data since it is not updated
                operationStatus = UpdateStatus.Stale;
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return operationStatus;
        }

        // delete method
        public int Delete(int id)
        {
            T currentEntity = GetByExpression(ent => ent.Id == id).FirstOrDefault();
            _db.Set<T>().Remove(currentEntity);
            return _db.SaveChanges();
        }
    }
}

﻿using Base.DAL;
using System;
using System.Linq;

namespace Base.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        private readonly ICheckAccessService _accessService;
        public BaseService(ICheckAccessService accessService)
        {
            _accessService = accessService;
        }
        public void ChangeSortOrder(IUnitOfWork unitOfWork, T obj, int posId)
        {
            double oldOrder = obj.SortOrder;

            var repository = unitOfWork.GetRepository<T>();

            var updateObj = repository.All().FirstOrDefault(x => x.ID == obj.ID);

            if (updateObj == null) return;

            double newOrder = repository.All().Where(x => x.ID == posId).Select(x => x.SortOrder).FirstOrDefault();

            if (newOrder > oldOrder)
            {
                double order = repository.All().Where(x => x.SortOrder > newOrder)
                    .Select(x => x.SortOrder).DefaultIfEmpty(-1).Min();

                if (order == -1)
                    updateObj.SortOrder = GetMaxSortOrder(unitOfWork) + 1;
                else
                    updateObj.SortOrder = newOrder + (order - newOrder) / 2;
            }
            else
            {
                double order = repository.All().Where(x => x.SortOrder < newOrder)
                    .Select(x => x.SortOrder).DefaultIfEmpty(-1).Max();

                if (order == -1)
                    updateObj.SortOrder = GetMinSortOrder(unitOfWork) - 1;
                else
                    updateObj.SortOrder = newOrder - (newOrder - order) / 2;
            }

            repository.Update(updateObj);
            unitOfWork.SaveChanges();
        }

        public T Create(IUnitOfWork unitOfWork, T obj)
        {
            _accessService.ThrowIfAccessDenied(unitOfWork, Enums.AccessModifier.Create, typeof(T));

            InitSortOrder(unitOfWork, obj);
            unitOfWork.GetRepository<T>().Create(obj);
            unitOfWork.SaveChanges();
            return obj;
        }

        public T CreateDefault(IUnitOfWork unitOfWork)
        {
            return Activator.CreateInstance<T>();
        }

        public void Delete(IUnitOfWork unitOfWork, T obj, bool setHidden = true)
        {
            _accessService.ThrowIfAccessDenied(unitOfWork, Enums.AccessModifier.Delete, typeof(T));

            if (setHidden)
                unitOfWork.GetRepository<T>().ChangeProperty(obj.ID, x => x.Hidden, true);
            else
                unitOfWork.GetRepository<T>().Delete(obj);

            unitOfWork.SaveChanges();

        }

        public void Delete(IUnitOfWork unitOfWork, int id, bool setHidden = true)
        {
            _accessService.ThrowIfAccessDenied(unitOfWork, Enums.AccessModifier.Delete, typeof(T));

            if (setHidden)
                unitOfWork.GetRepository<T>().ChangeProperty(id, x => x.Hidden, true);
            else
            {
                var obj = unitOfWork.GetRepository<T>().All().Where(x => x.ID == id).Single();
                unitOfWork.GetRepository<T>().Delete(obj);
            }

            unitOfWork.SaveChanges();

        }

        public IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool hidden = false)
        {
            if(!typeof(T).GetInterfaces().Contains(typeof(IClientEntity)))
                _accessService.ThrowIfAccessDenied(unitOfWork, Enums.AccessModifier.Read, typeof(T));

            IQueryable<T> q = unitOfWork.GetRepository<T>().All();
            if (hidden)
                q = q.Where(a => a.Hidden == true);
            else
                q = q.Where(a => a.Hidden == false);
            return q.OrderByDescending(x => x.ID);
        }

        public T Update(IUnitOfWork unitOfWork, T obj)
        {
            _accessService.ThrowIfAccessDenied(unitOfWork, Enums.AccessModifier.Update, typeof(T));

            if (obj.ID == 0) return this.Create(unitOfWork, obj);
            unitOfWork.GetRepository<T>().Update(obj);
            unitOfWork.SaveChanges();
            return obj;
        }


        protected virtual void InitSortOrder(IUnitOfWork unitOfWork, T obj)
        {
            if (obj.SortOrder != -1) return;

            obj.SortOrder = GetMaxSortOrder(unitOfWork) + 1;
        }

        protected virtual double GetMaxSortOrder(IUnitOfWork unitOfWork)
        {
            return unitOfWork.GetRepository<T>().All().Select(x => x.SortOrder).DefaultIfEmpty(0).Max();
        }

        protected virtual double GetMinSortOrder(IUnitOfWork unitOfWork)
        {
            return unitOfWork.GetRepository<T>().All().Select(x => x.SortOrder).DefaultIfEmpty(0).Min();
        }
    }
}

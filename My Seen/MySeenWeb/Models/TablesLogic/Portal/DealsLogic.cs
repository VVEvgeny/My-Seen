using System;
using System.Linq;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables.Portal;
using MySeenWeb.Models.TablesLogic.Base;
using System.Collections.Generic;
using MySeenResources;

namespace MySeenWeb.Models.TablesLogic.Portal
{
    public class DealsLogic : Deals, IBaseLogic
    {
        private readonly ICacheService _cache;
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;

        //пустой конструктор надо от.к. отнаследован от класса таблицы энтити
        public DealsLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }

        public DealsLogic(ICacheService cache) : this()
        {
            _cache = cache;
        }

        public bool Add(string dateTime, string other)
        {
            return Fill(dateTime, other) && Verify() && Add();
        }

        public static List<Deals> GetAll(ICacheService cache)
        {
            return new DealsLogic(cache).GetAll();
        }

        public List<Deals> GetAll()
        {
            var deals = _cache.Get<List<Deals>>(CacheNames.Deals.ToString());
            if (deals == null)
            {
                deals = _ac.Deals.AsNoTracking().ToList();
                _cache.Set(CacheNames.Deals.ToString(), deals, 15);
            }
            return deals;
        }
        private bool Contains()
        {
            return GetAll().Any(f => f.Month == Month && f.Year == Year);
        }
        private bool Verify()
        {
            if (Contains()) ErrorMessage = Resource.AlreadyExists;
            else return true;

            return false;
        }
        private bool Fill(string dateTime, string other)
        {
            try
            {
                Year = Convert.ToInt32(dateTime.Split('-')[1]);
                Month = Convert.ToInt32(dateTime.Split('-')[0]);
                Amount = Convert.ToInt32(other);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return true;
        }
        private bool Add()
        {
            try
            {
                _ac.Deals.Add(this);
                _ac.SaveChanges();
                _cache.Remove(CacheNames.Deals.ToString());
            }
            catch (Exception e)
            {
                ErrorMessage = $"{Resource.ErrorWorkWithDB} = {e.Message}";
                return false;
            }
            return true;
        }

        public string GetError()
        {
            return ErrorMessage;
        }

        public bool Delete(string id, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
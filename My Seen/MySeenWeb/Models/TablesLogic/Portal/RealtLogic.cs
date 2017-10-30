using System;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables.Portal;
using MySeenWeb.Models.TablesLogic.Base;
using System.Collections.Generic;
using System.Linq;
using MySeenLib;

namespace MySeenWeb.Models.TablesLogic.Portal
{
    public class RealtLogic : Realt, IBaseLogic
    {
        private readonly ICacheService _cache;
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;

        public RealtLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
        public RealtLogic(ICacheService cache): this()
        {
            _cache = cache;
        }

        public static List<Realt> GetAll(ICacheService cache)
        {
            return new RealtLogic(cache).GetAll();
        }
        public List<Realt> GetAll()
        {
            var realt = _cache.Get<List<Realt>>(CacheNames.Realt.ToString());
            if (realt == null)
            {
                realt = _ac.Realt.AsNoTracking().ToList();
                _cache.Set(CacheNames.Realt.ToString(), realt, 15);
            }
            return realt;
        }

        public bool Add(string dateTime, string other)
        {
            return Fill(dateTime, other) && Verify() && Add();
        }

        private bool Contains()
        {
            return GetAll().Any(f => f.Date == Date);
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
                var dateTmp = Convert.ToDateTime(dateTime);
                Date = new DateTime(dateTmp.Year, dateTmp.Month, dateTmp.Day);
                Count = Convert.ToInt32(other.Split('-')[0]);
                Price = Convert.ToInt32(other.Split('-')[1]);
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
                _ac.Realt.Add(this);
                _ac.SaveChanges();
                _cache.Remove(CacheNames.Realt.ToString());
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
            throw new System.NotImplementedException();
        }
    }
}
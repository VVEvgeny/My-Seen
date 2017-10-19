using System;
using System.Linq;
using MySeenLib;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables.Portal;
using MySeenWeb.Models.TablesLogic.Base;
using static MySeenLib.UmtTime;

namespace MySeenWeb.Models.TablesLogic.Portal
{
    public class MemesLogic : Memes, IBaseLogic
    {
        private readonly ApplicationDbContext _ac;
        private readonly ICacheService _cache;
        public string ErrorMessage;

        public MemesLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
        public MemesLogic(ICacheService cache) : this()
        {
            _cache = cache;
        }

        public Memes Get(int id)
        {
            return _ac.Memes.Any(f => f.Id == id) ? _ac.Memes.FirstOrDefault(f => f.Id == id) : new Memes();
        }

        private bool Fill(string text, string link, string userId)
        {
            try
            {
                Name = text;
                Date = To(DateTime.Now);
                Image = link;
                UserId = userId;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return true;
        }

        private bool Contains()
        {
            return _ac.Memes.Any(f => f.Image == Image);
        }

        private bool Verify()
        {
            if (Contains()) ErrorMessage = Resource.AlreadyExists;
            else return true;

            return false;
        }

        private bool Add()
        {
            try
            {
                _ac.Memes.Add(this);
                _ac.SaveChanges();
                _cache.Remove(CacheNames.Memes.ToString());
            }
            catch (Exception e)
            {
                ErrorMessage = $"{Resource.ErrorWorkWithDB} = {e.Message}";
                return false;
            }
            return true;
        }

        public bool Add(string text, string link, string userId)
        {
            return Fill(text, link, userId) && Verify() && Add();
        }

        public string GetError()
        {
            return ErrorMessage;
        }

        public bool Delete(string id, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
                _ac.MemesStats.RemoveRange(_ac.MemesStats.Where(s => s.MemesId == Id));
                _ac.Memes.RemoveRange(_ac.Memes.Where(b => b.Id == Id));
                _ac.SaveChanges();
                _cache.Remove(CacheNames.Memes.ToString());
            }
            catch (Exception e)
            {
                ErrorMessage = $"{Resource.ErrorWorkWithDB} = {e.Message}";
                return false;
            }
            return true;
        }
    }
}
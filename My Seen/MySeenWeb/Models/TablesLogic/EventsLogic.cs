using System;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesLogic
{
    public class EventsLogic : Events
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;
        public EventsLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
        private bool Fill(string name, string datetime, string type, string userId)
        {
            try
            {
                Name = name;
                Date = UmtTime.To(Convert.ToDateTime(datetime));
                RepeatType = Convert.ToInt32(type);                
                DateChange = UmtTime.To(DateTime.Now);
                UserId = userId;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return true;
        }
        private bool Fill(string id, string name, string datetime, string type, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return Fill(name,  datetime, type, userId);
        }
        private bool Contains()
        {
            return _ac.Events.Any(f => f.Name == Name && f.UserId == UserId && f.Id != Id);
        }
        private bool Verify()
        {
            if (string.IsNullOrEmpty(Name))
            {
                ErrorMessage = Resource.EnterEventName;
            }
            if (string.IsNullOrEmpty(Defaults.EventTypes.GetById(RepeatType)))
            {
                ErrorMessage = Resource.IncorrectEventType;
            }
            else if (Contains())
            {
                ErrorMessage = Resource.EventNameAlreadyExists;
            }
            else return true;

            return false;
        }
        private bool Add()
        {
            try
            {
                _ac.Events.Add(this);
                _ac.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }
        private bool Update()
        {
            try
            {
                var film = _ac.Events.First(f => f.UserId == UserId && f.Id == Id);
                film.Name = Name;
                film.Date = Date;
                film.DateChange = DateChange;
                film.RepeatType = RepeatType;
                _ac.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }
        public bool Add(string name, string datetime, string type, string userId)
        {
            return Fill(name, datetime, type, userId) && Verify() && Add();
        }
        public bool Update(string id, string name, string datetime, string type, string userId)
        {
            return Fill(id, name, datetime, type, userId) && Verify() && Update();
        }
        public bool Delete(string id, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
                _ac.Events.RemoveRange(_ac.Events.Where(f => f.UserId == userId && f.Id == Id));
                _ac.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }
        public string GetShare(string id, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
                if (_ac.Events.First(f => f.UserId == userId && f.Id == Id).Shared)
                {
                    var key = _ac.Users.First(t => t.Id == userId).ShareEventsKey;
                    return MySeenWebApi.ApiHost + MySeenWebApi.ShareEvents + key;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
            }
            return "-";
        }
        public string GenerateShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();
            var iid = Convert.ToInt32(id);
            var key = ac.Users.First(t => t.Id == userId).ShareEventsKey;
            ac.Events.First(e => e.Id == iid).Shared = true;
            ac.SaveChanges();
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareEvents + key;
        }
        public string DeleteShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();
            var iid = Convert.ToInt32(id);
            ac.Events.First(e => e.Id == iid).Shared = false;
            ac.SaveChanges();
            return "-";
        }
    }
}

using System;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesLogic
{
    public class TracksLogic : Tracks
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;

        public TracksLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }

        private bool Fill(string name, string datetime, string type, string coordinates, string distance, string userId)
        {
            try
            {
                Name = name;
                Date = UmtTime.To(Convert.ToDateTime(datetime));
                Type = Convert.ToInt32(type);
                Coordinates = coordinates;
                if (distance.Contains('.')) distance = distance.Remove(distance.IndexOf('.'));//Только кол-во КМ запишем
                Distance = Convert.ToDouble(distance);
                UserId = userId;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return true;
        }
        private bool Fill(string id, string name, string datetime, string type, string coordinates, string distance, string userId)
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
            return Fill(name, datetime, type, coordinates, distance, userId);
        }

        private bool Verify()
        {
            if (string.IsNullOrEmpty(Name)) ErrorMessage = Resource.ShortName;
            else if (Coordinates.Length == 0) ErrorMessage = Resource.NoCoordinates;
            else if (Distance == 0) ErrorMessage = Resource.ErrorCalculating;
            else return true;
            return false;
        }

        private bool Add()
        {
            try
            {
                _ac.Tracks.Add(this);
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
                var elem = _ac.Tracks.First(t => t.Id == Id && t.UserId == UserId);
                elem.Name = Name;
                elem.Type = Type;
                elem.Coordinates = Coordinates;
                elem.Date = Date;
                elem.Distance = Distance;
                _ac.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }
        public bool Add(string name, string datetime, string type, string coordinates, string distance, string userId)
        {
            return Fill(name, datetime, type, coordinates, distance, userId) && Verify() && Add();
        }
        public bool Update(string id, string name, string datetime, string type, string coordinates, string distance, string userId)
        {
            return Fill(id, name, datetime, type, coordinates, distance, userId) && Verify() && Update();
        }
    }
}

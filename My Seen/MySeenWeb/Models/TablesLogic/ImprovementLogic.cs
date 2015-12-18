using System;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesLogic
{
    public class ImprovementLogic : Bugs
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;
        public ImprovementLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
        private bool Fill(string text, string complex, string userId)
        {
            try
            {
                Text = text;
                DateFound = UmtTime.To(DateTime.Now);
                Complex = Convert.ToInt32(complex);
                UserId = userId;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return true;
        }
        private bool Fill(string id, string desc, string version, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
                TextEnd = desc;
                DateEnd = UmtTime.To(DateTime.Now);
                Version = Convert.ToInt32(version);
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
            return _ac.Bugs.Any(f => f.Text == Text && f.UserId == UserId && f.Id != Id);
        }
        private bool Verify()
        {
            if (Id == 0 && string.IsNullOrEmpty(Text)) ErrorMessage = Resource.DescToShort;
            else if (Contains()) ErrorMessage = Resource.BugAlreadyExists;
            else if (Id != 0 && string.IsNullOrEmpty(TextEnd)) ErrorMessage = Resource.DescToShort;
            else return true;

            return false;
        }
        private bool Add()
        {
            try
            {
                _ac.Bugs.Add(this);
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
                var elem = _ac.Bugs.First(f => f.UserId == UserId && f.Id == Id);
                elem.TextEnd = TextEnd;
                elem.DateEnd = DateEnd;
                elem.Version = Version;
                _ac.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }
        public bool Add(string text, string complex, string userId)
        {
            return Fill(text, complex, userId) && Verify() && Add();
        }
        public bool Update(string id, string desc, string version, string userId)
        {
            return Fill(id, desc, version, userId) && Verify() && Update();
        }
        public bool Delete(string id, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
                _ac.Bugs.RemoveRange(_ac.Bugs.Where(b => b.Id == Id));
                _ac.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }
    }
}

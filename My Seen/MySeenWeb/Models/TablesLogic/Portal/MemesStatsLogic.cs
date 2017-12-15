using System;
using System.Linq;
using MySeenResources;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables.Portal;

namespace MySeenWeb.Models.TablesLogic.Portal
{
    public class MemesStatsLogic : MemesStats
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;

        public MemesStatsLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }

        public bool AddRate(string id, string userId, bool plus)
        {
            try
            {
                Id = Convert.ToInt32(id);
                if (!_ac.MemesStats.Any(f => f.UserId == userId && f.Id == Id))
                {
                    UserId = userId;
                    MemesId = Id;
                    Plus = plus;
                    Minus = !plus;

                    _ac.MemesStats.Add(this);
                    _ac.SaveChanges();
                }
                else
                {
                    ErrorMessage = Resource.NoData;
                    return false;
                }
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
using System;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesLogic
{
    public class NLogErrorsLogic : Bugs
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;
        public NLogErrorsLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
        
        public bool Delete(string id, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
                _ac.NLogErrors.RemoveRange(_ac.NLogErrors.Where(b => b.Id == Id));
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

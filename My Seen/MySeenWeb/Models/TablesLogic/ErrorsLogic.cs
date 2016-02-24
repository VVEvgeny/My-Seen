using System;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesLogic
{
    public class ErrorsLogic : Bugs
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;
        public ErrorsLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
        public bool RemoveAll()
        {
            try
            {
                _ac.NLogErrors.RemoveRange(_ac.NLogErrors);
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

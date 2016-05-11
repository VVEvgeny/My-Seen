using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables.Portal;

namespace MySeenWeb.Models.TablesLogic.Portal
{
    public class RealtLogic : Realt
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;

        public RealtLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
    }
}
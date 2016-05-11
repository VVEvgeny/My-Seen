using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables.Portal;

namespace MySeenWeb.Models.TablesLogic.Portal
{
    public class SalaryLogic : Salary
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;

        public SalaryLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
    }
}
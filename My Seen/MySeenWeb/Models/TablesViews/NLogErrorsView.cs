using System.Globalization;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesViews
{
    public class NLogErrorsView :NLogErrors
    {
        public static NLogErrorsView Map(NLogErrors model)
        {
            if (model == null) return new NLogErrorsView();

            return new NLogErrorsView
            {
                Id = model.Id,
                DateTimeStamp = model.DateTimeStamp,
                Host = model.Host,
                Message = model.Message,
                Level = model.Level,
                StackTrace = model.StackTrace,
                Variables = model.Variables
            };
        }

        public string DateTimeStampText
        {
            get { return DateTimeStamp.ToString(CultureInfo.CurrentCulture); }
        }
    }
}

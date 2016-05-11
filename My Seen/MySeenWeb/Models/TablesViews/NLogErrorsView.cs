using System.Globalization;
using MySeenWeb.Models.Tables;
using static MySeenLib.UmtTime;

namespace MySeenWeb.Models.TablesViews
{
    public class NLogErrorsView : NLogErrors
    {
        public string DateTimeStampText => DateTimeStamp.ToString(CultureInfo.CurrentCulture);

        public static NLogErrorsView Map(NLogErrors model)
        {
            if (model == null) return new NLogErrorsView();

            return new NLogErrorsView
            {
                Id = model.Id,
                DateTimeStamp = From(model.DateTimeStamp),
                Host = model.Host,
                Message = model.Message,
                Level = model.Level,
                StackTrace = model.StackTrace,
                Variables = model.Variables
            };
        }
    }
}
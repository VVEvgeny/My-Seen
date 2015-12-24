using System.Globalization;
using System.Text;
using MySeenLib;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

namespace MySeenWeb.Add_Code.Services.Logging.NLog
{
    [LayoutRenderer("utc_date")]
    public class UtcDateRenderer : LayoutRenderer
    {
        public CultureInfo Culture { get; set; }
        
        public UtcDateRenderer()
        {
            Culture = new CultureInfo(CultureInfoTool.Cultures.Russian);
        }

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(UmtTime.To(logEvent.TimeStamp).ToString(Culture));
        }

    }
}
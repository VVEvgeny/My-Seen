using System;
using System.Globalization;
using System.Text;
using MySeenLib;
using NLog;
using NLog.LayoutRenderers;

namespace MySeenWeb.Add_Code.Services.Logging.NLog
{
    [LayoutRenderer("utc_date")]
    public class UtcDateRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            /*
            var time = string.Empty;
            try
            {
                time = UmtTime.To(logEvent.TimeStamp).ToString(new CultureInfo(CultureInfoTool.Cultures.English));
            }
            catch (Exception)
            {
                time = string.Empty;
            }
            if (string.IsNullOrEmpty(time))
            {
                try
                {
                    time = UmtTime.To(logEvent.TimeStamp).ToString(new CultureInfo(CultureInfoTool.Cultures.Russian));
                }
                catch (Exception)
                {
                    time = string.Empty;
                }
            }
            */
            builder.Append(UmtTime.To(logEvent.TimeStamp).ToString(CultureInfo.CurrentCulture));
        }
    }
}
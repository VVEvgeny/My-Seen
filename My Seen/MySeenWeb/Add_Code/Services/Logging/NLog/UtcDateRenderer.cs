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
            try
            {
                builder.Append(UmtTime.To(logEvent.TimeStamp));
            }
            catch (Exception)
            {
                try
                {
                    builder.Append(UmtTime.To(logEvent.TimeStamp));
                }
                catch (Exception)
                {
                    try
                    {
                        builder.Append(UmtTime.To(DateTime.Now));
                    }
                    catch (Exception)
                    {
                        builder.Append(UmtTime.To(DateTime.Now));
                    }
                }
            }
        }
    }
}
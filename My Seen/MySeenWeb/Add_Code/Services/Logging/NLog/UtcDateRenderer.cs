using System;
using System.Text;
using NLog;
using NLog.LayoutRenderers;
using static MySeenLib.UmtTime;

namespace MySeenWeb.Add_Code.Services.Logging.NLog
{
    [LayoutRenderer("utc_date")]
    public class UtcDateRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            try
            {
                builder.Append(To(logEvent.TimeStamp));
            }
            catch (Exception)
            {
                try
                {
                    builder.Append(To(logEvent.TimeStamp));
                }
                catch (Exception)
                {
                    try
                    {
                        builder.Append(To(DateTime.Now));
                    }
                    catch (Exception)
                    {
                        builder.Append(To(DateTime.Now));
                    }
                }
            }
        }
    }
}
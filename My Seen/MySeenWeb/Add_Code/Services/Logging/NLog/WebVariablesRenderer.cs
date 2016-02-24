using System.Globalization;
using System.Text;
using System.Web;
using System.Xml;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

namespace MySeenWeb.Add_Code.Services.Logging.NLog
{
    [LayoutRenderer("web_variables")]
    public class WebVariablesRenderer : LayoutRenderer
    {

        ///
        /// Initializes a new instance of the  class.
        ///
        public WebVariablesRenderer()
        {
            this.Format = "";
            this.Culture = CultureInfo.CurrentCulture;
        }

        protected int GetEstimatedBufferSize(LogEventInfo ev)
        {
            // This will be XML of an unknown size
            return 10000;
        }

        ///
        /// Gets or sets the culture used for rendering.
        ///
        ///
        public CultureInfo Culture { get; set; }

        ///
        /// Gets or sets the date format. Can be any argument accepted by DateTime.ToString(format).
        ///
        ///
        [DefaultParameter]
        public string Format { get; set; }

        ///
        /// Renders the current date and appends it to the specified .
        ///
        /// <param name="builder">The  to append the rendered data to.
        /// <param name="logEvent">Logging event.
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var sb = new StringBuilder();
            var writer = XmlWriter.Create(sb);

            writer.WriteStartElement("error");

            // -----------------------------------------
            // Server Variables
            // -----------------------------------------
            writer.WriteStartElement("serverVariables");

            foreach (var key in HttpContext.Current.Request.ServerVariables.AllKeys)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("name", key);

                writer.WriteStartElement("value");
                writer.WriteAttributeString("string", HttpContext.Current.Request.ServerVariables[key].ToString());
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            // -----------------------------------------
            // Cookies
            // -----------------------------------------
            writer.WriteStartElement("cookies");

            foreach (var key in HttpContext.Current.Request.Cookies.AllKeys)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("name", key);

                writer.WriteStartElement("value");
                writer.WriteAttributeString("string", HttpContext.Current.Request.Cookies[key].Value.ToString());
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            // -----------------------------------------

            writer.WriteEndElement();
            // -----------------------------------------

            writer.Flush();
            writer.Close();

            var xml = sb.ToString();

            builder.Append(xml);
        }

    }
}
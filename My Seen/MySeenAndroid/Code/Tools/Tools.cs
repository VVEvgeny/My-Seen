using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MySeenAndroid
{
    public enum States
    {
        Films,
        Serials
    }
    public static class HTMLTool
    {
        public static string HtmlEncode(string s)
        {
            if (s == null)
            {
                return null;
            }

            var result = new StringBuilder(s.Length);

            foreach (char ch in s)
            {
                if (ch <= '>')
                {
                    switch (ch)
                    {
                        case '<':
                            result.Append("&lt;");
                            break;

                        case '>':
                            result.Append("&gt;");
                            break;

                        case '"':
                            result.Append("&quot;");
                            break;

                        case '\'':
                            result.Append("&#39;");
                            break;

                        case '&':
                            result.Append("&amp;");
                            break;

                        default:
                            result.Append(ch);
                            break;
                    }
                }
                else if (ch >= 160 && ch < 256)
                {
                    result.Append("&#").Append(((int)ch).ToString(CultureInfo.InvariantCulture)).Append(';');
                }
                else
                {
                    result.Append(ch);
                }
            }

            return result.ToString();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static MySeenLib.Defaults;

namespace MySeenWeb.Models.Prepared
{
    public class PreparedDataLogs
    {
        public IEnumerable<SelectListItem> LanguagesList { get; } =
            Languages.GetAll()
                .Select(
                    sel =>
                        new SelectListItem
                        {
                            Text = sel,
                            Value = Languages.GetId(sel).ToString(),
                            Selected = Languages.GetId(sel) == 0
                        })
                .ToList();
    }
}
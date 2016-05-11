using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using static MySeenLib.CultureInfoTool;
using static MySeenLib.Defaults;

namespace MySeenWeb.Models.TablesViews
{
    public class UsersView
    {
        public string Name { get; set; }
        public string Culture { get; set; }
        public int FilmsCount { get; set; }
        public int SerialsCount { get; set; }
        public int BooksCount { get; set; }
        public int TracksCount { get; set; }
        public int EventsCount { get; set; }
        public string RegisterDate { get; set; }
        public DateTime LastAction { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public string LastActionText => LastAction.ToString(CultureInfo.CurrentCulture);

        public static UsersView Map(ApplicationUser model)
        {
            if (model == null) return new UsersView();

            var ap = new ApplicationDbContext();

            return new UsersView
            {
                Name = model.UserName,
                RegisterDate = model.RegisterDate.ToShortDateString(),
                Culture = model.Culture == Cultures.English
                    ? Languages.GetById((int) LanguagesBase.Indexes.English)
                    : Languages.GetById((int) LanguagesBase.Indexes.Russian),
                FilmsCount = ap.Films.Count(f => f.UserId == model.Id),
                SerialsCount = ap.Serials.Count(f => f.UserId == model.Id),
                BooksCount = ap.Books.Count(f => f.UserId == model.Id),
                TracksCount = ap.Tracks.Count(f => f.UserId == model.Id),
                EventsCount = ap.Events.Count(f => f.UserId == model.Id),
                LastAction =
                    ap.Logs.Any(l => l.UserId == model.Id)
                        ? ap.Logs.Where(l => l.UserId == model.Id).Max(l => l.DateLast)
                        : model.RegisterDate,
                Roles = ap.UserRoles.Where(r => r.UserId == model.Id).Select(r => r.RoleId)
            };
        }
    }
}
using MySeenWeb.Models.Tables.Portal;

namespace MySeenWeb.Models.TablesViews.Portal
{
    public class RealtView : Realt
    {
        public string DateText
        {
            get { return Date.Day + "/" + Date.Month + "/" + Date.Year; }
        }

        public static RealtView Map(Realt model)
        {
            if (model == null) return new RealtView();

            return new RealtView
            {
                Id = model.Id,
                Date = model.Date,
                Price = model.Price,
                Count = model.Count
            };
        }
    }
}
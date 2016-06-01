using MySeenLib;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.TablesLogic.Portal;

namespace MySeenWeb.Models.TablesLogic.Base
{
    public static class BaseLogicFactory
    {
        public static IBaseLogic Create(int pageId, ICacheService cache)
        {
            switch (pageId)
            {
                case (int) Defaults.CategoryBase.IndexesMain.Memes:
                    return new MemesLogic(cache);
                case (int) Defaults.CategoryBase.IndexesExt.Improvements:
                    return new ImprovementLogic();
                case (int) Defaults.CategoryBase.Indexes.Events:
                    return new EventsLogic();
                case (int) Defaults.CategoryBase.Indexes.Roads:
                    return new RoadsLogic(cache);
                case (int) Defaults.CategoryBase.Indexes.Books:
                    return new BooksLogic(cache);
                case (int) Defaults.CategoryBase.Indexes.Serials:
                    return new SerialsLogic(cache);
                case (int) Defaults.CategoryBase.Indexes.Films:
                    return new FilmsLogic(cache);
            }
            return null;
        }
    }
}
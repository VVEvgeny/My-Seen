using System;
using System.Web;
using MySeenLib;
using MySeenWeb.Models.Tables;
using MySeenWeb.Models.TablesLogic;

namespace MySeenWeb.Models.Meta.Shared
{
    public class MetaSharedRoads : MetaBase
    {
        public static string Path = "/mymemory/roads/shared/";
        public MetaSharedRoads(HttpRequestBase request)
            : base(request)
        {
            Title = Resource.Roads + " " + Resource.User2;
            try
            {
                var logic = new RoadsLogic();
                if (logic.IsSingle(request.Path.Split('/')[request.Path.Split('/').Length - 1]))//Ссылка на одичный трек, покажем название, расстояние и дату
                {
                    var item = logic.GetOne(request.Path.Split('/')[request.Path.Split('/').Length - 1]);
                    Description = Resource.Name + ": " + item.Name + "; " + Resource.DateFound + ": " + item.Date + "; ";
                }
                else
                {
                    Description = Resource.Total + ": " + logic.GetCountShared(request.Path.Split('/')[request.Path.Split('/').Length - 1]);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}

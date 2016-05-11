using System;
using System.Web;
using MySeenLib;
using MySeenWeb.Models.TablesLogic;

namespace MySeenWeb.Models.Meta.Shared
{
    public class MetaSharedBooks : MetaBase
    {
        public static readonly string Path = "/mymemory/books/shared/";

        public MetaSharedBooks(HttpRequestBase request)
            : base(request)
        {
            Title = Resource.Books + " " + Resource.User2;
            try
            {
                var logic = new BooksLogic();
                Description = Resource.Total + ": " +
                              logic.GetCountShared(request.Path.Split('/')[request.Path.Split('/').Length - 1]);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
using System;
using System.Linq;
using MySeenWeb.Add_Code;

namespace MySeenWeb.Migrations
{
    using System.Data.Entity.Migrations;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MySeenWeb.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;//Никаких true, уже стоит рабочая версия
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //23 миграция
            //Обновим старых пользователей, посчитаем им ключи открытия доступа к фильмам, сериалам, книгам, событиям, остальным названичим создание в момент создания пользователя
            var ac =new ApplicationDbContext();
            if (ac.Users.Any(
                    u =>
                        string.IsNullOrEmpty(u.ShareFilmsKey) || string.IsNullOrEmpty(u.ShareSerialsKey) ||
                        string.IsNullOrEmpty(u.ShareBooksKey) || string.IsNullOrEmpty(u.ShareEventsKey)))
            {
                foreach (var user in ac.Users.Where(u =>
                        string.IsNullOrEmpty(u.ShareFilmsKey) || string.IsNullOrEmpty(u.ShareSerialsKey) ||
                        string.IsNullOrEmpty(u.ShareBooksKey) || string.IsNullOrEmpty(u.ShareEventsKey)))
                {
                    if (string.IsNullOrEmpty(user.ShareFilmsKey))
                        user.ShareFilmsKey = Md5Tools.Generate(user.Id, user.UserName, 1);
                    if (string.IsNullOrEmpty(user.ShareSerialsKey))
                        user.ShareSerialsKey = Md5Tools.Generate(user.Id, user.UserName, 2);
                    if (string.IsNullOrEmpty(user.ShareBooksKey))
                        user.ShareBooksKey = Md5Tools.Generate(user.Id, user.UserName, 3);
                    if (string.IsNullOrEmpty(user.ShareEventsKey))
                        user.ShareEventsKey = Md5Tools.Generate(user.Id, user.UserName, 4);
                }
                ac.SaveChanges();
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _46_add_bots_data : DbMigration
    {
        public override void Up()
        {
            var ac = new ApplicationDbContext();
            ac.Bots.Add(new Bots
            {
                Name = "CheckHost",
                UserAgent = "CheckHost",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "DuckDuckGo",
                UserAgent = "DuckDuckGo-Favicons-Bot",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "Survey",
                UserAgent = "SurveyBot",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "Yahoo",
                UserAgent = "Yahoo! Slurp",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });


            ac.Bots.Add(new Bots
            {
                Name = "w3.org",
                UserAgent = "http://validator.w3.org/services",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "www.site-shot.com",
                UserAgent = "http://www.site-shot.com/",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "UnitPay",
                UserAgent = "UnitPay Robot",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "Bing",
                UserAgent = "bingbot",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "Facebook",
                UserAgent = "facebookexternalhit",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "AddThis.com",
                UserAgent = "AddThis.com",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "Relap",
                UserAgent = "Relap fetcher",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "Google",
                UserAgent = "Google PP Default",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "Google",
                UserAgent = "Structured-Data-Testing-Tool",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "Google",
                UserAgent = "Google Page Speed",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "Google",
                UserAgent = "Googlebot",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "Google",
                UserAgent = "Google Favicon",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });
            ac.Bots.Add(new Bots
            {
                Name = "Google",
                UserAgent = "developers.google.com",
                Language = (int)Defaults.LanguagesBase.Indexes.English
            });

            ac.Bots.Add(new Bots
            {
                Name = "top100.rambler.ru",
                UserAgent = "top100.rambler.ru",
                Language = (int)Defaults.LanguagesBase.Indexes.Russian
            });
            ac.Bots.Add(new Bots
            {
                Name = "vk",
                UserAgent = "vkShare",
                Language = (int)Defaults.LanguagesBase.Indexes.Russian
            });

            ac.Bots.Add(new Bots
            {
                Name = "Yandex",
                UserAgent = "YandexMetrika",
                Language = (int)Defaults.LanguagesBase.Indexes.Russian
            });
            ac.Bots.Add(new Bots
            {
                Name = "Yandex",
                UserAgent = "YandexBot",
                Language = (int)Defaults.LanguagesBase.Indexes.Russian
            });
            ac.Bots.Add(new Bots
            {
                Name = "openstat.ru",
                UserAgent = "openstat.ru/Bot",
                Language = (int)Defaults.LanguagesBase.Indexes.Russian
            });


            ac.SaveChanges();
        }
        
        public override void Down()
        {
        }
    }
}

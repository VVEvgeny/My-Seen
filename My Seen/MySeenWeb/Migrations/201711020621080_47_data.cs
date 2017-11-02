using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;
using MySeenWeb.Models.Tables.Portal;

namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _47_data : DbMigration
    {
        public override void Up()
        {
            var ac = new ApplicationDbContext();
            ac.Deals.Add(new Deals
            {
                Year = 2017,
                Month = 1,
                Amount = 3153
            });
            ac.Deals.Add(new Deals
            {
                Year = 2017,
                Month = 2,
                Amount = 4046
            });
            ac.Deals.Add(new Deals
            {
                Year = 2017,
                Month = 3,
                Amount = 4125
            });
            ac.Deals.Add(new Deals
            {
                Year = 2017,
                Month = 4,
                Amount = 3748
            });
            ac.Deals.Add(new Deals
            {
                Year = 2017,
                Month = 5,
                Amount = 4274
            });
            ac.Deals.Add(new Deals
            {
                Year = 2017,
                Month = 6,
                Amount = 4453
            });
            ac.Deals.Add(new Deals
            {
                Year = 2017,
                Month = 7,
                Amount = 4502
            });
            ac.Deals.Add(new Deals
            {
                Year = 2017,
                Month = 8,
                Amount = 4592
            });
            ac.Deals.Add(new Deals
            {
                Year = 2017,
                Month = 9,
                Amount = 4294
            });




            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 1,
                Amount = 2500
            });
            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 2,
                Amount = 3689
            });
            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 3,
                Amount = 4216
            });
            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 4,
                Amount = 4135
            });
            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 5,
                Amount = 4368
            });
            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 6,
                Amount = 4907
            });
            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 7,
                Amount = 4654
            });
            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 8,
                Amount = 4641
            });
            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 9,
                Amount = 4152
            });
            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 10,
                Amount = 3910
            });
            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 11,
                Amount = 4075
            });
            ac.Deals.Add(new Deals
            {
                Year = 2016,
                Month = 12,
                Amount = 4297
            });

            ac.SaveChanges();
        }
        
        public override void Down()
        {
        }
    }
}

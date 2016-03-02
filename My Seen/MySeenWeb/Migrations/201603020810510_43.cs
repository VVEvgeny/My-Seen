using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables.Portal;

namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _43 : DbMigration
    {
        public override void Up()
        {
            var ac =new ApplicationDbContext();

            ac.Salary.Add(new Salary { Year = 2004, Month = 1, Amount = 132 });
            ac.Salary.Add(new Salary { Year = 2004, Month = 2, Amount = 134 });
            ac.Salary.Add(new Salary { Year = 2004, Month = 3, Amount = 143 });
            ac.Salary.Add(new Salary { Year = 2004, Month = 4, Amount = 148 });
            ac.Salary.Add(new Salary { Year = 2004, Month = 5, Amount = 156 });
            ac.Salary.Add(new Salary { Year = 2004, Month = 6, Amount = 165 });
            ac.Salary.Add(new Salary { Year = 2004, Month = 7, Amount = 170 });
            ac.Salary.Add(new Salary { Year = 2004, Month = 8, Amount = 172 });
            ac.Salary.Add(new Salary { Year = 2004, Month = 9, Amount = 170 });
            ac.Salary.Add(new Salary { Year = 2004, Month = 10, Amount = 170 });
            ac.Salary.Add(new Salary { Year = 2004, Month = 11, Amount = 180 });
            ac.Salary.Add(new Salary { Year = 2004, Month = 12, Amount = 200 });

            ac.Salary.Add(new Salary { Year = 2005, Month = 1, Amount = 183 });
            ac.Salary.Add(new Salary { Year = 2005, Month = 2, Amount = 185 });
            ac.Salary.Add(new Salary { Year = 2005, Month = 3, Amount = 198 });
            ac.Salary.Add(new Salary { Year = 2005, Month = 4, Amount = 205 });
            ac.Salary.Add(new Salary { Year = 2005, Month = 5, Amount = 209 });
            ac.Salary.Add(new Salary { Year = 2005, Month = 6, Amount = 222 });
            ac.Salary.Add(new Salary { Year = 2005, Month = 7, Amount = 227 });
            ac.Salary.Add(new Salary { Year = 2005, Month = 8, Amount = 231 });
            ac.Salary.Add(new Salary { Year = 2005, Month = 9, Amount = 226 });
            ac.Salary.Add(new Salary { Year = 2005, Month = 10, Amount = 233 });
            ac.Salary.Add(new Salary { Year = 2005, Month = 11, Amount = 234 });
            ac.Salary.Add(new Salary { Year = 2005, Month = 12, Amount = 261 });

            ac.Salary.Add(new Salary { Year = 2006, Month = 1, Amount = 241 });
            ac.Salary.Add(new Salary { Year = 2006, Month = 2, Amount = 244 });
            ac.Salary.Add(new Salary { Year = 2006, Month = 3, Amount = 260 });
            ac.Salary.Add(new Salary { Year = 2006, Month = 4, Amount = 261 });
            ac.Salary.Add(new Salary { Year = 2006, Month = 5, Amount = 270 });
            ac.Salary.Add(new Salary { Year = 2006, Month = 6, Amount = 282 });
            ac.Salary.Add(new Salary { Year = 2006, Month = 7, Amount = 286 });
            ac.Salary.Add(new Salary { Year = 2006, Month = 8, Amount = 291 });
            ac.Salary.Add(new Salary { Year = 2006, Month = 9, Amount = 283 });
            ac.Salary.Add(new Salary { Year = 2006, Month = 10, Amount = 289 });
            ac.Salary.Add(new Salary { Year = 2006, Month = 11, Amount = 286 });
            ac.Salary.Add(new Salary { Year = 2006, Month = 12, Amount = 312 });

            ac.Salary.Add(new Salary { Year = 2007, Month = 1, Amount = 288 });
            ac.Salary.Add(new Salary { Year = 2007, Month = 2, Amount = 287 });
            ac.Salary.Add(new Salary { Year = 2007, Month = 3, Amount = 308 });
            ac.Salary.Add(new Salary { Year = 2007, Month = 4, Amount = 309 });
            ac.Salary.Add(new Salary { Year = 2007, Month = 5, Amount = 320 });
            ac.Salary.Add(new Salary { Year = 2007, Month = 6, Amount = 331 });
            ac.Salary.Add(new Salary { Year = 2007, Month = 7, Amount = 341 });
            ac.Salary.Add(new Salary { Year = 2007, Month = 8, Amount = 344 });
            ac.Salary.Add(new Salary { Year = 2007, Month = 9, Amount = 332 });
            ac.Salary.Add(new Salary { Year = 2007, Month = 10, Amount = 343 });
            ac.Salary.Add(new Salary { Year = 2007, Month = 11, Amount = 342 });
            ac.Salary.Add(new Salary { Year = 2007, Month = 12, Amount = 369 });

            ac.Salary.Add(new Salary { Year = 2008, Month = 1, Amount = 352 });
            ac.Salary.Add(new Salary { Year = 2008, Month = 2, Amount = 354 });
            ac.Salary.Add(new Salary { Year = 2008, Month = 3, Amount = 376 });
            ac.Salary.Add(new Salary { Year = 2008, Month = 4, Amount = 390 });
            ac.Salary.Add(new Salary { Year = 2008, Month = 5, Amount = 396 });
            ac.Salary.Add(new Salary { Year = 2008, Month = 6, Amount = 419 });
            ac.Salary.Add(new Salary { Year = 2008, Month = 7, Amount = 435 });
            ac.Salary.Add(new Salary { Year = 2008, Month = 8, Amount = 444 });
            ac.Salary.Add(new Salary { Year = 2008, Month = 9, Amount = 446 });
            ac.Salary.Add(new Salary { Year = 2008, Month = 10, Amount = 453 });
            ac.Salary.Add(new Salary { Year = 2008, Month = 11, Amount = 446 });
            ac.Salary.Add(new Salary { Year = 2008, Month = 12, Amount = 456 });

            ac.Salary.Add(new Salary { Year = 2009, Month = 1, Amount = 347 });
            ac.Salary.Add(new Salary { Year = 2009, Month = 2, Amount = 323 });
            ac.Salary.Add(new Salary { Year = 2009, Month = 3, Amount = 337 });
            ac.Salary.Add(new Salary { Year = 2009, Month = 4, Amount = 341 });
            ac.Salary.Add(new Salary { Year = 2009, Month = 5, Amount = 352 });
            ac.Salary.Add(new Salary { Year = 2009, Month = 6, Amount = 360 });
            ac.Salary.Add(new Salary { Year = 2009, Month = 7, Amount = 364 });
            ac.Salary.Add(new Salary { Year = 2009, Month = 8, Amount = 368 });
            ac.Salary.Add(new Salary { Year = 2009, Month = 9, Amount = 367 });
            ac.Salary.Add(new Salary { Year = 2009, Month = 10, Amount = 375 });
            ac.Salary.Add(new Salary { Year = 2009, Month = 11, Amount = 370 });
            ac.Salary.Add(new Salary { Year = 2009, Month = 12, Amount = 385 });

            ac.Salary.Add(new Salary { Year = 2010, Month = 1, Amount = 350 });
            ac.Salary.Add(new Salary { Year = 2010, Month = 2, Amount = 350 });
            ac.Salary.Add(new Salary { Year = 2010, Month = 3, Amount = 370 });
            ac.Salary.Add(new Salary { Year = 2010, Month = 4, Amount = 376 });
            ac.Salary.Add(new Salary { Year = 2010, Month = 5, Amount = 384 });
            ac.Salary.Add(new Salary { Year = 2010, Month = 6, Amount = 410 });
            ac.Salary.Add(new Salary { Year = 2010, Month = 7, Amount = 428 });
            ac.Salary.Add(new Salary { Year = 2010, Month = 8, Amount = 430 });
            ac.Salary.Add(new Salary { Year = 2010, Month = 9, Amount = 432 });
            ac.Salary.Add(new Salary { Year = 2010, Month = 10, Amount = 442 });
            ac.Salary.Add(new Salary { Year = 2010, Month = 11, Amount = 472 });
            ac.Salary.Add(new Salary { Year = 2010, Month = 12, Amount = 530 });

            ac.Salary.Add(new Salary { Year = 2011, Month = 1, Amount = 468 });
            ac.Salary.Add(new Salary { Year = 2011, Month = 2, Amount = 477 });
            ac.Salary.Add(new Salary { Year = 2011, Month = 3, Amount = 508 });
            ac.Salary.Add(new Salary { Year = 2011, Month = 4, Amount = 513 });
            ac.Salary.Add(new Salary { Year = 2011, Month = 5, Amount = 465 });
            ac.Salary.Add(new Salary { Year = 2011, Month = 6, Amount = 358 });
            ac.Salary.Add(new Salary { Year = 2011, Month = 7, Amount = 373 });
            ac.Salary.Add(new Salary { Year = 2011, Month = 8, Amount = 397 });
            ac.Salary.Add(new Salary { Year = 2011, Month = 9, Amount = 421 });
            ac.Salary.Add(new Salary { Year = 2011, Month = 10, Amount = 354 });
            ac.Salary.Add(new Salary { Year = 2011, Month = 11, Amount = 280 });
            ac.Salary.Add(new Salary { Year = 2011, Month = 12, Amount = 340 });

            ac.Salary.Add(new Salary { Year = 2012, Month = 1, Amount = 343 });
            ac.Salary.Add(new Salary { Year = 2012, Month = 2, Amount = 358 });
            ac.Salary.Add(new Salary { Year = 2012, Month = 3, Amount = 390 });
            ac.Salary.Add(new Salary { Year = 2012, Month = 4, Amount = 403 });
            ac.Salary.Add(new Salary { Year = 2012, Month = 5, Amount = 436 });
            ac.Salary.Add(new Salary { Year = 2012, Month = 6, Amount = 451 });
            ac.Salary.Add(new Salary { Year = 2012, Month = 7, Amount = 471 });
            ac.Salary.Add(new Salary { Year = 2012, Month = 8, Amount = 490 });
            ac.Salary.Add(new Salary { Year = 2012, Month = 9, Amount = 486 });
            ac.Salary.Add(new Salary { Year = 2012, Month = 10, Amount = 494 });
            ac.Salary.Add(new Salary { Year = 2012, Month = 11, Amount = 496 });
            ac.Salary.Add(new Salary { Year = 2012, Month = 12, Amount = 553 });

            ac.Salary.Add(new Salary { Year = 2013, Month = 1, Amount = 506 });
            ac.Salary.Add(new Salary { Year = 2013, Month = 2, Amount = 522 });
            ac.Salary.Add(new Salary { Year = 2013, Month = 3, Amount = 544 });
            ac.Salary.Add(new Salary { Year = 2013, Month = 4, Amount = 565 });
            ac.Salary.Add(new Salary { Year = 2013, Month = 5, Amount = 575 });
            ac.Salary.Add(new Salary { Year = 2013, Month = 6, Amount = 592 });
            ac.Salary.Add(new Salary { Year = 2013, Month = 7, Amount = 616 });
            ac.Salary.Add(new Salary { Year = 2013, Month = 8, Amount = 622 });
            ac.Salary.Add(new Salary { Year = 2013, Month = 9, Amount = 595 });
            ac.Salary.Add(new Salary { Year = 2013, Month = 10, Amount = 598 });
            ac.Salary.Add(new Salary { Year = 2013, Month = 11, Amount = 576 });
            ac.Salary.Add(new Salary { Year = 2013, Month = 12, Amount = 621 });

            ac.Salary.Add(new Salary { Year = 2014, Month = 1, Amount = 556 });
            ac.Salary.Add(new Salary { Year = 2014, Month = 2, Amount = 556 });
            ac.Salary.Add(new Salary { Year = 2014, Month = 3, Amount = 586 });
            ac.Salary.Add(new Salary { Year = 2014, Month = 4, Amount = 590 });
            ac.Salary.Add(new Salary { Year = 2014, Month = 5, Amount = 604 });
            ac.Salary.Add(new Salary { Year = 2014, Month = 6, Amount = 611 });
            ac.Salary.Add(new Salary { Year = 2014, Month = 7, Amount = 629 });
            ac.Salary.Add(new Salary { Year = 2014, Month = 8, Amount = 614 });
            ac.Salary.Add(new Salary { Year = 2014, Month = 9, Amount = 603 });
            ac.Salary.Add(new Salary { Year = 2014, Month = 10, Amount = 598 });
            ac.Salary.Add(new Salary { Year = 2014, Month = 11, Amount = 576 });
            ac.Salary.Add(new Salary { Year = 2014, Month = 12, Amount = 621 });

            ac.Salary.Add(new Salary { Year = 2015, Month = 1, Amount = 419 });
            ac.Salary.Add(new Salary { Year = 2015, Month = 2, Amount = 406 });
            ac.Salary.Add(new Salary { Year = 2015, Month = 3, Amount = 435 });
            ac.Salary.Add(new Salary { Year = 2015, Month = 4, Amount = 452 });
            ac.Salary.Add(new Salary { Year = 2015, Month = 5, Amount = 467 });
            ac.Salary.Add(new Salary { Year = 2015, Month = 6, Amount = 451 });
            ac.Salary.Add(new Salary { Year = 2015, Month = 7, Amount = 459 });
            ac.Salary.Add(new Salary { Year = 2015, Month = 8, Amount = 427 });
            ac.Salary.Add(new Salary { Year = 2015, Month = 9, Amount = 388 });
            ac.Salary.Add(new Salary { Year = 2015, Month = 10, Amount = 394 });
            ac.Salary.Add(new Salary { Year = 2015, Month = 11, Amount = 378 });
            ac.Salary.Add(new Salary { Year = 2015, Month = 12, Amount = 406 });

            ac.Salary.Add(new Salary { Year = 2016, Month = 1, Amount = 336 });
            ac.Salary.Add(new Salary { Year = 2016, Month = 2, Amount = 336 });

            ac.SaveChanges();

        }
        
        public override void Down()
        {
        }
    }
}

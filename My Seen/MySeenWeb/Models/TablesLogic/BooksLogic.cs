using System;
using System.Linq;
using MySeenLib;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesLogic
{
    public class BooksLogic : Books
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;
        private readonly ICacheService _cache;
        public BooksLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
        public BooksLogic(ICacheService cache) : this()
        {
            _cache = cache;
        }
        private bool Fill(string name, string year, string authors, string datetime, string genre, string rating, string userId)
        {
            try
            {
                Name = name;
                Authors = authors;
                DateRead = UmtTime.To(Convert.ToDateTime(datetime));
                Year = string.IsNullOrEmpty(year) ? 0 : Convert.ToInt32(year);
                Genre = Convert.ToInt32(genre);                
                Rating = Convert.ToInt32(rating);
                DateChange = UmtTime.To(DateTime.Now);
                UserId = userId;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return true;
        }
        private bool Fill(string id, string name, string year, string authors, string datetime, string genre, string rating, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return Fill(name, year, authors, datetime, genre, rating, userId);
        }
        private bool Contains()
        {
            return _ac.Books.Any(f => f.Name == Name && f.UserId == UserId && f.Id != Id && f.Year == Year);
        }
        private bool Verify()
        {
            if (string.IsNullOrEmpty(Name))
            {
                ErrorMessage = Resource.EnterBookName;
            }
            if (string.IsNullOrEmpty(Authors))
            {
                ErrorMessage = Resource.EnterBookAuthors;
            }
            else if (Contains())
            {
                ErrorMessage = Resource.BookNameAlreadyExists;
            }
            else return true;

            return false;
        }
        private bool Add()
        {
            try
            {
                _ac.Books.Add(this);
                _ac.SaveChanges();
                _cache.Remove(CacheNames.UserBooks.ToString(), UserId);
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }
        private bool Update()
        {
            try
            {
                var film = _ac.Books.First(f => f.UserId == UserId && f.Id == Id);
                film.Name = Name;
                film.Year = Year;
                film.Authors = Authors;
                film.Genre = Genre;
                film.Rating = Rating;
                film.DateChange = DateChange;
                film.DateRead = DateRead;
                _ac.SaveChanges();
                _cache.Remove(CacheNames.UserBooks.ToString(), UserId);
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }
        public bool Add(string name, string year, string authors, string datetime, string genre, string rating, string userId)
        {
            return Fill(name, year, authors, datetime, genre, rating, userId) && Verify() && Add();
        }
        public bool Update(string id, string name, string year, string authors, string datetime, string genre, string rating, string userId)
        {
            return Fill(id, name, year, authors, datetime, genre, rating, userId) && Verify() && Update();
        }
        public bool Delete(string id, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
                if (_ac.Books.Any(f => f.UserId == userId && f.Id == Id))
                {
                    _ac.Books.RemoveRange(_ac.Books.Where(f => f.UserId == userId && f.Id == Id));
                    _ac.SaveChanges();
                    _cache.Remove(CacheNames.UserBooks.ToString(), userId);
                }
                else
                {
                    ErrorMessage = Resource.NoData;
                    return false;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }
        public string GetShare(string id, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
                if (_ac.Books.First(f => f.UserId == userId && f.Id == Id).Shared)
                {
                    var key = _ac.Users.First(t => t.Id == userId).ShareBooksKey;
                    return MySeenWebApi.ApiHost + MySeenWebApi.ShareBooks + key;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
            }
            return "-";
        }
        public string GenerateShare(string id, string userId)
        {
            var iid = Convert.ToInt32(id);
            var key = _ac.Users.First(t => t.Id == userId).ShareBooksKey;
            _ac.Books.First(e => e.Id == iid).Shared = true;
            _ac.SaveChanges();
            _cache.Remove(CacheNames.UserBooks.ToString(), userId);
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareBooks + key;
        }
        public string DeleteShare(string id, string userId)
        {
            var iid = Convert.ToInt32(id);
            _ac.Books.First(e => e.Id == iid && e.UserId == userId).Shared = false;
            _ac.SaveChanges();
            _cache.Remove(CacheNames.UserBooks.ToString(), userId);
            return "-";
        }
    }
}

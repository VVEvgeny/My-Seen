using System;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesLogic
{
    public class BooksLogic : Books
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;
        public BooksLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
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
            return _ac.Books.Any(f => f.Name == Name && f.UserId == UserId && f.Id != Id);
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
        public bool MarkDeleted(string id, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
                _ac.Serials.First(f => f.UserId == userId && f.Id == Id).isDeleted = true;
                _ac.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }
    }
}

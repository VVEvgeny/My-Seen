﻿using MySeenLib;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesViews
{
    public class BooksView : Books
    {
        public static BooksView Map(Books model)
        {
            if (model == null) return new BooksView();

            return new BooksView
            {
                Id = model.Id,
                Name = model.Name,
                UserId = model.UserId,
                DateChange = UMTTime.From(model.DateChange),
                Genre = model.Genre,
                Rating = model.Rating,
                DateRead = UMTTime.From(model.DateRead),
                isDeleted = model.isDeleted,
                Authors = model.Authors
            };
        }
        public string GenreText
        {
            get
            {
                return Defaults.Genres.GetById(Genre);
            }
        }
        public string RatingText
        {
            get
            {
                return Defaults.Ratings.GetById(Rating);
            }
        }
    }
}

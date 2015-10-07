using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models
{
    public class FilmsView: Films
    {
        public static FilmsView Map(Films model)
        {
            if (model == null) return new FilmsView();

            return new FilmsView
            {
                Id = model.Id,
                Name = model.Name,
                UserId = model.UserId,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rate = model.Rate
            };
        }
        public string GenreText
        {
            get
            {
                return LibTools.Genres.GetById(Genre);
            }
        }
        public string RateText
        {
            get
            {
                return LibTools.Ratings.GetById(Rate);
            }
        }
    }
}

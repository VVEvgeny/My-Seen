﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using System.Globalization;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models
{
    public class HomeViewModelRoads
    {
        public IEnumerable<TracksView> DataFoot { get; set; }
        public IEnumerable<TracksView> DataCar { get; set; }
        public IEnumerable<TracksView> DataBike { get; set; }
        public IEnumerable<SelectListItem> YearsList { get; set; }
        public HomeViewModelRoads(string userId, int roadYear, string search, string shareKey)
        {
            var ac = new ApplicationDbContext();

            DataFoot =
                ac.Tracks.AsNoTracking()
                    .Where(
                        t =>
                            t.Type == (int) TrackTypes.Foot
                            && (roadYear == 0 || t.Date.Year == roadYear)
                            && ((string.IsNullOrEmpty(shareKey) && t.UserId == userId)
                                ||
                                (!string.IsNullOrEmpty(shareKey) && (t.User.ShareTracksFootKey == shareKey || t.ShareKey == shareKey)))
                            && (string.IsNullOrEmpty(search) || t.Name.Contains(search))
                    )
                    .OrderByDescending(t => t.Date)
                    .Select(TracksView.Map)
                    .ToList();
            if (DataFoot.Any())
            {
                var dataFootAll = new List<TracksView>
                {
                    new TracksView
                    {
                        Id = -(int) TrackTypes.Foot,
                        Name = Resource.All,
                        UserId = DataFoot.Count().ToString(),
                        Date = new DateTime(1980, 3, 3),
                        Distance = DataFoot.Sum(item => item.Distance),
                        ShareKey = "+"
                    }
                };
                DataFoot = dataFootAll.Concat(DataFoot);
            }

            DataCar =
                ac.Tracks.AsNoTracking()
                    .Where(
                        t =>
                            t.Type == (int) TrackTypes.Car
                            && (roadYear == 0 || t.Date.Year == roadYear)
                            && ((string.IsNullOrEmpty(shareKey) && t.UserId == userId)
                                ||
                                (!string.IsNullOrEmpty(shareKey) &&
                                 (t.User.ShareTracksCarKey == shareKey || t.ShareKey == shareKey)))
                            && (string.IsNullOrEmpty(search) || t.Name.Contains(search))
                    )
                    .OrderByDescending(t => t.Date)
                    .Select(TracksView.Map)
                    .ToList();
            if (DataCar.Any())
            {
                var dataCarAll = new List<TracksView>
                {
                    new TracksView
                    {
                        Id = -(int) TrackTypes.Car,
                        Name = Resource.All,
                        UserId = DataCar.Count().ToString(),
                        Date = new DateTime(1980, 3, 3),
                        Distance = DataCar.Sum(item => item.Distance),
                        ShareKey = "+"
                    }
                };
                DataCar = dataCarAll.Concat(DataCar);
            }

            DataBike =
                ac.Tracks.AsNoTracking()
                    .Where(
                        t =>
                            t.Type == (int) TrackTypes.Bike
                            && (roadYear == 0 || t.Date.Year == roadYear)
                            && ((string.IsNullOrEmpty(shareKey) && t.UserId == userId)
                                ||
                                (!string.IsNullOrEmpty(shareKey) &&
                                 (t.User.ShareTracksBikeKey == shareKey || t.ShareKey == shareKey)))
                            && (string.IsNullOrEmpty(search) || t.Name.Contains(search))
                    )
                    .OrderByDescending(t => t.Date)
                    .Select(TracksView.Map)
                    .ToList();
            if (DataBike.Any())
            {
                var dataBikeAll = new List<TracksView>
                {
                    new TracksView
                    {
                        Id = -(int) TrackTypes.Bike,
                        Name = Resource.All,
                        UserId = DataBike.Count().ToString(),
                        Date = new DateTime(1980, 3, 3),
                        Distance = DataBike.Sum(item => item.Distance),
                        ShareKey = "+"
                    }
                };
                DataBike = dataBikeAll.Concat(DataBike);
            }
            var years = new List<SelectListItem> { 
                new SelectListItem
                {
                    Text = Resource.All,
                    Value = "0",
                    Selected = roadYear == 0
                } };
            foreach (var elem in ac.Tracks.AsNoTracking().Where(t =>
                ((string.IsNullOrEmpty(shareKey) && t.UserId == userId)
                 ||
                 (!string.IsNullOrEmpty(shareKey) &&
                  (((t.User.ShareTracksBikeKey == shareKey && t.Type == (int) TrackTypes.Bike) ||
                    (t.User.ShareTracksCarKey == shareKey && t.Type == (int) TrackTypes.Car) ||
                    (t.User.ShareTracksFootKey == shareKey && t.Type == (int) TrackTypes.Foot))
                   || t.ShareKey == shareKey)))
                && (string.IsNullOrEmpty(search) || t.Name.Contains(search))
                ).Select(t => t.Date.Year).Distinct())
            {
                years.Add(new SelectListItem
                {
                    Text = elem.ToString(),
                    Value = elem.ToString(),
                    Selected = roadYear == elem
                });
            }
            YearsList = years.OrderByDescending(y => y.Text);
        }
    }
}

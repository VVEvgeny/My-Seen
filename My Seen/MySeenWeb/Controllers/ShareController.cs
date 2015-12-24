﻿using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenWeb.ActionFilters;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Models;
using MySeenWeb.Models.ShareViewModels;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers
{
    //[RequireHttps]
    public class ShareController : BaseController
    {
        [BrowserActionFilter]
        public ActionResult Films(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult Films(string id)";
            try
            {
                return View(new ShareViewModelFilmsMin(id));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index","Home");
        }

        [BrowserActionFilter]
        public ActionResult Serials(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult Serials(string id)";
            try
            {
                return View(new ShareViewModelSerialsMin(id));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Home");
        }

        [BrowserActionFilter]
        public ActionResult Books(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult Books(string id)";
            try
            {
                return View(new ShareViewModelBooksMin(id));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Home");
        }

        [BrowserActionFilter]
        public ActionResult Events(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult Events(string id)";
            try
            {
                return View(new ShareViewModelEventsMin(id));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Home");
        }

        [BrowserActionFilter]
        public ActionResult Tracks(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult Tracks(string id)";
            try
            {
                return View(new ShareViewModelTracks(id, MarkersOnRoads, ReadUserSideStorage(UserSideStorageKeys.RoadsYear, 0)));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
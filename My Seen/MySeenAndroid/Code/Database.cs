using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using System.Collections.Generic;
using SQLite;
using MySeenLib;
using System.IO;

namespace MySeenAndroid
{
    public class DatabaseHelper
    {
        private static string LogTAG = "MySeenAndroid_DATABASE";
        private SQLiteConnection connection;
        private string DBName;
        private string databaseFilePath;

        public DatabaseHelper()
        {
            DBName = "myseen.db3";
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            databaseFilePath = System.IO.Path.Combine(folder, DBName);

            Log.Warn(LogTAG, "databaseFilePath=" + databaseFilePath);


            bool needCreateDB = !File.Exists(databaseFilePath);
            Log.Warn(LogTAG, "needCreateDB=" + needCreateDB.ToString());

            connection = new SQLiteConnection(databaseFilePath);
            if (needCreateDB) CreateTables();
            Log.Warn(LogTAG, "END DatabaseHelper()");
        }

        public void CreateTables()
        {
            Log.Warn(LogTAG, "CreateTables Create tables begin");
            connection.CreateTable<Films>();
            connection.CreateTable<Serials>();
        }
        public int GetSerialsCount()
        {
            return connection.Table<Serials>().Count();
        }
        public int GetFilmsCount()
        {
            return connection.Table<Films>().Count();
        }
        public void Add(Films film)
        {
            //Log.Warn(LogTAG, "Add Films begin name=" + film.Name);
            connection.Insert(film);
            //Log.Warn(LogTAG, "Add Films end id=" + film.Id.ToString());
        }
        public void Add(Serials film)
        {
            //Log.Warn(LogTAG, "Add Serials begin name=" + film.Name);
            connection.Insert(film);
            //Log.Warn(LogTAG, "Add Serials end id=" + film.Id.ToString());
        }
        public IEnumerable<Films> GetFilms()
        {
            //Log.Warn(LogTAG, "GetFilms()");
            return connection.Table<Films>().OrderByDescending(f => f.DateSee);
        }
        public IEnumerable<Serials> GetSerials()
        {
            //Log.Warn(LogTAG, "GetSerials()");
            return connection.Table<Serials>().OrderByDescending(f=>f.DateLast);
        }
    }
}
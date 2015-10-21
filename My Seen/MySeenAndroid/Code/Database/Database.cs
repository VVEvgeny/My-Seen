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
    public static class DatabaseHelper
    {
        private static DatabaseHelperClass _DatabaseHelper;
        public static DatabaseHelperClass Get
        {
            get
            {
                if (_DatabaseHelper == null) _DatabaseHelper = new DatabaseHelperClass();
                return _DatabaseHelper;
            }
        }
        public static SQLiteConnection Connect
        {
            get
            {
                if (_DatabaseHelper == null) _DatabaseHelper = new DatabaseHelperClass();
                return _DatabaseHelper.connection;
            }
        }
    }
    public class DatabaseHelperClass
    {
        private const int FILMS_VERSION = 1;
        private const int SERIALS_VERSION = 1;
        private const int ADD_DATA_VERSION = 1;

        private static string LogTAG = "MySeenAndroid_DATABASE";
        public SQLiteConnection connection;
        private string DBName;
        private string databaseFilePath;

        public DatabaseHelperClass()
        {
            DBName = "myseen.db3";
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            databaseFilePath = System.IO.Path.Combine(folder, DBName);

            Log.Warn(LogTAG, "databaseFilePath=" + databaseFilePath);


            bool needCreateDB = !File.Exists(databaseFilePath);
            Log.Warn(LogTAG, "needCreateDB=" + needCreateDB.ToString());

            connection = new SQLiteConnection(databaseFilePath);

            if (needCreateDB) CreateTables();
            else
            {
                TablesVersion tv=connection.Table<TablesVersion>().Where(t => t.TableName == "Films").First();
                Log.Warn(LogTAG, "table Films version=" + tv.Version.ToString() + " current version=" + FILMS_VERSION.ToString());
                if (tv.Version < FILMS_VERSION)
                {
                    Log.Warn(LogTAG, "table films is OLD RECREAT");
                    connection.DropTable<Films>();
                    connection.CreateTable<Films>();
                    tv.Version = FILMS_VERSION;
                    connection.Update(tv);
                }
                tv = connection.Table<TablesVersion>().Where(t => t.TableName == "Serials").First();
                Log.Warn(LogTAG, "table Serials version=" + tv.Version.ToString() + " current version=" + SERIALS_VERSION.ToString());
                if (tv.Version < SERIALS_VERSION)
                {
                    Log.Warn(LogTAG, "table Serials is OLD RECREAT");
                    connection.DropTable<Serials>();
                    connection.CreateTable<Serials>();
                    tv.Version = SERIALS_VERSION;
                    connection.Update(tv);
                }
                tv = connection.Table<TablesVersion>().Where(t => t.TableName == "Add_Data").First();
                Log.Warn(LogTAG, "table Add_Data version=" + tv.Version.ToString() + " current version=" + ADD_DATA_VERSION.ToString());
                if (tv.Version < ADD_DATA_VERSION)
                {
                    Log.Warn(LogTAG, "table Add_Data is OLD RECREAT");
                    connection.DropTable<AddData>();
                    connection.CreateTable<AddData>();
                    tv.Version = ADD_DATA_VERSION;
                    connection.Update(tv);
                }
            }
            Log.Warn(LogTAG, "END DatabaseHelper()");
        }
        public void CreateTables()
        {
            Log.Warn(LogTAG, "CreateTables Create tables begin");
            connection.CreateTable<Films>();
            connection.CreateTable<Serials>();
            connection.CreateTable<AddData>();

            connection.CreateTable<TablesVersion>();
            connection.Insert(new TablesVersion { TableName = "Films", Version = FILMS_VERSION });
            connection.Insert(new TablesVersion { TableName = "Serials", Version = SERIALS_VERSION });
            connection.Insert(new TablesVersion { TableName = "Add_Data", Version = ADD_DATA_VERSION });
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
            connection.Insert(film);
        }
        public void Update(Films film)
        {
            connection.Update(film);
        }
        public void Update(Serials film)
        {
            connection.Update(film);
        }
        public bool isFilmExist(string name)
        {
            return connection.Table<Films>().Where(f => f.Name == name).Count() != 0;
        }
        public bool isFilmExist(int? id)
        {
            if (id.GetValueOrDefault(-1) == -1) return false;
            return connection.Table<Films>().Where(f => f.Id == id.Value).Count() != 0;
        }
        public bool isFilmExistAndNotSame(string name,int id)
        {
            return connection.Table<Films>().Where(f => f.Name == name && f.Id != id).Count() != 0;
        }
        public bool isSerialExist(string name)
        {
            return connection.Table<Serials>().Where(f => f.Name == name).Count() != 0;
        }
        public bool isSerialExist(int? id)
        {
            if (id.GetValueOrDefault(-1) == -1) return false;
            return connection.Table<Serials>().Where(f => f.Id == id.Value).Count() != 0;
        }
        public bool isSerialExistAndNotSame(string name, int id)
        {
            return connection.Table<Serials>().Where(f => f.Name == name && f.Id != id).Count() != 0;
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
            return connection.Table<Films>().Where(f => f.isDeleted != true || f.isDeleted == null).OrderByDescending(f => f.DateSee);
        }
        public IEnumerable<Films> GetFilmsWithDeleted()
        {
            //Log.Warn(LogTAG, "GetFilms()");
            return connection.Table<Films>().OrderByDescending(f => f.DateSee);
        }
        public Films GetFilmById(int id)
        {
            return connection.Table<Films>().Where(f => f.Id == id).First();
        }
        public Serials GetSerialById(int id)
        {
            return connection.Table<Serials>().Where(f => f.Id == id).First();
        }
        public IEnumerable<Serials> GetSerials()
        {
            //Log.Warn(LogTAG, "GetSerials()");
            return connection.Table<Serials>().Where(f => f.isDeleted != true || f.isDeleted == null).OrderByDescending(f => f.DateLast);
        }
        public IEnumerable<Serials> GetSerialsWithDeleted()
        {
            //Log.Warn(LogTAG, "GetSerials()");
            return connection.Table<Serials>().OrderByDescending(f => f.DateLast);
        }
        public int GetAddDataCount()
        {
            return connection.Table<AddData>().Count();
        }
        public AddData GetAddData()
        {
            return connection.Table<AddData>().First();
        }
        public void ClearAddData()
        {
            connection.DeleteAll<AddData>();
        }
        public void ClearFilms()
        {
            connection.DeleteAll<Films>();
        }
        public void ClearSerials()
        {
            connection.DeleteAll<Serials>();
        }
        public void Add(AddData data)
        {
            connection.Insert(data);
        }
        public void Delete(object film)
        {
            connection.Delete(film);
        }
    }
}
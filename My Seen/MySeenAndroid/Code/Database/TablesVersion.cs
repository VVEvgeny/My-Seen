using System;
using SQLite;

namespace MySeenAndroid
{
    public class TablesVersion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string TableName { get; set; }
        public int Version { get; set; }
    }
}
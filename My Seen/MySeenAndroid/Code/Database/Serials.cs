using System;
using SQLite;

namespace MySeenAndroid
{
    public class Serials
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int? Id_R { get; set; }
        public string Name { get; set; }
        public int LastSeason { get; set; }
        public int LastSeries { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateLast { get; set; }
        public int Genre { get; set; }
        public int Rating { get; set; }
        public DateTime DateChange { get; set; }
        public bool? isDeleted { get; set; }
    }
}
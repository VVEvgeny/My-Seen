using System;
using SQLite;

namespace MySeenAndroid
{
    public class Films
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateSee { get; set; }
        public int Genre { get; set; }
        public int Rate { get; set; }
        public DateTime DateChange { get; set; }
    }
}
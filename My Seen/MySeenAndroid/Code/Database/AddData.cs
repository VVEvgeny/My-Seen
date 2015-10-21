using System;
using SQLite;

namespace MySeenAndroid
{
    public class AddData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
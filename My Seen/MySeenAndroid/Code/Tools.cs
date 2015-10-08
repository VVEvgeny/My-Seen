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
    public enum States
    {
        Films,
        Serials
    }
}
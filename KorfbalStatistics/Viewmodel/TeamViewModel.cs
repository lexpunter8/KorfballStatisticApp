using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using KorfbalStatistics.Model;

namespace KorfbalStatistics.Viewmodel
{
    public class TeamViewModel
    {
        public List<DbUser> Players { get; } = new List<DbUser>();
    }
}
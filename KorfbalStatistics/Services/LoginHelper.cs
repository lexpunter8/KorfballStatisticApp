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

namespace KorfbalStatistics.Services
{
    public static class LoginHelper
    {
        public static bool IsFirstLogin { get; set; } = false;
    }
}
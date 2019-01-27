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

namespace KorfbalStatistics.CustomExtensions
{
    public class Attributes
    {
        public class RemoteDbNameAttribute : Attribute
        {
            public RemoteDbNameAttribute(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }
    }
}
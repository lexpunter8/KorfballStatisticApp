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
using static KorfbalStatistics.Model.Enums;

namespace KorfbalStatistics.Model
{
    public class Player
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public EPlayerFunction ZoneFunction { get; set; }
        public string Abbrevation { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }

        public string FullName() => FirstName + " " + Name;
    }
}
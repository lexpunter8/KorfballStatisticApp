using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

namespace KorfbalStatistics.Services
{
    public class ContextManager : Application
    {
        private static ContextManager myInstance;
        public static ContextManager Instance
        {
            get
            {
                if (myInstance == null)
                    myInstance = new ContextManager();
                return myInstance;
            }
        }
        public Context CurrentContext { get; set; }
        
        public void ShowSnackBar()
        {
           // Snackbar snackBar = Snackbar.Make()
        }
    }
}
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
    public class ServiceLocator
    {
        static Dictionary<string, object> servicesDictionary = new Dictionary<string, object>();

        public static void Register<T>(T service)
        {
            servicesDictionary[typeof(T).Name] = service;
        }

        public static T GetService<T>()
        {
            T instance = default(T);
            if (servicesDictionary.ContainsKey(typeof(T).Name) == true)
            {
                instance = (T)servicesDictionary[typeof(T).Name];
            }
            return instance;
        }
    }
}
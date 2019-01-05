using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using KorfbalStatistics.Viewmodel;

namespace KorfbalStatistics
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private MainViewModel myViewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            myViewModel = MainViewModel.Instance;
            //Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);

            Button myLoginButton = FindViewById<Button>(Resource.Id.loginButton);
            myLoginButton.Click += LoginButtonClick;

            StartActivity(typeof(HomeActivity));
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void LoginButtonClick(object sender, EventArgs eventArgs)
        {
            StartActivity(typeof(HomeActivity));
            Finish();
           //View view = (View) sender;
           //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
           //  .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using KorfbalStatistics.Interface;
using KorfbalStatistics.Model;

namespace KorfbalStatistics.Adapters
{
    internal class GameListAdapter : ArrayAdapter<IDbGame>
    {
        public GameListAdapter(List<DbGame> data, Activity context) : base(context, Resource.Layout.list_view_game)
        {
            mydata = data;
            myContext = context;
        }

        private List<DbGame> mydata {get;set;}
        private Activity myContext {get;set;}

        public override long GetItemId(int position)
        {
            return base.GetItemId(position);
        }

        new public DbGame GetItem(int position)
        {
            return mydata[position];
        }

        public override int Count
        {
            get { return mydata.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = mydata[position];
            View view = convertView;
            if (view == null)
                view = myContext.LayoutInflater.Inflate(Resource.Layout.list_view_game, null);
            view.FindViewById<TextView>(Resource.Id.homeTeamText).Text = item.IsHome ? "LDODK" : item.Opponent;
            view.FindViewById<TextView>(Resource.Id.awayTeamText).Text = !item.IsHome ? "LDODK" : item.Opponent;
            view.FindViewById<TextView>(Resource.Id.statusText).Text = "W";
            view.FindViewById<TextView>(Resource.Id.dateText).Text = item.Date.ToString("dd-MM-yyyy HH:mm");
            return view;
        }
    }
}
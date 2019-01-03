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

namespace KorfbalStatistics.Adapters
{
    public class ZonePlayersListAdapter : ArrayAdapter<DbPlayer>
    {
        public ZonePlayersListAdapter(Activity context) : base(context, Resource.Layout.detailed_player_view)
        {
            mydata = new List<DbPlayer>
            {
                null,
                null,
                null,
                null
            };
            myContext = context;
        }

        private List<DbPlayer> mydata;
        private Activity myContext;

        public override long GetItemId(int position)
        {
            return base.GetItemId(position);
        }

        new public DbPlayer GetItem(int position)
        {
            return mydata[position];
        }

        public override int Count
        {
            get { return mydata.Count; }
        }

        new public void Add(DbPlayer player)
        {
            bool foundNull = false;
            int index = 0;
            mydata.ForEach(p =>
            {
                if (foundNull)
                    return;
                if (p == null)
                {
                    foundNull = true;
                    return;
                }
                index++;
            });
            if (foundNull)
            {
                mydata[index] = player;
                NotifyDataSetChanged();
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view;
            var item = mydata[position];
            if (item == null)
            {
                view = myContext.LayoutInflater.Inflate(Resource.Layout.empty_zoneplayer_listview, null);
                return view;
            }
            
            view = myContext.LayoutInflater.Inflate(Resource.Layout.detailed_player_view, null);   
            view.FindViewById<TextView>(Resource.Id.playerAbbrevation).Text = item.Abbrevation;
            view.FindViewById<TextView>(Resource.Id.playerName).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.playerNumber).Text = item.Number.ToString();
                        
            return view;
        }
    }
}
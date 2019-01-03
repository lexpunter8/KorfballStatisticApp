using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace KorfbalStatistics.CustomviewClasses
{
    public class RoundedTextViewLayout : LinearLayout
    {
        public RoundedTextViewLayout(Context context) : base(context)
        {
            Init();
        }

        public RoundedTextViewLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public RoundedTextViewLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public RoundedTextViewLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        protected RoundedTextViewLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public void Init()
        {
            Orientation = Orientation.Horizontal;               
        }

        public void AddViews(List<Model.Player> myCurrentPlayers)
        {
            Space space = new Space(Context)
            {
                LayoutParameters = new LayoutParams(0, 1, 1f)
            };

            RemoveAllViews();
            AddView(space);
            for (int i = 0; i < myCurrentPlayers.Count; i++)
            {
                RoundedTextView newButton = new RoundedTextView(Context)
                {
                    LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent),
                    Text = myCurrentPlayers[i].Abbrevation                   
                };
                AddView(newButton);
                AddView(new Space(Context)
                {
                    LayoutParameters = new LayoutParams(0, 1, 1f)
                });
            }

        }
    }
}
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
    public class SquaredTextViewLayout : LinearLayout
    {
        public SquaredTextViewLayout(Context context) : base(context)
        {
            Init();
        }

        public SquaredTextViewLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public SquaredTextViewLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public SquaredTextViewLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        protected SquaredTextViewLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public void Init()
        {
            Orientation = Orientation.Horizontal;
        }

        public void AddViews(List<Model.Player> myCurrentPlayers)
        {
            RemoveAllViews();
            for (int i = 0; i < myCurrentPlayers.Count; i++)
            {
                var layoutParams = new LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1);
                layoutParams.SetMargins(15, 15, 15, 15);
                SquaredTextView newButton = new SquaredTextView(Context)
                {
                    LayoutParameters = layoutParams,
                    Text = myCurrentPlayers[i].FirstName
                };
                AddView(newButton);
            }

        }
    }
}
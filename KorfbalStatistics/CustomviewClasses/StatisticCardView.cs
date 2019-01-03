using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace KorfbalStatistics.CustomviewClasses
{
    public class StatisticCardView : View
    {
        public TextView myHeader;
        private string myHeadertext;

        public StatisticCardView(Context context) : base(context)
        {
            Init(context);
        }
    
        public StatisticCardView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public StatisticCardView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        public StatisticCardView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(context, attrs);
        }

        protected StatisticCardView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private void Init(Context context, IAttributeSet attrs = null)
        {
            if (attrs == null)
                return;

            var array = context.ObtainStyledAttributes(attrs, Resource.Styleable.StatisticCardView, 0, 0);
            HeaderText = array.GetString(Resource.Styleable.StatisticCardView_headerText);

            array.Recycle();
        }

        Paint color = new Paint(PaintFlags.AntiAlias)
        {
            Color = Color.Green,
            TextSize = 20
        };
        protected override void OnDraw(Canvas canvas)
        {
            canvas.DrawText(HeaderText, 0, 0, color);
        }

        public string HeaderText
        {
            get => myHeadertext;
            set
            {
                myHeadertext = value;
                Invalidate();
            }
        }
    }
}
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
    public class RoundedTextView : TextView
    {
        public RoundedTextView(Context context) : base(context)
        {
            Init();
        }

        public RoundedTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public RoundedTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public RoundedTextView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        protected RoundedTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        private void Init()
        {
            TextAlignment = TextAlignment.Center;
            Gravity = GravityFlags.Center;
            Background = Context.GetDrawable(Resource.Drawable.deselectedradioButtonCircle);
            SetTextColor(Android.Graphics.Color.Black);
        }
    }
}
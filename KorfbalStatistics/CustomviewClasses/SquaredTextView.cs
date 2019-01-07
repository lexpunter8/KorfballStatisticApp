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
    public class SquaredTextView : TextView
    {
        public SquaredTextView(Context context) : base(context)
        {
            Init();
        }

        public SquaredTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public SquaredTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public SquaredTextView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        protected SquaredTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        private void Init()
        {
            TextAlignment = TextAlignment.Center;
            Gravity = GravityFlags.Center;
            Background = Context.GetDrawable(Resource.Drawable.square_deselected_radiobutton);
            SetTextColor(Android.Graphics.Color.Black);
        }
    }
}
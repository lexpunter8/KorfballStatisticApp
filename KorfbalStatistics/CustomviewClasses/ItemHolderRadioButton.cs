using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace KorfbalStatistics.CustomviewClasses
{
    public class ItemHolderRadioButton : RadioButton
    {
        public ItemHolderRadioButton(Context context, bool isSquared) : base(context)
        {
            IsSquared = isSquared;
            Init();
            CheckedChange += ItemHolderRadioButton_CheckedChange;
        }

        private void ItemHolderRadioButton_CheckedChange(object sender, CheckedChangeEventArgs e)
        {
            Checked = e.IsChecked;
            if (e.IsChecked)
            {
                if (IsSquared)
                    Background = Context.GetDrawable(Resource.Drawable.square_selected_radiobutton);
                else
                    Background = Context.GetDrawable(Resource.Drawable.selectedRadioCircle);
            }
            else
                if (IsSquared)
                    Background = Context.GetDrawable(Resource.Drawable.square_deselected_radiobutton);
                else
                    Background = Context.GetDrawable(Resource.Drawable.deselectedradioButtonCircle);
        }

        public ItemHolderRadioButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.ItemHolderRadioButton);
            string s = a.GetString(Resource.Styleable.ItemHolderRadioButton_itemId);
            IsSquared = a.GetBoolean(Resource.Styleable.ItemHolderRadioButton_isSquared, false);
            if (s != null)
            {
                Guid guidOut;
                bool parsed = Guid.TryParse(s, out guidOut);
                if (parsed)
                {
                    ItemId = guidOut;
                }
            }
            a.Recycle();
            Init();
            CheckedChange += ItemHolderRadioButton_CheckedChange;
        }

        public ItemHolderRadioButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
            CheckedChange += ItemHolderRadioButton_CheckedChange;
        }

        public ItemHolderRadioButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
            CheckedChange += ItemHolderRadioButton_CheckedChange;
        }

        protected ItemHolderRadioButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
            CheckedChange += ItemHolderRadioButton_CheckedChange;
        }
        
        private void Init()
        {
            TextAlignment = TextAlignment.Center;
            if (IsSquared)
                Background = Context.GetDrawable(Resource.Drawable.square_deselected_radiobutton);
            else
                Background = Context.GetDrawable(Resource.Drawable.deselectedradioButtonCircle);
            Checked = false;
            SetButtonDrawable(Android.Resource.Color.Transparent);
            
         }

        public bool IsSquared { get; set; }

        public Guid ItemId { get; set; }
    }
}
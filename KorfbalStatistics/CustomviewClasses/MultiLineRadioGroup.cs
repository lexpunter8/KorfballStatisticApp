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
using KorfbalStatistics.Viewmodel;

namespace KorfbalStatistics.CustomviewClasses
{
    public class MultiLineRadioGroup : ItemIdHolderRadioGroup
    {
        public MultiLineRadioGroup(Context context) : base(context)
        {
        }

        public MultiLineRadioGroup(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            ChildViewAdded += MultiLineRadioGroup_ChildViewAdded;
            
        }

        private void MultiLineRadioGroup_ChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            if (e.Child.GetType().Equals(typeof(LinearLayout)))
            {
                LinearLayout ll = e.Child as LinearLayout;
                SetRadioButtons(ll);
            }
            if (e.Child.GetType().Equals(typeof(ItemHolderRadioButton)))
            {
                ItemHolderRadioButton rb = e.Child as ItemHolderRadioButton;
                rb.CheckedChange += Rb_CheckedChange;
                myButtons.Add(rb);
            }
                
        }

        protected MultiLineRadioGroup(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            
        }
        
        private void SetRadioButtons(LinearLayout ll)
        {
            int count = ll.ChildCount;
            for (int i = 0; i < count; i++)
            {
                View o = ll. GetChildAt(i);
                if (o.GetType() == typeof(ItemHolderRadioButton))
                {
                    ItemHolderRadioButton rb = o as ItemHolderRadioButton;
                    rb.CheckedChange += Rb_CheckedChange;
                    myButtons.Add(rb);
                }
            }
        }

        private void Rb_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (!e.IsChecked)
                return;
            ItemHolderRadioButton checkedChangedButton = sender as ItemHolderRadioButton;
            myButtons.ForEach(rb =>
            {
                if (rb.Id != checkedChangedButton.Id)
                    rb.Checked = false;
            });
            OnCheckedChanged(new CheckedChangeEventArgs(checkedChangedButton.Id));
        }

        public event EventHandler CheckedChanged;
        
        private void OnCheckedChanged(CheckedChangeEventArgs e)
        {
            CheckedChanged?.Invoke(this, e);
        }
    }
}
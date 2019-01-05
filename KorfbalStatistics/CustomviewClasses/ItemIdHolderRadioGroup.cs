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
using KorfbalStatistics.Model;
using KorfbalStatistics.Viewmodel;

namespace KorfbalStatistics.CustomviewClasses
{
    public class ItemIdHolderRadioGroup : RadioGroup
    {
        
        public ItemIdHolderRadioGroup(Context context) : base(context)
        {
        }

        public ItemIdHolderRadioGroup(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        protected ItemIdHolderRadioGroup(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected List<ItemHolderRadioButton> myButtons = new List<ItemHolderRadioButton>();

        public ItemHolderRadioButton GetSelected()
        {
            return myButtons.FirstOrDefault(r => r.Checked);
        }
        public void DeselectAll()
        {
            myButtons.ForEach(b => b.Checked = false);
        }
        public void SetToDefault()
        {
            myButtons.ForEach(b => b.Init());
        }
        public virtual void SetPlayers(List<Player> myCurrentPlayers)
        {
            myButtons.Clear();
            Space space = new Space(Context)
            {
                LayoutParameters = new LayoutParams(0, 1, 1f)
            };

            RemoveAllViews();
            AddView(space);
            for (int i = 0; i < myCurrentPlayers.Count; i++)
            {
                ItemHolderRadioButton newButton = new ItemHolderRadioButton(Context, false)
                {
                    LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent),
                    ItemId = myCurrentPlayers[i].Id,
                    Text = myCurrentPlayers[i].Abbrevation
                };
                AddView(newButton);
                myButtons.Add(newButton);
                AddView(new Space(Context)
                {
                    LayoutParameters = new LayoutParams(0, 1, 1f)
                });
            }

        }
    }
}
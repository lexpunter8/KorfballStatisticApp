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
    public class MultiLineRadioGroup : ItemIdHolderRadioGroup
    {
        private Space space;

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

        public override void SetPlayers(List<Model.Player> myCurrentPlayers)
        {
            int count = myCurrentPlayers.Count;
            int layoutCount = ChildCount;

            int childPerLayout = 2;//count / layoutCount;

            int playerIndex = 0;
            for (int child = 0; child < ChildCount; child++)
            {
                LinearLayout view = GetChildAt(child) as LinearLayout;
                view.RemoveAllViews();
                for (int i = 0; i < childPerLayout; i++)
                {
                    if (playerIndex >= count)
                        break;
                    var layoutParams = new LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1);
                    layoutParams.SetMargins(15, 15, 15, 15);
                    Player currentPlayer = myCurrentPlayers[playerIndex];
                    ItemHolderRadioButton newButton = new ItemHolderRadioButton(Context, true)
                    {
                        LayoutParameters = layoutParams,
                        
                        ItemId = currentPlayer.Id,
                        Text = (currentPlayer.Number > -1 ? currentPlayer.Number + " " : "") + currentPlayer.FirstName,
                        Id = GenerateViewId(),
                        
                    };
                    newButton.CheckedChange += Rb_CheckedChange;
                    myButtons.Add(newButton);
                    view.AddView(newButton);
                    playerIndex++;
                }
            }

        }
    }
}
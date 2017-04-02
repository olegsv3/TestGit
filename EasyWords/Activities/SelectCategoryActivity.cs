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

namespace EasyWords
{
    [Activity(Label = "Select category", Theme = "@style/MyTheme")]
    public class SelectCategoryActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectCategory);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            //ActionBar.Set
        }
    }
}
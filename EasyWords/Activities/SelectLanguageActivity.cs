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
    [Activity(Label = "SelectCategoryActivity", Theme = "@style/MyTheme")]
    public class SelectLanguageActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectLanguage);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_keyboard_backspace_white_36dp);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.OnBackPressed();
            return base.OnOptionsItemSelected(item);
        }
    }
}
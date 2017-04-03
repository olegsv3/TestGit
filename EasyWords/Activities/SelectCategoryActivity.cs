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
        private ListView _listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectCategory);

            SetActionBar();

            UpdateList();

            _listView.ItemClick += OnItemClick;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.OnBackPressed();
            return base.OnOptionsItemSelected(item);
        }

        private void SetActionBar()
        {
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_keyboard_backspace_white_36dp);
        }

        public void UpdateList()
        {
            var list = FileWorker.GetCategories();
            list.Add(new Card { Word1 = "Add category...", Word2 = "" });

            _listView = FindViewById<ListView>(Resource.Id.listView1);
            var adapter = new SelectAdapter(this, list);

            _listView.Adapter = adapter;
        }

        public void AddCategory()
        {
            var builder = new AlertDialog.Builder(this);
            var alertDialog = builder.Create();
            var editText = new EditText(this);

            editText.SetTextColor(Android.Graphics.Color.Black);

            builder
                .SetTitle("Enter category")
                .SetView(editText)
                .SetPositiveButton("OK", (s, e) =>
                {
                    if (FileWorker.TestStringCat(editText.Text))
                    {
                        FileWorker.AddCategory(editText.Text);
                        UpdateList();
                    }
                    else Toast.MakeText(this, "Invalid symbol", ToastLength.Long).Show();
                })
                .SetNegativeButton("Cancel", (s, e) =>
                {
                    alertDialog.Cancel();
                })
                .Show();
        }

        public void OnItemClick(object sender, ListView.ItemClickEventArgs e)
        {
            var item = FindViewById<ListView>(Resource.Id.listView1).Adapter.GetView(e.Position, null, null);

            if (item.FindViewById<TextView>(Resource.Id.textView1).Text == "Add category...")
                AddCategory();
        }
    }
}
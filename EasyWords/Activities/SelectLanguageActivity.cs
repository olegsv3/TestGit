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
    [Activity(Label = "Select language", Theme = "@style/MyTheme")]
    public class SelectLanguageActivity : Activity
    {
        private ListView _listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectLanguage);

            SetActionBar();

            UpdateList();
            _listView.ItemClick += OnItemClick;
        }

        private void SetActionBar()
        {
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_keyboard_backspace_white_36dp);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.OnBackPressed();
            return base.OnOptionsItemSelected(item);
        }

        public void UpdateList()
        {
            var list = FileWorker.GetLanguages();
            list.Add(new Card { Word1 = "Add language...", Word2 = ""});

            _listView = FindViewById<ListView>(Resource.Id.listView1);
            var adapter = new SelectAdapter(this, list);
            
         
            _listView.Adapter = adapter;
        }

        public void AddLanguage()
        {
            var builder = new AlertDialog.Builder(this);
            var alertDialog = builder.Create();
            var editText = new EditText(this);
            editText.SetTextColor(Android.Graphics.Color.Black);
            builder
                .SetTitle("Enter language")
                .SetView(editText)
                .SetPositiveButton("OK", (s, e) =>
                {
                    FileWorker.AddLanguage(editText.Text);
                    UpdateList();
                })
                .SetNegativeButton("Cancel", (s, e) =>
                {
                    alertDialog.Cancel();
                })
                .Show();
        }

        public void OnItemClick(object sender, ListView.ItemClickEventArgs e)
        {
            if (e.Position == _listView.Count - 1)
                AddLanguage();
        }
    }
}
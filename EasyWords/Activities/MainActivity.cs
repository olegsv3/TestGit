using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Linq;

namespace EasyWords
{
    [Activity(Label = "EasyWords", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : Activity
    {
        private List<string> _list;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            SetActionBar();

            GenerateList();

        }

        private void SetActionBar()
        {
            ActionBar.SetDisplayShowHomeEnabled(true);
            ActionBar.SetIcon(Resource.Drawable.ic_crop_5_4_white_36dp);
            ActionBar.Title = "   Easy words";
        }

        private void GenerateList()
        {           
            _list = ((GetString(Resource.String.MenuString)).Split('_')).ToList<string>();
            var adapter = new ItemAdapter(this, _list);
            var listView = FindViewById<ListView>(Resource.Id.listView1);
            listView.Adapter = adapter;
            listView.ItemClick += OnItemClick;
        }

        private void OnItemClick(object sender, ListView.ItemClickEventArgs e)
        {
            var item = FindViewById<ListView>(Resource.Id.listView1).Adapter.GetView(e.Position, null, null);
            //Select Language
            if (item.FindViewById<TextView>(Resource.Id.textView1).Text == (GetString(Resource.String.MenuString)).Split('_')[0])
            {
                StartActivity(typeof(SelectLanguageActivity));
            }
            //Options
            if (item.FindViewById<TextView>(Resource.Id.textView1).Text == (GetString(Resource.String.MenuString)).Split('_')[1])
            {
                
            }
            //Exit
            if (item.FindViewById<TextView>(Resource.Id.textView1).Text == (GetString(Resource.String.MenuString)).Split('_')[2])
            {               
                this.FinishAffinity();
            }
        }
    }
}


using Android.App;
using Android.Widget;
using Android.OS;

namespace EasyWords
{
    [Activity(Label = "EasyWords", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            SetActionBar();

        }

        private void SetActionBar()
        {
            //ActionBar.SetDisplayShowHomeEnabled(true);
            ///ActionBar.SetIcon(Resource.Drawable.ic_crop_5_4_white_36dp);
            //ActionBar.Title = "   Easy words";
        }
    }
}


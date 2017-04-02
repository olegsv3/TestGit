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
    class SelectAdapter : BaseAdapter<Card>
        { 
        private Context _context;
        private List<Card> _list;

        public SelectAdapter(Context context, List<Card> list)
        {
            _context = context;
            _list = list;
        }

        public override int Count => _list.Count;

        public override long GetItemId(int position) => position;

        public override Card this[int position] => _list[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? LayoutInflater.From(_context).Inflate(Resource.Layout.SelectItem, null);

            view.FindViewById<TextView>(Resource.Id.textView1).Text = _list[position].Word1;
            view.FindViewById<TextView>(Resource.Id.textView2).Text = _list[position].Word2;

            if (position == _list.Count - 1)
                view.FindViewById<ImageView>(Resource.Id.imageView1).Alpha = 0;

            return view;
        }
    }
}
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
using Android.Views.Animations;

namespace EasyWords
{
    [Activity(Label = "WordsActivitys", Theme = "@style/MyTheme")]
    public class WordsActivity : Activity
    {
        private FrameLayout _activeCard;
        private FrameLayout _nonActiveCard;
        private EditText _activeEditText;
        private EditText _nonActiveEditText;
        private TextView _statusText;

        private readonly float _clampLeft = -1200.0f;
        private readonly float _clampRight = 1200.0f;
        private readonly float _clampUp = -1200.0f;
        private readonly float _clampDown = 1200.0f;
        private readonly float _zero = 0.0f;

        private float _startX = default(float);
        private float _startY = default(float);

        private List<Card> _words;
        private Card _currentCard;
        private Card _originalCard;

        private int _currentIndex;
        private bool _isTranslate = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Words);
            SetActionBar();

            _words = FileWorker.GetWords();
     
            _activeCard = FindViewById<FrameLayout>(Resource.Id.ActiveCard);
            _nonActiveCard = FindViewById<FrameLayout>(Resource.Id.NonActiveCard);
            _activeEditText = _activeCard.FindViewById<EditText>(Resource.Id.editText1);
            _nonActiveEditText = _nonActiveCard.FindViewById<EditText>(Resource.Id.editText1);
            _statusText = FindViewById<TextView>(Resource.Id.textView1);

            _activeCard.TranslationX = _clampRight;
            _nonActiveCard.TranslationX = _clampRight;

            Start();
        }

        public void SetActionBar()
        {
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_keyboard_backspace_white_36dp);
            ActionBar.SetDisplayShowCustomEnabled(true);
        }

        public void Save(object sender, EventArgs e)
        {
            if (_isTranslate)
            {
                _currentCard.Word2 = _activeEditText.Text;
                _words[_currentIndex].Word2 = _activeEditText.Text;
            }
            else
            {
                _currentCard.Word1 = _activeEditText.Text;
                _words[_currentIndex].Word1 = _activeEditText.Text;
            }
        }

        public void Start()
        {
            if(_words.Count == 0)
            {
                _originalCard = new Card();
                _currentCard = new Card();
                _words.Add(new Card());

                _activeEditText.Text = string.Empty;

                _isTranslate = false;

                _activeCard.Animate().SetDuration(1000).TranslationX(_zero).Start();
            }

            else
            {
                _originalCard = _words[0];
                _currentCard = _originalCard;

                _activeEditText.Text = _currentCard.Word1;

                _isTranslate = false;

                _activeCard.Animate().SetDuration(600).TranslationX(_zero).Start();
            }
            _currentIndex = 0;
            _activeEditText.AfterTextChanged += Save;
        }

        public void Left()
        {
            if(_currentIndex == _words.Count-1)
            {
                Animation anim = AnimationUtils.LoadAnimation(this, Resource.Animation.AnimLeftClamp);
                _activeCard.StartAnimation(anim);
            }

            else
            {
                _activeEditText.AfterTextChanged -= Save;
                SaveCard();

                _currentIndex++;

                _activeCard.Animate().SetDuration(600).TranslationX(_clampLeft).Start();               

                _originalCard = _words[_currentIndex];
                _currentCard = _originalCard;
                _isTranslate = false;
                _nonActiveEditText.Text = _currentCard.Word1;

                _nonActiveCard.TranslationX = _clampRight;
                _nonActiveCard.Animate().SetDuration(600).TranslationX(_zero).Start();

                SwithCard();
                _activeEditText.AfterTextChanged += Save;
            }
        }

        public void Right()
        {
            if (_currentIndex == 0)
            {
                Animation anim = AnimationUtils.LoadAnimation(this, Resource.Animation.AnimRightClamp);
                _activeCard.StartAnimation(anim);
            }

            else
            {
                _activeEditText.AfterTextChanged -= Save;

                SaveCard();

                _currentIndex--;

                _activeCard.Animate().SetDuration(600).TranslationX(_clampRight).Start();

                _originalCard = _words[_currentIndex];
                _currentCard = _originalCard;
                _isTranslate = false;
                _nonActiveEditText.Text = _currentCard.Word1;

                _nonActiveCard.TranslationX = _clampLeft;
                _nonActiveCard.Animate().SetDuration(600).TranslationX(_zero).Start();

                SwithCard();

                _activeEditText.AfterTextChanged += Save;
            }
        }

        public void Scale()
        {
            _activeEditText.AfterTextChanged -= Save;
            Animation animDown = AnimationUtils.LoadAnimation(this, Resource.Animation.AnimScaleDown);
            Animation animUp = AnimationUtils.LoadAnimation(this, Resource.Animation.AnimScaleUp);

            _activeCard.StartAnimation(animDown);

            animDown.AnimationEnd += (s, e) =>
            {
                _isTranslate = !_isTranslate;
                _activeEditText.Text = _isTranslate ? _currentCard.Word2 : _currentCard.Word1;
                _activeCard.StartAnimation(animUp);
            };
            _activeEditText.AfterTextChanged += Save;
        }

        public void AddCard()
        {
            if (_words.Last().Word1 == string.Empty && _currentIndex != _words.Count-1)
            {
                _activeEditText.AfterTextChanged -= Save;
                SaveCard();

                _currentIndex = _words.Count - 1;

                _activeCard.Animate().SetDuration(600).TranslationX(_clampLeft).Start();

                _originalCard = _words[_currentIndex];
                _currentCard = _originalCard;
                _isTranslate = false;
                _nonActiveEditText.Text = _currentCard.Word1;

                _nonActiveCard.TranslationX = _clampRight;
                _nonActiveCard.Animate().SetDuration(600).TranslationX(_zero).Start();

                SwithCard();
                _activeEditText.AfterTextChanged += Save;
            }

            else if(_words.Last().Word1 != string.Empty)
            {
                _activeEditText.AfterTextChanged -= Save;
                SaveCard();

                _activeCard.Animate().SetDuration(600).TranslationX(_clampLeft).Start();

                _originalCard = new Card();
                _currentCard = new Card();
                _words.Add(new Card());
                _currentIndex = _words.Count - 1;
                _isTranslate = false;
                _nonActiveEditText.Text = _currentCard.Word1;

                _nonActiveCard.TranslationX = _clampRight;
                _nonActiveCard.Animate().SetDuration(600).TranslationX(_zero).Start();

                SwithCard();
                _activeEditText.AfterTextChanged += Save;
            }
        }

        public void SaveCard()
        {
            if(_originalCard != _currentCard)
            {
                FileWorker.SaveWord(_currentCard);
                _words = FileWorker.GetWords();
            }
        }

        public void SwithCard()
        {
            var tmpCard = _activeCard;
            var tmpText = _activeEditText;

            _activeCard = _nonActiveCard;
            _activeEditText = _nonActiveEditText;

            _nonActiveCard = tmpCard;
            _nonActiveEditText = tmpText;
        }



        public override bool OnTouchEvent(MotionEvent e)
        {
            MotionEventActions action = e.ActionMasked;
            switch(action)
            {
                case MotionEventActions.Down:
                    _startX = e.GetX();
                    _startY = e.GetY();
                    break;
                case MotionEventActions.Up:
                    float endX = e.GetX();
                    float endY = e.GetY();

                    if (_startX - endX > 100.0f)
                        Left();
                    else if (endX - _startX > 100.0f)
                        Right();
                    else if (Math.Abs(endY - _startY) > 50.0f && Math.Abs(endX - _startX) < 50.0f)
                        Scale();
                    break;
            }
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.buttonAdd)
                AddCard();
            else
            {
                this.OnBackPressed();
            }
            return true;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menuAdd, menu);
            return true;
        }
    }
}
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


        private readonly float _clampLeft = -1200.0f;
        private readonly float _clampRight = 1200.0f;
        private readonly float _clampUp = -1200.0f;
        private readonly float _clampDown = 1200.0f;
        private readonly float _zero = 0.0f;

        private float _startX = default(float);
        private float _startY = default(float);

        private List<Card> _words;
        private Card _currentCard;

        private int _currentIndex;
        private bool _isTranslate = false;
        private bool _isNewCard = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Words);

            _activeCard = FindViewById<FrameLayout>(Resource.Id.ActiveCard);
            _nonActiveCard = FindViewById<FrameLayout>(Resource.Id.NonActiveCard);
            
            _activeCard.TranslationX = _clampRight;
            _nonActiveCard.TranslationX = _clampRight;

            //_words = FileWorker.GetWords();
            _currentIndex = default(int);

            _words = new List<Card>
            {
                new Card{ Word1 = "Apple", Word2 = "Яблоко"},
                new Card{ Word1 = "Pineapple", Word2 = "Ананас"},
                new Card{ Word1 = "Lemon", Word2 = "Лимон"},
                new Card{ Word1 = "Orange", Word2 = "Апельсин"}
            };

            SetActionBar();
            Start();
        }

        public void SetActionBar()
        {
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_keyboard_backspace_white_36dp);
            ActionBar.SetDisplayShowCustomEnabled(true);
        }




        public void Start()
        {
            if (_words.Count > 0)
            {
                _activeCard.FindViewById<EditText>(Resource.Id.editText1).Text = _words[0].Word1;
                _currentCard = new Card { Word1 = _words[0].Word1, Word2 = _words[0].Word2};

            }
            else
            {
                _isNewCard = true;
                _currentCard = new Card();
            }
            _activeCard.Animate().SetDuration(500).TranslationX(_zero).Start();
            UpdateStatus();
        }

        public void AddCard()
        {
            SaveCard();

            _activeCard.Animate().SetDuration(500).TranslationX(_clampLeft).Start();

            _nonActiveCard.FindViewById<EditText>(Resource.Id.editText1).Text = "";
            _nonActiveCard.TranslationY = _clampUp;
            _nonActiveCard.TranslationX = _zero;
            _nonActiveCard.Animate().SetDuration(500).TranslationY(_zero).Start();

            SwitchActiveCard();

            _currentIndex = _words.Count;
            _isNewCard = true;
            _isTranslate = false;
            _currentCard = new Card();
        }

        public void SwitchActiveCard()
        {
            var tmp = _activeCard;
            _activeCard = _nonActiveCard;
            _nonActiveCard = tmp;
        }      

        public void Left()
        {
            if (_currentIndex > _words.Count - 2)
            {
                Animation anim = AnimationUtils.LoadAnimation(this, Resource.Animation.AnimLeftClamp);
                _activeCard.StartAnimation(anim);
            }
            else
            {
                _activeCard.Animate().SetDuration(500).TranslationX(_clampLeft).Start();

                _currentIndex++;
                _isTranslate = false;
                _currentCard = new Card { Word1 = _words[_currentIndex].Word1, Word2 = _words[_currentIndex].Word2 };
                _nonActiveCard.FindViewById<EditText>(Resource.Id.editText1).Text = _currentCard.Word1;

                _nonActiveCard.TranslationX = _clampRight;
                _nonActiveCard.Animate().SetDuration(500).TranslationX(_zero).Start();                

                SwitchActiveCard();
                SaveCard();
                UpdateStatus();
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
                _activeCard.Animate().SetDuration(500).TranslationX(_clampRight).Start();

                _currentIndex--;
                _isTranslate = false;
                _currentCard = new Card { Word1 = _words[_currentIndex].Word1, Word2 = _words[_currentIndex].Word2 };
                _nonActiveCard.FindViewById<EditText>(Resource.Id.editText1).Text = _currentCard.Word1;

                _nonActiveCard.TranslationX = _clampLeft;
                _nonActiveCard.Animate().SetDuration(500).TranslationX(_zero).Start();

                SwitchActiveCard();
                SaveCard();
                UpdateStatus();
            }
        }

        public void Scale()
        {
            Animation animDown = AnimationUtils.LoadAnimation(this, Resource.Animation.AnimScaleDown);
            Animation animUp = AnimationUtils.LoadAnimation(this, Resource.Animation.AnimScaleUp);

            _activeCard.StartAnimation(animDown);
            animDown.AnimationEnd += (s, e) =>
            {
                _isTranslate = !_isTranslate;
                SwitchTextInCard();                
                _activeCard.StartAnimation(animUp);
            };
        }

        public void SwitchTextInCard()
        {
            if (_isTranslate)
                _activeCard.FindViewById<EditText>(Resource.Id.editText1).Text = _currentCard.Word2;
            else
                _activeCard.FindViewById<EditText>(Resource.Id.editText1).Text = _currentCard.Word1;
        }

        public void SaveCard()
        {
            if(_isNewCard)
            {
                if(_currentCard.Word1 != "")
                {

                }
            }

            else if(_words[_currentIndex].Word1 != _currentCard.Word1 || _words[_currentIndex].Word2 != _currentCard.Word2)
            { 

            }
        }

        public void SaveCardToList()
        {

        }

        public void SaveCardToFile()
        {

        }

        public void EditCardInList()
        {

        }

        public void EditCardInFile()
        {

        }

        public void UpdateStatus()
        {
            FindViewById<TextView>(Resource.Id.textView1).Text = $"{_currentIndex + 1}/{_words.Count}";
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
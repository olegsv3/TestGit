using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EasyWords
{
    static class FileWorker
    { 
        static readonly private string _path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        static readonly private string _fileName = Path.Combine(_path, "words.txt");

        static private StreamReader _reader;
        static private StreamWriter _writer;

        static private List<string> _list;
        static private List<DataItem> _ListData;

        static public int GetLastIndex
        {
            get
            {
                if (_ListData.Count == 0 || GetWords().Count ==0)
                    return 0;
                int i = 0;
                int index = -1;
                while(index == -1)
                {
                    index = _ListData[_ListData.Count - ++i].Key;
                }
                return index;                
            }
        }

        static public string ActiveLanguage { get; set; }
        static public string ActiveCategory { get; set; }

        static public void Start()
        {
            
            File.Create(_fileName).Dispose();
            UpdateList();
        }

        static public void UpdateList()
        {
            _list = new List<string>();
            _ListData = new List<DataItem>();

            string line = string.Empty;

            _reader = new StreamReader(_fileName);

            while ((line = _reader.ReadLine()) != null)
            {
                _list.Add(line);
            }
            _reader.Close();

            foreach (var str in _list)
            {
                string[] tmp = str.Split('|');
                var dataItem = new DataItem()
                {
                    Key = tmp.Length > 0 ? int.Parse(tmp[0]) : 0,
                    Language = tmp.Length > 1 ? tmp[1] : "",
                    Category = tmp.Length > 2 ? tmp[2] : "",
                    Word1 = tmp.Length > 3 ? tmp[3] : "",
                    Word2 = tmp.Length > 4 ? tmp[4] : ""
                };
                _ListData.Add(dataItem);
            }
        }

        static public List<Card> GetLanguages()
        {
            if (_ListData != null)
            {
                var langs = from lang in _ListData
                            group lang by lang.Language into tmp
                            select new Card
                            {
                                Word1 = tmp.Key,
                                Word2 = (((from cat in _ListData
                                          where cat.Language == tmp.Key
                                          group cat by cat.Category into z
                                          select z).Count() - 1).ToString()) + " categories"
                            };

                return langs.ToList<Card>();
            }
            return new List<Card>();
        }

        static public bool AddLanguage(string lang)
        {
            foreach (var card in GetLanguages())
            {
                if (card.Word1 == lang) return false;
            }
            _writer = new StreamWriter(_fileName, true);
            _writer.WriteLine($"-1|{lang}");
            _writer.Close();
            UpdateList();
            return true;
        }

        static public bool AddCategory(string cat)
        {
            var formatString = $"-1|{ActiveLanguage}|{cat}";
            if (_list.Contains(formatString)) return false;
            _writer = new StreamWriter(_fileName, true);
            _writer.WriteLine(formatString);
            _writer.Close();
            UpdateList();
            return true;
        }

        static public List<Card> GetCategories()
        {
            var cats = (from cat in _ListData
                        where cat.Language == ActiveLanguage && cat.Category != ""
                        group cat by cat.Category into tmp
                        select new Card
                        {
                            Word1 = tmp.Key,
                            Word2 = (((from word in tmp
                                       where word.Language == ActiveLanguage && word.Category != ""
                                       where word.Category == tmp.Key
                                       group word by word.Category into z
                                       select z).Count() - 1).ToString()) + " words"
                        });

            return cats.ToList<Card>();
        }

        static public void SaveWord(Card card)
        {
            string formatString = $"{GetLastIndex+1}|{ActiveLanguage}|{ActiveCategory}|{card.Word1}|{card.Word2}";
            if (card.Key == default(int))
            {
                _writer = new StreamWriter(_fileName, true);
                _writer.WriteLine(formatString);
                _writer.Close();
            }
            else
            {
                var index = _list.FindIndex(c => c.StartsWith($"{card.Key}|"));
                _list[index] = formatString;
                ResafeFile();
            }
            UpdateList();
        }

        static public void ResafeFile()
        {
            File.Create(_fileName).Dispose();
            _writer = new StreamWriter(_fileName, true);
            foreach (var item in _list)
            {
                _writer.WriteLine(item);
            }
            _writer.Close();
        }


        static public List<Card> GetWords()
        {
            return (from card in _ListData
                    where card.Language == ActiveLanguage && card.Category == ActiveCategory && card.Word1 != ""
                    select new Card { Key = card.Key, Word1 = card.Word1, Word2 = card.Word2 }).ToList<Card>();
        }

        static public bool TestString(string s) => !(s.Contains("|") || s == "" || s.Contains(" ") || s.Contains("\n"));

        static public bool TestStringCat(string s) => !(s.Contains("|") || s.Contains("\n"));

    }
}
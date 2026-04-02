using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class Class1
    {
        private List<string> list = new List<string>();
        private string filename;
        private int count;

        public Class1(string filename)
        {
            this.filename = filename;
            OpenFile();
            count = list.Count;
        }
        public int Count => count;

        public List<string> GetWords() => new List<string>(list);

        private void OpenFile()
        {
            try
            {
                list.Clear();
                if (File.Exists(filename))
                {
                    using (StreamReader f = new StreamReader(filename, Encoding.UTF8))
                    {
                        while (!f.EndOfStream)
                        {
                            string word = f.ReadLine()?.Trim();
                            if (!string.IsNullOrEmpty(word))
                                list.Add(word);
                        }
                    }
                }
                count = list.Count;
            }
            catch
            {
                throw new Exception("Ошибка доступа к файлу!");
            }
        }
        public void AddWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return;
            word = word.Trim();
            if (!list.Contains(word))
            {
                list.Add(word);
                count = list.Count;
            }
        }
        public void RemoveWord(string word)
        {
            if (list.Contains(word))
            {
                list.Remove(word);
                count = list.Count;
            }
        }
        public void SaveToFile(string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                foreach (var word in list.OrderBy(w => w))
                {
                    sw.WriteLine(word);
                }
            }
        }
        public List<string> FuzzySearch(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return new List<string>();

            var result = new List<string>();
            foreach (var word in list)
            {
                if (LevenshteinDistance(word, pattern) <= 3)
                    result.Add(word);
            }
            return result;
        }
        private int LevenshteinDistance(string s, string t)
        {
            int n = s.Length, m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }
        public List<string> FindUniqueLettersWords(int length)
        {
            return list.Where(word => word.Length == length &&
                word.Distinct().Count() == word.Length).ToList();
        }
        public List<string> GetWordsStartingWith(string prefix)
        {
            return list.Where(w => w.StartsWith(prefix,
                StringComparison.OrdinalIgnoreCase))
                .OrderBy(w => w)
                .ToList();
        }
    }
}
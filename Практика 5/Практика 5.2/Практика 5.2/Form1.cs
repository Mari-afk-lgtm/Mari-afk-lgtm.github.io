using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ClassLibrary1;

namespace Практика_5._2
{
    public partial class Form1 : Form
    {
        private Class1 currentDictionary;
        private string currentFilePath;

        public Form1()
        {
            InitializeComponent();

            tabPage1.Text = "Словарь";
            tabPage2.Text = "Поиск";
            tabPage3.Text = "Фильтры";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Текстовые файлы|*.txt|Все файлы|*.*",
                Title = "Выберите файл словаря"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentFilePath = ofd.FileName;
                    currentDictionary = new Class1(currentFilePath);
                    label1.Text = $"Словарь загружен: {currentDictionary.Count} слов";
                    UpdateWordList(currentDictionary.GetWords());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
        private void UpdateWordList(List<string> words)
        {
            listBox1.DataSource = null;
            listBox1.DataSource = words;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (currentDictionary == null)
            {
                MessageBox.Show("Сначала загрузите словарь");
                return;
            }
            string word = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(word))
            {
                MessageBox.Show("Введите слово для добавления");
                return;
            }
            currentDictionary.AddWord(word);
            UpdateWordList(currentDictionary.GetWords());
            textBox1.Clear();
            label1.Text = $"Слово добавлено. Всего: {currentDictionary.Count}";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (currentDictionary == null)
            {
                MessageBox.Show("Сначала загрузите словарь");
                return;
            }
            if (listBox1.SelectedItem != null)
            {
                string word = listBox1.SelectedItem.ToString();
                currentDictionary.RemoveWord(word);
                UpdateWordList(currentDictionary.GetWords());
                label1.Text = $"Слово удалено. Всего: {currentDictionary.Count}";
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (currentDictionary == null)
            {
                MessageBox.Show("Сначала загрузите словарь");
                return;
            }
            if (!int.TryParse(textBox2.Text, out int length) || length <= 0)
            {
                MessageBox.Show("Введите корректную длину слова");
                return;
            }
            var result = currentDictionary.FindUniqueLettersWords(length);
            listBox1.DataSource = result;
            label1.Text = $"Найдено слов: {result.Count}";

            if (result.Count == 0)
                MessageBox.Show("Поиск не дал результатов");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Текстовые файлы|*.txt",
                Title = "Сохранить результаты поиска"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var words = (List<string>)listBox1.DataSource;
                File.WriteAllLines(sfd.FileName, words);
                MessageBox.Show("Результаты сохранены");
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (currentDictionary == null)
            {
                MessageBox.Show("Сначала загрузите словарь");
                return;
            }
            string pattern = textBox3.Text.Trim();
            if (string.IsNullOrEmpty(pattern))
            {
                MessageBox.Show("Введите шаблон для нечёткого поиска");
                return;
            }
            var result = currentDictionary.FuzzySearch(pattern);
            listBox1.DataSource = result;
            label1.Text = $"Нечёткий поиск: найдено {result.Count}";

            if (result.Count == 0)
                MessageBox.Show("Ничего не найдено");
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (currentDictionary == null)
            {
                MessageBox.Show("Нет загруженного словаря");
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Текстовые файлы|*.txt",
                Title = "Сохранить словарь"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                currentDictionary.SaveToFile(sfd.FileName);
                MessageBox.Show("Словарь сохранён");
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Текстовые файлы|*.txt",
                Title = "Создать новый словарь"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, "");
                currentFilePath = sfd.FileName;
                currentDictionary = new Class1(currentFilePath);
                UpdateWordList(new List<string>());
                label1.Text = "Создан новый пустой словарь";
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (currentDictionary != null)
                UpdateWordList(currentDictionary.GetWords());
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (currentDictionary == null)
            {
                MessageBox.Show("Сначала загрузите словарь");
                return;
            }
            string prefix = textBox4.Text.Trim();
            if (string.IsNullOrEmpty(prefix))
            {
                MessageBox.Show("Введите начальные буквы");
                return;
            }
            var result = currentDictionary.GetWordsStartingWith(prefix);
            listBox1.DataSource = result;
            label1.Text = $"Найдено слов, начинающихся на '{prefix}': {result.Count}";
        }
    }
}
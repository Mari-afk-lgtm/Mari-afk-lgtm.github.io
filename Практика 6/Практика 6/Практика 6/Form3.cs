using System;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Практика_6
{
    public partial class Form3 : Form
    {
        private string xmlPath = "quiz.xml";
        public Form3()
        {
            InitializeComponent();
            LoadTopics();
        }
        private void LoadTopics()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList topics = doc.SelectNodes("/quiz/topic");
            comboBox1.Items.Clear();
            foreach (XmlNode t in topics)
                comboBox1.Items.Add(t.Attributes["name"].Value);
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           string newTopic = ShowInputDialog("Введите название века:", "Новая тема");
            if (!string.IsNullOrEmpty(newTopic))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
                XmlElement root = doc.DocumentElement;
                XmlElement topicElem = doc.CreateElement("topic");
                topicElem.SetAttribute("name", newTopic);

                // Создаём 3 уровня
                for (int i = 1; i <= 3; i++)
                {
                    XmlElement levelElem = doc.CreateElement("level");
                    levelElem.SetAttribute("difficulty", i.ToString());
                    topicElem.AppendChild(levelElem);
                }

                root.AppendChild(topicElem);
                doc.Save(xmlPath);
                LoadTopics();
                MessageBox.Show("Тема добавлена!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите тему!");
                return;
            }

            string topicName = comboBox1.SelectedItem.ToString();
            int difficulty = comboBox2.SelectedIndex + 1;

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);

            // Находим нужную тему и уровень
            XmlNode topicNode = doc.SelectSingleNode($"/quiz/topic[@name='{topicName}']");
            XmlNode levelNode = topicNode.SelectSingleNode($"level[@difficulty='{difficulty}']");

            // Создаём вопрос
            XmlElement questionElem = doc.CreateElement("question");
            questionElem.SetAttribute("text", textBox1.Text);

            // Изображение
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                string fileName = Path.GetFileName(textBox2.Text);
                questionElem.SetAttribute("image", "images/" + fileName);
                // Копируем файл в папку images
                File.Copy(textBox2.Text, "images/" + fileName, true);
            }

            // Добавляем варианты ответов
            string[] answers = { textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text };
            for (int i = 0; i < answers.Length; i++)
            {
                if (!string.IsNullOrEmpty(answers[i]))
                {
                    XmlElement answerElem = doc.CreateElement("answer");
                    if (i == 0 && checkBox1.Checked) answerElem.SetAttribute("right", "yes");
                    if (i == 1 && checkBox2.Checked) answerElem.SetAttribute("right", "yes");
                    if (i == 2 && checkBox3.Checked) answerElem.SetAttribute("right", "yes");
                    if (i == 3 && checkBox4.Checked) answerElem.SetAttribute("right", "yes");
                    answerElem.InnerText = answers[i];
                    questionElem.AppendChild(answerElem);
                }
            }

            // Добавляем подсказку
            if (!string.IsNullOrEmpty(textBox7.Text))
            {
                XmlElement hintElem = doc.CreateElement("hint");
                hintElem.InnerText = textBox7.Text;
                questionElem.AppendChild(hintElem);
            }

            levelNode.AppendChild(questionElem);
            doc.Save(xmlPath);
            MessageBox.Show("Вопрос сохранён!");
            ClearForm();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
                textBox2.Text = ofd.FileName;
        }
        private void ClearForm()
        {
            textBox1.Clear();
            textBox3.Clear(); textBox4.Clear(); textBox5.Clear(); textBox6.Clear();
            checkBox1.Checked = checkBox2.Checked = checkBox3.Checked = checkBox4.Checked = false;
            textBox8.Clear();
            textBox2.Clear();
        }
        private string ShowInputDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label() { Left = 20, Top = 20, Text = text, Width = 350 };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 350 };
            Button confirmation = new Button() { Text = "OK", Left = 250, Width = 100, Top = 80, DialogResult = DialogResult.OK };
            Button cancel = new Button() { Text = "Отмена", Left = 140, Width = 100, Top = 80, DialogResult = DialogResult.Cancel };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}

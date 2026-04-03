using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Практика_6
{
    public partial class Form2 : Form
    {
        private string topic;
        private int level;
        private List<XmlNode> questions;
        private List<XmlNode> currentSession;
        private int currentIndex = 0;
        private int score = 0;
        private int timeLeft = 60;
        private Timer timer;
        private int questionsPerSession = 5; // 5 вопросов за сеанс

        public Form2(string topic, int level)
        {
            InitializeComponent();
            this.FormClosing += Form2_FormClosing;
            this.topic = topic;
            this.level = level;

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer1_Tick;

            LoadQuestions();
            StartNewSession();
            DisplayCurrentQuestion();
            timer.Start();
        }

        // ИСПРАВЛЕНО: загружает вопросы из выбранной темы и уровня
       
           private void LoadQuestions()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("quiz.xml");
            questions = new List<XmlNode>();

            string xpath = $"/quiz/topic[@name='{topic}']/level[@difficulty='{level}']/question";

            // ОТЛАДКА: показываем, что ищем
            MessageBox.Show($"Ищем вопросы по пути: {xpath}");

            XmlNodeList allQuestions = doc.SelectNodes(xpath);

            // ОТЛАДКА: сколько нашли
            MessageBox.Show($"Найдено вопросов: {allQuestions.Count}");

            foreach (XmlNode q in allQuestions)
            {
                questions.Add(q);
            }

            if (questions.Count == 0)
            {
                MessageBox.Show($"Вопросы не найдены!\nТема: '{topic}'\nУровень: {level}");
                this.Close();
            }
        }
        

        // Выбрать 5 случайных вопросов для текущего сеанса
        private void StartNewSession()
        {
            Random rnd = new Random();

            if (questions.Count >= questionsPerSession)
            {
                currentSession = questions.OrderBy(q => rnd.Next()).Take(questionsPerSession).ToList();
            }
            else
            {
                // Если вопросов меньше 5, повторяем существующие
                currentSession = new List<XmlNode>();
                for (int i = 0; i < questionsPerSession; i++)
                {
                    currentSession.Add(questions[i % questions.Count]);
                }
                currentSession = currentSession.OrderBy(q => rnd.Next()).ToList();
            }

            currentIndex = 0;
            score = 0;
            UpdateScoreLabel();
            UpdateQuestionCounter();
        }

        // Отобразить текущий вопрос
        private void DisplayCurrentQuestion()
        {
            if (currentIndex >= currentSession.Count)
            {
                FinishLevel();
                return;
            }

            XmlNode q = currentSession[currentIndex];

            // Текст вопроса (ищем в разных форматах)
            XmlNode textNode = q.SelectSingleNode("text");
            if (textNode != null)
                label1.Text = textNode.InnerText;
            else if (q.Attributes["text"] != null)
                label1.Text = q.Attributes["text"].Value;
            else
                label1.Text = "Вопрос без текста";

            // Изображение
            XmlNode imageNode = q.SelectSingleNode("image");
            if (imageNode != null)
            {
                string imgFile = imageNode.InnerText;
                string fullPath = Path.Combine(Application.StartupPath, imgFile);
                if (File.Exists(fullPath))
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(fullPath);
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch { pictureBox1.Image = null; }
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
            else if (q.Attributes["image"] != null)
            {
                string imgFile = q.Attributes["image"].Value;
                string fullPath = Path.Combine(Application.StartupPath, imgFile);
                if (File.Exists(fullPath))
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(fullPath);
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch { pictureBox1.Image = null; }
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
            else
            {
                pictureBox1.Image = null;
            }

            // Варианты ответов (ищем в разных форматах)
            XmlNodeList answers = q.SelectNodes("answer");
            RadioButton[] rb = { radioButton1, radioButton2, radioButton3, radioButton4 };

            // Если нет answer, ищем correct/wrong
            if (answers.Count == 0)
            {
                // Формат с correct и wrong
                List<string> allAnswers = new List<string>();
                XmlNode correctNode = q.SelectSingleNode("correct");
                if (correctNode != null)
                    allAnswers.Add(correctNode.InnerText);

                XmlNodeList wrongNodes = q.SelectNodes("wrong");
                foreach (XmlNode w in wrongNodes)
                    allAnswers.Add(w.InnerText);

                // Перемешиваем
                Random rnd = new Random();
                allAnswers = allAnswers.OrderBy(x => rnd.Next()).ToList();

                for (int i = 0; i < rb.Length && i < allAnswers.Count; i++)
                {
                    rb[i].Text = allAnswers[i];
                    rb[i].Checked = false;
                    rb[i].Visible = true;
                }

                // Сохраняем правильный ответ в Tag кнопки для проверки
                button1.Tag = correctNode?.InnerText;
            }
            else
            {
                // Старый формат с answer
                for (int i = 0; i < rb.Length && i < answers.Count; i++)
                {
                    rb[i].Text = answers[i].InnerText;
                    rb[i].Checked = false;
                    rb[i].Visible = true;
                }
                button1.Tag = null;
            }

            // Скрываем лишние кнопки
            for (int i = answers.Count; i < rb.Length; i++)
            {
                rb[i].Visible = false;
            }

            // Подсказка
            XmlNode hintNode = q.SelectSingleNode("hint");
            button2.Tag = hintNode?.InnerText ?? "Подсказка отсутствует";

            UpdateQuestionCounter();
        }

        private void UpdateQuestionCounter()
        {
            label4.Text = $"Вопрос {currentIndex + 1}/{currentSession.Count}";
        }

        private void button1_Click(object sender, EventArgs e)  // Проверка ответа
        {
            string selectedAnswer = "";
            RadioButton[] rb = { radioButton1, radioButton2, radioButton3, radioButton4 };

            foreach (var r in rb)
            {
                if (r.Checked && r.Visible)
                {
                    selectedAnswer = r.Text;
                    break;
                }
            }

            if (string.IsNullOrEmpty(selectedAnswer))
            {
                MessageBox.Show("Выберите вариант ответа!");
                return;
            }

            XmlNode currentQ = currentSession[currentIndex];

            // Определяем правильный ответ в зависимости от формата XML
            string correctAnswer = "";

            // Проверяем формат с answer
            XmlNode rightAnswerNode = currentQ.SelectSingleNode("answer[@right='yes']");
            if (rightAnswerNode != null)
            {
                correctAnswer = rightAnswerNode.InnerText;
            }
            else
            {
                // Формат с correct
                XmlNode correctNode = currentQ.SelectSingleNode("correct");
                if (correctNode != null)
                {
                    correctAnswer = correctNode.InnerText;
                }
            }

            int pointsPerQuestion = 100 / currentSession.Count;

            if (correctAnswer == selectedAnswer)
            {
                score += pointsPerQuestion;
                MessageBox.Show($"Правильно! +{pointsPerQuestion} баллов", "Отлично!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Неправильно! Правильный ответ: {correctAnswer}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            UpdateScoreLabel();
            currentIndex++;

            if (currentIndex < currentSession.Count)
                DisplayCurrentQuestion();
            else
                FinishLevel();
        }

        private void UpdateScoreLabel()
        {
            label3.Text = $"Счёт: {score}/100";
        }

        private void FinishLevel()
        {
            if (timer != null)
                timer.Stop();

            if (score >= 80)
            {
                DialogResult res = MessageBox.Show($"Вы набрали {score} баллов!\nПерейти на следующий уровень?",
                    "Успех!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes && level < 3)
                {
                    level++;
                    LoadQuestions();
                    StartNewSession();
                    DisplayCurrentQuestion();
                    timeLeft = 60;
                    UpdateTimerDisplay();
                    if (timer != null)
                        timer.Start();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show($"Вы набрали {score} баллов.\nДля перехода на следующий уровень нужно 80+ баллов.",
                    "Недостаточно баллов", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)  // Подсказка
        {
            string hint = button2.Tag?.ToString() ?? "Нет подсказки";
            MessageBox.Show(hint, "Подсказка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateTimerDisplay()
        {
            // Обновляем отображение таймера
            label3.Text = $"⏱ {timeLeft / 60:D2}:{timeLeft % 60:D2}";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                UpdateTimerDisplay();
            }

            if (timeLeft <= 0)
            {
                if (timer != null)
                    timer.Stop();
                MessageBox.Show("Время вышло! Возврат в главное меню.", "Время закончилось",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}
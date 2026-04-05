using System;
using System.Windows.Forms;
using System.Xml;

namespace Практика_6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadTopicsIntoComboBox();
        }
        private void LoadTopicsIntoComboBox()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("quiz.xml");
                XmlNodeList topics = doc.SelectNodes("/quiz/topic");

                comboBox1.Items.Clear();
                foreach (XmlNode topic in topics)
                {
                    comboBox1.Items.Add(topic.Attributes["name"].Value);
                }
                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки XML: " + ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите век!");
                return;
            }
            string selectedTopic = comboBox1.SelectedItem.ToString();
            int difficulty = comboBox2.SelectedIndex + 1; 

            Form2 quizForm = new Form2(selectedTopic, difficulty);
            quizForm.ShowDialog(); 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 adminForm = new Form3();
            adminForm.ShowDialog();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

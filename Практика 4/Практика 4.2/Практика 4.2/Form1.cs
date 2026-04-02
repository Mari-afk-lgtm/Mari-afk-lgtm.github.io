using System;
using System.Windows.Forms;

namespace Практика_4._2
{
    public partial class Form1 : Form
    {
        private CountriesList countries;
        public Form1()
        {
            InitializeComponent();
            countries = new CountriesList();
            InitializeDataGridView();

            tabPage1.Text = "Ввод данных";
            tabPage2.Text = "Таблица";
            tabPage3.Text = "Диаграмма";
        }
        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("colName", "Страна");
            dataGridView1.Columns.Add("colGold", "Золото");
            dataGridView1.Columns.Add("colSilver", "Серебро");
            dataGridView1.Columns.Add("colBronze", "Бронза");
            dataGridView1.Columns.Add("colPoints", "Баллы");

            dataGridView1.Columns["colName"].Width = 150;
            dataGridView1.Columns["colGold"].Width = 80;
            dataGridView1.Columns["colSilver"].Width = 80;
            dataGridView1.Columns["colBronze"].Width = 80;
            dataGridView1.Columns["colPoints"].Width = 80;

            dataGridView1.AutoGenerateColumns = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {MessageBox.Show("Введите название страны");
                return;}

            if (!int.TryParse(textBox2.Text, out int gold) ||
                !int.TryParse(textBox3.Text, out int silver) ||
                !int.TryParse(textBox4.Text, out int bronze))
            {MessageBox.Show("Введите корректные числа медалей");
                return;}

            countries.Add(new Country(name, gold, silver, bronze));
            RefreshDataGridView();
            RefreshChart();
            ClearInput();
        }
        private void RefreshDataGridView()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < countries.Count; i++)
            {
                var c = countries[i];
                dataGridView1.Rows.Add(c.Name, c.Gold, c.Silver, c.Bronze, c.TotalPoints);
            }
        }
        private void RefreshChart()
        {
            if (countries.Count > 0)
                countries.DrawPieChart(chart1);
            else
                chart1.Series.Clear();
        }
        private void ClearInput()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Текстовые файлы|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                countries.SaveToFile(sfd.FileName);
                MessageBox.Show("Данные сохранены");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Текстовые файлы|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    countries.LoadFromFile(ofd.FileName);
                    RefreshDataGridView();
                    RefreshChart();
                    MessageBox.Show("Данные загружены");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки: " + ex.Message);
                }
            }
            }
    }
}

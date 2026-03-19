using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FractionApp;
using System.IO;

namespace Практика_2
{
    public partial class Form1 : Form
    {
        private List<string> fractionLines = new List<string>();
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {textBox1.Text = openFileDialog1.FileName;}
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {MessageBox.Show("Выберите файл для загрузки.", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;}

            try
            {
                fractionLines.Clear();
                listBox1.Items.Clear();
                listBox2.Items.Clear();

                string[] lines = File.ReadAllLines(textBox1.Text);

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        fractionLines.Add(line.Trim());
                        listBox1.Items.Add(line.Trim());
                    }
                }
                if (fractionLines.Count > 0)
                {
                    button2.Enabled = true;
                    MessageBox.Show($"Загружено {fractionLines.Count} дробей.", "Успешно",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Файл не содержит данных.", "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Clear();
                var results = FractionProcessor.ProcessFractions(fractionLines);

                foreach (string result in results)
                {listBox2.Items.Add(result);}

                MessageBox.Show("Обработка завершена.", "Успешно",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

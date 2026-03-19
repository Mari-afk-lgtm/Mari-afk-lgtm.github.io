using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double x = double.Parse(textBox1.Text);
                double epsilon = double.Parse(textBox2.Text);

                if (Math.Abs(x) >= 1)
                {
                    MessageBox.Show("x должен быть |x| < 1", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (epsilon <= 0)
                {
                    MessageBox.Show("Точность должна быть положительным числом", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                double sum = 0;
                double term = x; 
                int n = 1;
                double previousTerm;

                do
                {
                    previousTerm = term;
                    sum += term;
                    n++;
                    term = -term * x * (n - 1) / n; 
                } 
                while (Math.Abs(term) > epsilon); 

                double mathValue = Math.Log(1 + x);

                label4.Text = $"Sin(x) {mathValue:F4}";
                label5.Text = $"Сумма ряда {sum:F4}";
                label6.Text = $"Количество членов ряда {n}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               e.KeyChar != ',' && e.KeyChar != '-')
            {e.Handled = true;}
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
                e.Handled = true;
            if (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0)
                e.Handled = true;
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {e.Handled = true;}
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
                e.Handled = true;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {CheckFields();}
        private void textBox2_TextChanged(object sender, EventArgs e)
        {CheckFields();}
        private void CheckFields()
        {
            button1.Enabled = !string.IsNullOrWhiteSpace(textBox1.Text) &&
                                 !string.IsNullOrWhiteSpace(textBox2.Text);
        }
    }
}

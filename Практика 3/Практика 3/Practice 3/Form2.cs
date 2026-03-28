using System;
using System.Drawing;
using System.Windows.Forms;

namespace Practice_3
{
    public partial class Form2 : Form
    {
        public Color SelectedColor { get; private set; }
        public int TimerInterval { get; private set; }
        public float Speed { get; private set; }
        public float MorphStep { get; private set; }

        public Form2(Color currentColor, int currentInterval,
                            float currentSpeed, float currentMorphStep)
        {
            InitializeComponent();

            SelectedColor = currentColor;
            TimerInterval = currentInterval;
            Speed = currentSpeed;
            MorphStep = currentMorphStep / 10; 

            panel1.BackColor = currentColor;
            trackBar3.Value = currentInterval;
            trackBar1.Value = (int)currentSpeed;
            trackBar2.Value = (int)(currentMorphStep * 10);

            UpdateLabels();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = SelectedColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                SelectedColor = colorDialog1.Color;
                panel1.BackColor = SelectedColor;
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Speed = trackBar1.Value;
            label1.Text = Speed.ToString();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            MorphStep = trackBar2.Value / 10f;
            label2.Text = MorphStep.ToString("0.0");
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            TimerInterval = trackBar3.Value;
            label3.Text = TimerInterval.ToString();
        }
        private void UpdateLabels()
        {
            label1.Text = Speed.ToString();
            label2.Text = MorphStep.ToString("0.0");
            label3.Text = TimerInterval.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MorphStep = trackBar2.Value / 10f;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Practice_3
{
    public partial class Form1 : Form
    {
        private float x = 100;          
        private float y = 100;         
        private float baseRadius = 40;   
        private float currentWidth = 40; 
        private float currentHeight = 40;

        private int directionY = 1;       
        private int morphDirection = 1;    
        private float morphStep = 0.5f;   
        private float speed = 3;

        private Color figureColor = Color.SteelBlue;

        private bool isRunning = true;
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            this.KeyPreview = true; 
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!isRunning) return;

            y += speed * directionY;

            float maxY = this.ClientSize.Height - currentHeight / 2 - 10;
            float minY = currentHeight / 2 + 10;

            if (y > maxY)
            {
                y = maxY;
                directionY = -1; 
            }
            else if (y < minY)
            {
                y = minY;
                directionY = 1; 
            }
            currentWidth += morphDirection * morphStep;

            if (currentWidth > baseRadius * 2.5f)
            {
                currentWidth = baseRadius * 2.5f;
                morphDirection = -1; 
            }
            else if (currentWidth < baseRadius)
            {
                currentWidth = baseRadius;
                morphDirection = 1; 
            }
            this.Invalidate();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            RectangleF rect = new RectangleF(
                x - currentWidth / 2,
                y - currentHeight / 2,
                currentWidth,
                currentHeight
            );
            using (SolidBrush brush = new SolidBrush(figureColor))
            {g.FillEllipse(brush, rect);}
            using (Pen pen = new Pen(Color.Black, 2))
            {g.DrawEllipse(pen, rect);}
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                g.DrawLine(pen, x - 20, y, x + 20, y);
                g.DrawLine(pen, x, y - 20, x, y + 20);
            }  
    }
        private void Form1_Resize(object sender, EventArgs e)
        {
            float maxY = this.ClientSize.Height - currentHeight / 2 - 10;
            float minY = currentHeight / 2 + 10;

            if (y > maxY) y = maxY;
            if (y < minY) y = minY;

            float maxX = this.ClientSize.Width - currentWidth / 2 - 10;
            float minX = currentWidth / 2 + 10;

            if (x > maxX) x = maxX;
            if (x < minX) x = minX;

            this.Invalidate();
        }
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {Close();}
        private void button1_Click(object sender, EventArgs e)
        {OpenSettingsForm();}
        private void OpenSettingsForm()
        {
            timer1.Stop();
            Form2 settingsForm = new Form2(
                figureColor,
                timer1.Interval,
                speed,
                morphStep
            );
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                figureColor = settingsForm.SelectedColor;
                timer1.Interval = settingsForm.TimerInterval;
                speed = settingsForm.Speed;
                morphStep = settingsForm.MorphStep;
            }
            timer1.Start();
        }
        public Color FigureColor
        {
            get { return figureColor; }
            set { figureColor = value; this.Invalidate(); }
        }
        public int TimerInterval
        {
            get { return timer1.Interval; }
            set { timer1.Interval = value; }
        }
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public float MorphStep
        {
            get { return morphStep; }
            set { morphStep = value; }
        }
    }
}

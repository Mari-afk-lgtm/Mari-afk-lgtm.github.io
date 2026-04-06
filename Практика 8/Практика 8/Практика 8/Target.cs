using System;
using System.Drawing;

namespace ShootingGame
{
    public class Target
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Radius { get; set; } = 20;
        public float SpeedX { get; set; }
        public float SpeedY { get; set; }
        public Color Color { get; set; } = Color.Red;

        public Target(float x, float y, float speedX, float speedY)
        {
            X = x;
            Y = y;
            SpeedX = speedX;
            SpeedY = speedY;
        }
        public void Move(int width, int height)
        {
            X += SpeedX;
            Y += SpeedY;

            if (X - Radius <= 0 || X + Radius >= width)
                SpeedX = -SpeedX;
            if (Y - Radius <= 0 || Y + Radius >= height)
                SpeedY = -SpeedY;

            X = Math.Max(Radius, Math.Min(width - Radius, X));
            Y = Math.Max(Radius, Math.Min(height - Radius, Y));
        }
        public void Draw(Graphics g)
        {
            using (SolidBrush brush = new SolidBrush(Color))
            using (Pen pen = new Pen(Color.Black, 2))
            {
                g.FillEllipse(brush, X - Radius, Y - Radius, Radius * 2, Radius * 2);
                g.DrawEllipse(pen, X - Radius, Y - Radius, Radius * 2, Radius * 2);

                pen.Color = Color.White;
                g.DrawLine(pen, X - Radius / 2, Y, X + Radius / 2, Y);
                g.DrawLine(pen, X, Y - Radius / 2, X, Y + Radius / 2);
            }
        }
        public Rectangle GetBounds()
        {return new Rectangle((int)(X - Radius), (int)(Y - Radius), Radius * 2, Radius * 2);}
    }
}
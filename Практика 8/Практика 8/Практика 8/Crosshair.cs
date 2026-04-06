using System;
using System.Drawing;

namespace ShootingGame
{
    public class Crosshair
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; } = 20;
        public Color Color { get; set; } = Color.White;

        public Crosshair(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void Move(int dx, int dy, int width, int height)
        {
            X += dx;
            Y += dy;
            X = Math.Max(Size, Math.Min(width - Size, X));
            Y = Math.Max(Size, Math.Min(height - Size, Y));
        }
        public void Draw(Graphics g)
        {
            using (Pen pen = new Pen(Color, 2))
            {
                g.DrawLine(pen, X - Size, Y, X - 5, Y);
                g.DrawLine(pen, X + Size, Y, X + 5, Y);

                g.DrawLine(pen, X, Y - Size, X, Y - 5);
                g.DrawLine(pen, X, Y + Size, X, Y + 5);

                g.DrawEllipse(pen, X - 5, Y - 5, 10, 10);
            }
        }
        public Rectangle GetBounds()
        {return new Rectangle(X - 5, Y - 5, 10, 10);}
    }
}
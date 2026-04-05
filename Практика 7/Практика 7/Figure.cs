using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;

[Serializable]
public abstract class Figure
{
    public Rectangle Bounds { get; set; } // верхний левый угол + размер
    public Stroke Stroke { get; set; }

    protected Figure(Rectangle bounds, Stroke stroke)
    {
        Bounds = bounds;
        Stroke = stroke ?? new Stroke();
    }

    public abstract void Draw(Graphics g);

    public void Move(int dx, int dy)
    {
        Bounds = new Rectangle(Bounds.X + dx, Bounds.Y + dy, Bounds.Width, Bounds.Height);
    }

    public void MoveTo(Point newLocation)
    {
        Bounds = new Rectangle(newLocation, Bounds.Size);
    }

    public bool IsHit(Point point)
    {
        using (var path = GetPath())
        using (var pen = new Pen(Color.Black, Stroke.Width + 5))
        {
            return path.IsOutlineVisible(point, pen);
        }
    }

    protected abstract GraphicsPath GetPath();

    public virtual void DrawSelectionMarkers(Graphics g)
    {
        SelectionMarkers.DrawMarkers(g, Bounds);
    }
}
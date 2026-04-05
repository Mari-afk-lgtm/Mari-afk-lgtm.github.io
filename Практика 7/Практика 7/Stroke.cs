using System;
using System.Drawing;
using System.Drawing.Drawing2D;

[Serializable]
public class Stroke
{
    public Color Color { get; set; }
    public float Width { get; set; }
    public DashStyle DashStyle { get; set; }

    public Stroke()
    {
        Color = Color.Black;
        Width = 2f;
        DashStyle = DashStyle.Solid;
    }

    public Pen GetPen()
    {
        return new Pen(Color, Width) { DashStyle = DashStyle };
    }
}
using System.Drawing;
using System.Drawing.Drawing2D;

[Serializable]
public class ArcFigure : Figure
{
    public ArcFigure(Rectangle bounds, Stroke stroke) : base(bounds, stroke) { }

    protected override GraphicsPath GetPath()
    {
        var path = new GraphicsPath();
        path.AddArc(Bounds, 0, 180);
        return path;
    }

    public override void Draw(Graphics g)
    {
        using (var pen = Stroke.GetPen())
            g.DrawArc(pen, Bounds, 0, 180);
    }
}
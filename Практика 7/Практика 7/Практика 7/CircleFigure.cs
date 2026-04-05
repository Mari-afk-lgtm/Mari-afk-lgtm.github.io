using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VectorEditor
{
    [Serializable]
    public class CircleFigure : Figure
    {
        public CircleFigure(Rectangle bounds, Stroke stroke) : base(bounds, stroke) { }

        protected override GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            path.AddEllipse(Bounds);
            return path;
        }
        public override void Draw(Graphics g)
        {
            using (var pen = Stroke.GetPen())
                g.DrawEllipse(pen, Bounds);
        }
    }
}
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VectorEditor
{
    [Serializable]
    public class LineFigure : Figure
    {
        public LineFigure(Rectangle bounds, Stroke stroke) : base(bounds, stroke) { }

        protected override GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            path.AddLine(Bounds.Left, Bounds.Top, Bounds.Right, Bounds.Bottom);
            return path;
        }
        public override void Draw(Graphics g)
        {
            using (var pen = Stroke.GetPen())
                g.DrawLine(pen, Bounds.Left, Bounds.Top, Bounds.Right, Bounds.Bottom);
        }
    }
}
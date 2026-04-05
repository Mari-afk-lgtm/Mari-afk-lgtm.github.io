using System.Drawing;

namespace VectorEditor
{
    public static class SelectionMarkers
    {
        private const int MarkerSize = 7;

        public static void DrawMarkers(Graphics g, Rectangle bounds)
        {
            using (var pen = new Pen(Color.Blue, 2))
            using (var brush = new SolidBrush(Color.White))
            {
                Point[] markers =
                {
                    new Point(bounds.Left - MarkerSize/2, bounds.Top - MarkerSize/2),
                    new Point(bounds.Right - MarkerSize/2, bounds.Top - MarkerSize/2),
                    new Point(bounds.Left - MarkerSize/2, bounds.Bottom - MarkerSize/2),
                    new Point(bounds.Right - MarkerSize/2, bounds.Bottom - MarkerSize/2)
                };
                foreach (var p in markers)
                {
                    g.FillRectangle(brush, p.X, p.Y, MarkerSize, MarkerSize);
                    g.DrawRectangle(pen, p.X, p.Y, MarkerSize, MarkerSize);
                }
            }
        }
    }
}
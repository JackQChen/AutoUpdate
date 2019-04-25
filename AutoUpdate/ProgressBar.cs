using AutoUpdate.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace AutoUpdate
{
    public class ProgressBar : Control
    {
        public float Value { get; set; }
        public ProgressBar()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.EnableNotifyMessage
                | ControlStyles.SupportsTransparentBackColor, true);
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            DrawProgress(pevent.Graphics, this.DisplayRectangle, Value);
        }

        private void DrawProgress(Graphics g, Rectangle rect, float percent)
        {
            g.RendererBackground(Resources.Progress_Background, rect, 5);
            if (percent > 0)
            {
                var width = (int)(rect.Width * percent);
                if (width < 8)
                    width = 8;
                var rectProgress = new Rectangle(rect.X, rect.Y, width, rect.Height);
                g.RendererBackground(Resources.Progress_Progress, rectProgress, 5);
            }
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;
            g.DrawString(percent.ToString("p1"), this.Font, Brushes.DimGray, rect, sf);
        }
    }
}

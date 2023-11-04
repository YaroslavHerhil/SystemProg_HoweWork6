using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemProg_HoweWork6
{
    public class NewProgressBar : ProgressBar
    {
        Random random = new Random();
        public NewProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;

            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            rec.Height = rec.Height - 4;
            using (SolidBrush brush = new SolidBrush(ForeColor))
            {
                e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);

            }
        }
        public async Task Dance()
        {
            int direction = 1;
            while (true)
            {

                Value += direction;
                if (Value == Maximum) direction *= -1;
                if (Value == Minimum)
                    direction *= -1;
                await Task.Delay(50);
            }
        }
        
       

        public void RandomColor()
        {
            
            string color = "#";
            string red = random.Next(0, 255).ToString("X");
            if (red.Length < 2) red.Insert(0, "0");
            string green = random.Next(0, 255).ToString("X");
            if (green.Length < 2) green.Insert(0, "0");
            string blue = random.Next(0, 255).ToString("X");
            if (blue.Length < 2) blue.Insert(0, "0");

            color += red + green + blue;

            ForeColor = ColorTranslator.FromHtml(color);
        }
    }
}

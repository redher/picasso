using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicassoComponents
{
    public partial class DrawingBoard : UserControl
    {
        private bool mouseDown = false;
        private Color color = Color.Black;
        private Bitmap bit = new Bitmap(700, 480);
        private Graphics graphics;
        private Point current = new Point();
        private Point old = new Point();
        private Pen pen = new Pen(Color.Black,3);

        public bool EnableEdit { get; set; }

        public DrawingBoard()
        {
            InitializeComponent();
            graphics = panel1.CreateGraphics();
            pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            old = e.Location;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown && EnableEdit)
            {
                //bit = new Bitmap(bit, panel1.Size);
                //panel1.BackgroundImage = bit;
                //bit.SetPixel(e.X, e.Y, color);
                current = e.Location;
                graphics.DrawLine(pen, old, current);
                old = current;
            }
        }

        public Bitmap GetBitmap() {
            return new Bitmap(700, 480, graphics);
        }

        public void SetBitmap(Bitmap bitmap) {
            this.panel1.BackgroundImage = bitmap;
        }

        public void Clean() {
            this.panel1.BackgroundImage = new Bitmap(700, 480);
        }


    }
}

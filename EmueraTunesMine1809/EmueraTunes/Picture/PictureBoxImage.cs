using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MinorShift._Library;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameData;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.GameView;
using MinorShift.Emuera.Forms;

namespace MinorShift.Emuera.Picture
{
    public class PaintBoxImage : System.Windows.Forms.PictureBox
    {


        //Picture描画
        //親コントロール描画
        //protected override void OnPaintBackground(PaintEventArgs pevent)
        //{
        //base.OnPaintBackground(e);
        //    this.DrawParentControl(this.Parent, pevent);
        //}
        private void DrawParentControl(Control c, PaintEventArgs pevent)
        {
            using (Bitmap bmp = new Bitmap(c.Width, c.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    using (PaintEventArgs p = new PaintEventArgs(g, c.ClientRectangle))
                    {
                        this.InvokePaintBackground(c, p);
                        this.InvokePaint(c, p);
                    }
                }
                int offsetX = this.Left + (int)Math.Floor((double)
                    (this.Bounds.Width - this.ClientRectangle.Width) / 2.0);
                int offsetY = this.Top + (int)Math.Floor((double)
                    (this.Bounds.Height - this.ClientRectangle.Height) / 2.0);
                pevent.Graphics.DrawImage(bmp,
                    this.ClientRectangle, new Rectangle(offsetX, offsetY,
                        this.ClientRectangle.Width, this.ClientRectangle.Height), GraphicsUnit.Pixel);
            }
        }

        //背面コントロール描画
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
        {
            this.DrawParentControl(this.Parent, pevent);
            for (int i = this.Parent.Controls.Count - 1; i >= 0;
                i--)
            {
                Control c = this.Parent.Controls[i];
                if (c == this)
                {
                    break;
                }
                if (this.Bounds.IntersectsWith(c.Bounds) == false)
                {
                    continue;
                }
                this.DrawBackControl(c, pevent);
            }
        }

        private void DrawBackControl(Control c, System.Windows.Forms.PaintEventArgs pevent)
        {
            using (Bitmap bmp = new Bitmap(c.Width, c.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                c.DrawToBitmap(bmp, new Rectangle(0, 0, c.Width, c.Height));
                int offsetX = (c.Left - this.Left) -
                    (int)Math.Floor((double)(this.Bounds.Width - this.ClientRectangle.Width) / 2.0);
                int offsetY = (c.Top - this.Top) - (int)Math.Floor
                    ((double)(this.Bounds.Height -
                    this.ClientRectangle.Height) / 2.0);
                pevent.Graphics.DrawImage(bmp, offsetX, offsetY, c.Width, c.Height);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MinorShift.Emuera.Picture
{
    public class CreateRegion
    {
        //Format24bppRgb・Format32bppRgb・Format32bppArgb
        //http://d.hatena.ne.jp/aharisu/20090523/1243077727
        //コードは上記より拝借

        public Region CreateRegionFromBitmap(Bitmap bitmap)
        {
            if (!(bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ||
                bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb ||
                bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb))
                return null;// 24bitか32bit以外は無理

            //ここの変数もっとまともな名前つけたい
            int channels = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            int offset = 0;
            int count = channels;
            if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {//Format32bppArgbの場合はα値のみを確認する
                count = 1;
                offset = 3;
            }

            //ビットマップから生データを取得
            Rectangle rectBitmap = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData data = bitmap.LockBits(rectBitmap,
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
            byte[] values = new byte[data.Height * data.Stride];
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, values, 0, data.Height * data.Stride);
            bitmap.UnlockBits(data);

            //透明にする色を取得
            byte[] transparent = new byte[count];
            if (count == 1)
                transparent[0] = 0;
            else
            {
                for (int i = 0; i < count; ++i)
                    transparent[i] = values[i];
            }

            Region region = new Region();
            region.MakeEmpty();

            Rectangle rect = new Rectangle();
            for (int y = 0; y < data.Height; ++y)
            {
                int offsetY = y * data.Stride;
                for (int x = 0; x < data.Width; ++x)
                {
                    for (int c = 0; c < count; ++c)
                    {
                        if (transparent[c] != values[offsetY + x * channels + c + offset])
                        {//透過色ではない
                            rect.X = x;//透過色ではないスタート地点を保存
                            for (++x; x < data.Width; ++x)
                            {
                                for (c = 0; c < count; ++c)
                                {
                                    if (transparent[c] != values[offsetY + x * channels + c + offset])
                                        goto CONTINUE;	//透過色ではないので続ける
                                }
                                break;	//次の透過色を見つけたので脱出
                            CONTINUE:
                                ;
                            }
                            rect.Width = x - rect.X;
                            rect.Y = y;
                            rect.Height = 1;

                            //透過色でない部分をリージョンに追加する
                            region.Union(rect);
                            break;
                        }
                    }
                }
            }

            return region;
        }
    }
}

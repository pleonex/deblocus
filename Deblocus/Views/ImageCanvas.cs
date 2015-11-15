//
//  ImageCanvas.cs
//
//  Author:
//       Benito Palacios Sánchez (aka pleonex) <benito356@gmail.com>
//
//  Copyright (c) 2015 Benito Palacios Sánchez (c) 2015
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Xwt;
using Xwt.Drawing;

namespace Deblocus.Views
{
    // NOTE: This is not a generic widget. It's very hacked to the current needs.
    public class ImageCanvas : Canvas
    {
        public ImageCanvas(Image img)
        {
            Image = img;
        }

        public Image Image { get; set; }

        protected override void OnDraw(Context ctx, Rectangle dirtyRect)
        {
            base.OnDraw(ctx, dirtyRect);

            if (Image != null) {
                double width = Bounds.Width == 1 ? Image.Width : Bounds.Width;

                // Scale height respect width
                double height = (width / Image.Width) * Image.Height;
                if (HeightRequest != height)
                    HeightRequest = height;

                ctx.DrawImage(Image, 0, 0, width, height);
            }
        }
    }
}


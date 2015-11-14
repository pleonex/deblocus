//
//  CardView.cs
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
    public class CardView : Canvas
    {
        public CardView()
        {
            MinHeight = HeightRequest = 100;
            MinWidth  = WidthRequest  = 300;
        }

        protected override void OnDraw(Context ctx, Rectangle dirtyRect)
        {
            base.OnDraw(ctx, dirtyRect);

            DrawBase(ctx);
        }

        private void DrawBase(Context ctx)
        {
            ctx.Save ();

            const int radius = 30;
            double width  = Bounds.Width;
            double height = Bounds.Height;

            // Draw border arcs
            ctx.SetColor(Colors.LightGray);
            ctx.Arc(radius, radius, radius, 180, 270);                  // Top left
            ctx.Arc(width - radius, radius, radius, 270, 0);            // Top right
            ctx.Arc(width - radius, height - radius, radius, 0, 90);    // Bottom right
            ctx.Arc(radius, height - radius, radius, 90, 180);          // Bottom left

            // Close the paths and fill the rectangle
            ctx.ClosePath();
            ctx.StrokePreserve();
            ctx.Fill();

            ctx.Restore();
        }
    }
}


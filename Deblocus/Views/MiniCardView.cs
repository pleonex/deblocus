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
using Deblocus.Entities;

namespace Deblocus.Views
{
    public class MiniCardView : Canvas
    {
        private const int radius = 30;
        private static readonly Color[] BaseColor = {
            Colors.White,
            Colors.LightGray,
            Colors.LightSalmon,
            Colors.AliceBlue,
            Colors.DarkGreen
        };

        public MiniCardView(Card card)
        {
            Card = card;
            MinHeight = HeightRequest = 100;
            MinWidth  = WidthRequest  = 300;
            CreateComponents();
        }

        public Card Card {
            get; private set;
        }

        public event EventHandler<ButtonEventArgs> Clicked {
            add { this.ButtonReleased += value; }
            remove { this.ButtonReleased -= value; }
        }

        protected override void OnDraw(Context ctx, Rectangle dirtyRect)
        {
            base.OnDraw(ctx, dirtyRect);
            DrawBase(ctx);
        }

        private void DrawBase(Context ctx)
        {
            ctx.Save ();

            double width  = Bounds.Width;
            double height = Bounds.Height;

            // Draw border arcs
            ctx.SetColor(BaseColor[Card.GroupId]);
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

        private void CreateComponents()
        {
            var bounds = new Rectangle(
                20,
                5,
                WidthRequest - 40,
                HeightRequest - 10
            );

            var box = new VBox();

            var lblTitle = new Label {
                Text = Card.Title,
                TooltipText = Card.Title,
                Font = Font.WithWeight(FontWeight.Bold).WithSize(10),
                TextAlignment = Alignment.Center,
                Ellipsize = EllipsizeMode.End,
            };
            box.PackStart(lblTitle);

            var lblDescription = new Label {
                Text = Card.Description,
                TooltipText = Card.Description,
                Font = Font.WithSize(10),
                TextAlignment = Alignment.Start,
                Wrap = WrapMode.Word,

                HeightRequest = bounds.Height - 45,
                VerticalPlacement = WidgetPlacement.Start
            };
            box.PackStart(lblDescription);

            var statusBox = new HBox();
            statusBox.HeightRequest = 16;

            if (Card.Images.Count > 0)
                statusBox.PackEnd(new ImageView { Image = StockIcons.Zoom100 });
            box.PackStart(statusBox);

            AddChild(box, bounds);
        }
    }
}


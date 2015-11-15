//
//  ImageViewDialog.cs
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
    public class ImageViewDialog : Dialog
    {
        private Table table;

        public ImageViewDialog(Image image)
        {
            Decorated = false;
            ShowInTaskbar = false;
            FullScreen = true;
            Padding = new WidgetSpacing();

            table = new Table();
            table.BackgroundColor = Colors.Black;

            Button btnLeft = new Button("<");
            btnLeft.Clicked += (sender, e) => this.Dispose();
            table.Add(btnLeft, 0, 0, vpos: WidgetPlacement.Center);
            table.Add(new ImageCanvas(image), 1, 0, 1, 1, true, true,
                WidgetPlacement.Center, WidgetPlacement.Center);
            table.Add(new Button(">"), 2, 0, vpos: WidgetPlacement.Center);

            Content = table;
        }
    }
}


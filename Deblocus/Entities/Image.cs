//
//  Image.cs
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

namespace Deblocus.Entities
{
    public class Image
    {
        private string name;
        private Xwt.Drawing.Image imageObj;

        public Image()
        {
            Name = DefaultName;
        }

        public static string DefaultName {
            get { return "No Name"; }
        }

        public virtual int Id { get; protected set; }
        public virtual byte[] Data { get; set; }
        public virtual string Name {
            get { return name; }
            set { name = string.IsNullOrEmpty(value) ? DefaultName : value; }
        }
        public virtual string Description { get; set; }
        public virtual Card Card { get; set; }

        public virtual Xwt.Drawing.Image GetImage()
        {
            if (imageObj == null)
                imageObj = Xwt.Drawing.Image.FromStream(new System.IO.MemoryStream(Data));
            return imageObj;
        }
    }
}


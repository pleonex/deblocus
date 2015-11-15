//
//  Card.cs
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
using System.Collections.Generic;

namespace Deblocus.Entities
{
    public class Card
    {
        private string title;

        public Card()
        {
            Title = DefaultTitle;
            CreationDate = DateTime.Now;
            GroupChangeDate = CreationDate;
            Images = new List<Image>();
            TargetPoints = DefaultTargetPoints;
            Visible = true;
        }

        public static string DefaultTitle {
            get { return "No Title"; }
        }

        public static int DefaultTargetPoints {
            get { return 5; }
        }

        public virtual int Id { get; protected set; }
        public virtual string Title {
            get { return title; }
            set { title = string.IsNullOrEmpty(value) ? DefaultTitle : value; }
        }
        public virtual string Description { get; set; }
        public virtual IList<Image> Images { get; protected set; }
        public virtual DateTime CreationDate { get; protected set; }
        public virtual int Points { get; protected set; }
        public virtual int TargetPoints { get; set; }
        public virtual int GroupId { get; protected set; }
        public virtual DateTime GroupChangeDate { get; protected set; }
        public virtual Lesson Lesson { get; set; }
        public virtual bool Visible { get; set; }

        public virtual void AddImage(Image img)
        {
            img.Card = this;
            Images.Add(img);
        }

        public virtual void GivePoint()
        {
            Points++;
            if (Points >= TargetPoints) {
                Points = 0;
                GroupId++;
                GroupChangeDate = DateTime.Now;
            }
        }
    }
}


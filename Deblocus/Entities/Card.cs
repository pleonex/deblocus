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
        public Card()
        {
            Images = new List<Image>();
        }

        public virtual int Id { get; protected set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Image> Images { get; protected set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual int Points { get; set; }
        public virtual int TargetPoints { get; set; }
        public virtual int Group { get; set; }
        public virtual DateTime GroupChangeDate { get; set; }
    }
}


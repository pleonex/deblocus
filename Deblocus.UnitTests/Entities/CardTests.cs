//
//  CardTests.cs
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
using NUnit.Framework;
using Deblocus.Entities;
using System.Collections.Generic;

namespace Deblocus.UnitTests.Entities
{
    [TestFixture]
    public class CardTests
    {
        [Test]
        public void PropertiesExists()
        {
            byte[] img1 = new byte[] { 0x10, 0x20 };

            Card card = new Card {
                Title = "My Card",
                Description = "Description",
            };
            card.Images.Add(img1);

            Assert.AreEqual("My Card", card.Title);
            Assert.AreEqual("Description", card.Description);
            Assert.AreEqual(img1, card.Images[0]);
        }
    }
}


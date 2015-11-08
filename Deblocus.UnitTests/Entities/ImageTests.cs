//
//  ImageTests.cs
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

namespace Deblocus.UnitTests.Entities
{
    [TestFixture]
    public class ImageTests : DatabaseTests
    {
        [Test]
        public void PropertiesExist()
        {
            var imageData = new byte[3] { 0xAA, 0xBB, 0xCC };
            Image image = new Image {
                Name = "An image",
                Description = "This is a\ndescription.",
                Data = imageData
            };

            Assert.AreEqual("An image", image.Name);
            Assert.AreEqual("This is a\ndescription.", image.Description);
            Assert.AreEqual(imageData, image.Data);
        }

        [Test]
        public void DefaultValues()
        {
            Image image = new Image();
            Assert.AreEqual(Image.DefaultName, image.Name);
            Assert.IsNull(image.Description);
            Assert.IsNull(image.Data);
        }

        [Test]
        public void TryToSetNullName()
        {
            Image image = new Image();
            image.Name = null;
            Assert.IsNotNull(image.Name);
            Assert.AreEqual(Image.DefaultName, image.Name);
        }

        [Test]
        public void TryToSetEmptyName()
        {
            Image image = new Image();
            image.Name = string.Empty;
            Assert.IsNotNullOrEmpty(image.Name);
            Assert.AreEqual(Image.DefaultName, image.Name);
        }

        [Test]
        public void CreateInDB()
        {
            var imageData = new byte[3] { 0xAA, 0xBB, 0xCC };
            Image image = new Image {
                Name = "An image",
                Description = "This is a\ndescription.",
                Data = imageData
            };

            SaveOrUpdate(image);

            using (this.Session.BeginTransaction()) {
                var dbImages = Retrieve<Image>();
                Assert.AreEqual(1, dbImages.Count);
                Assert.AreEqual(image.Name, dbImages[0].Name);
                Assert.AreEqual(image.Description, dbImages[0].Description);
                Assert.AreEqual(image.Data, dbImages[0].Data);
            }
        }

        [Test]
        public void DefaultValuesFromDB()
        {
            Image image = new Image();
            SaveOrUpdate(image);

            using (this.Session.BeginTransaction()) {
                var dbImages = Retrieve<Image>();
                Assert.AreEqual(Image.DefaultName, dbImages[0].Name);
                Assert.IsNull(dbImages[0].Description);
                Assert.IsNull(dbImages[0].Data);
            }
        }
    }
}


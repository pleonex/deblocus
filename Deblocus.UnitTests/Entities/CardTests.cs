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

namespace Deblocus.UnitTests.Entities
{
    [TestFixture]
    public class CardTests : DatabaseTests
    {
        [Test]
        public void PropertiesExists()
        {
            DateTime creationTime = DateTime.Now;
            DateTime groupChangeTime = DateTime.Now;

            Card card = new Card {
                Title = "My Card",
                Description = "Description",
                CreationDate = creationTime,
                Points = 2,
                TargetPoints = 3,
                GroupId = 1,
                GroupChangeDate = groupChangeTime
            };

            Assert.AreEqual("My Card", card.Title);
            Assert.AreEqual("Description", card.Description);
            Assert.AreEqual(creationTime, card.CreationDate);
            Assert.AreEqual(2, card.Points);
            Assert.AreEqual(3, card.TargetPoints);
            Assert.AreEqual(1, card.GroupId);
            Assert.AreEqual(groupChangeTime, card.GroupChangeDate);
        }

        [Test]
        public void IntegrationWithImages()
        {
            Image image = new Image { Name = "My Image" };

            Card card = new Card();
            card.Images.Add(image);

            Assert.AreSame(image, card.Images[0]);
        }

        [Test]
        public void CreateCardInDB()
        {
            DateTime creationTime = DateTime.Now;
            DateTime groupChangeTime = DateTime.Now;

            Card card = new Card {
                Title = "My Card",
                Description = "Description",
                CreationDate = creationTime,
                Points = 2,
                TargetPoints = 3,
                GroupId = 1,
                GroupChangeDate = groupChangeTime
            };

            using (var transaction = this.Session.BeginTransaction()) {
                this.Session.SaveOrUpdate(card);
                transaction.Commit();
            }

            using (this.Session.BeginTransaction()) {
                var dbCard = this.Session.CreateCriteria<Card>().List<Card>();
                Assert.AreEqual(1, dbCard.Count);
                Assert.AreEqual(card.Title, dbCard[0].Title);
                Assert.AreEqual(card.Description, dbCard[0].Description);
                Assert.AreEqual(card.CreationDate, dbCard[0].CreationDate);
                Assert.AreEqual(card.Points, dbCard[0].Points);
                Assert.AreEqual(card.TargetPoints, dbCard[0].TargetPoints);
                Assert.AreEqual(card.GroupId, dbCard[0].GroupId);
                Assert.AreEqual(card.GroupChangeDate, dbCard[0].GroupChangeDate);
            }
        }
    }
}


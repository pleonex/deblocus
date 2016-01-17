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
        public void PropertiesExist()
        {
            Card card = new Card {
                Title = "My Card",
                Description = "Description",
                TargetPoints = 3
            };

            Assert.AreEqual("My Card", card.Title);
            Assert.AreEqual("Description", card.Description);
            Assert.IsInstanceOf<DateTime>(card.CreationDate);
            Assert.AreEqual(0, card.Points);
            Assert.AreEqual(3, card.TargetPoints);
            Assert.AreEqual(0, card.GroupId);
            Assert.IsInstanceOf<DateTime>(card.GroupChangeDate);
            Assert.AreEqual(0, card.Images.Count);
            Assert.IsNull(card.Lesson);
            Assert.IsTrue(card.Visible);
        }

        [Test]
        public void IntegrationWithImages()
        {
            Image image = new Image { Name = "My Image" };

            Card card = new Card();
            card.AddImage(image);

            Assert.AreEqual(1, card.Images.Count);
            Assert.AreSame(image, card.Images[0]);
        }

        [Test]
        public void DefaultValues()
        {
            Card card = new Card();
            Assert.AreEqual(Card.DefaultTitle, card.Title);
            Assert.IsNull(card.Description);
            Assert.IsNotNull(card.Images);
            Assert.AreEqual(0, card.Images.Count);
            Assert.AreNotEqual(new DateTime(), card.CreationDate);
            Assert.AreEqual(0, card.Points);
            Assert.AreEqual(Card.DefaultTargetPoints, card.TargetPoints);
            Assert.AreEqual(0, card.GroupId);
            Assert.AreEqual(card.CreationDate, card.GroupChangeDate);
            Assert.IsNull(card.Lesson);
            Assert.IsTrue(card.Visible);
        }

        [Test]
        public void TryToSetNullTitle()
        {
            Card card = new Card();
            card.Title = null;
            Assert.IsNotNull(card.Title);
            Assert.AreEqual(Card.DefaultTitle, card.Title);
        }

        [Test]
        public void TryToSetEmptyTitle()
        {
            Card card = new Card();
            card.Title = string.Empty;
            Assert.IsNotNullOrEmpty(card.Title);
            Assert.AreEqual(Card.DefaultTitle, card.Title);
        }

        [Test]
        public void GivePoint()
        {
            Card card = new Card { TargetPoints = 2 };
            DateTime initialDate = card.GroupChangeDate;

            Assert.AreEqual(0, card.Points);
            Assert.AreEqual(0, card.GroupId);
            Assert.AreEqual(initialDate, card.GroupChangeDate);

            card.GivePoint();
            Assert.AreEqual(1, card.Points);
            Assert.AreEqual(0, card.GroupId);
            Assert.AreEqual(initialDate, card.GroupChangeDate);

            card.GivePoint();
            Assert.AreEqual(0, card.Points);
            Assert.AreEqual(1, card.GroupId);
            Assert.Greater(card.GroupChangeDate, initialDate);
        }

        [Test]
        public void IsCompleted()
        {
            Card card = new Card();
            while (card.GroupId < Card.PointsForComplete) {
                Assert.IsFalse(card.IsComplete());
                card.GivePoint();
            }

            Assert.IsTrue(card.IsComplete());

            card.GivePoint();
            Assert.IsTrue(card.IsComplete());
        }

        [Test]
        public void ResetPoints()
        {
            Card card = new Card();
            Assert.DoesNotThrow(card.ResetPoints);
            Assert.AreEqual(0, card.Points);
            Assert.AreEqual(0, card.GroupId);

            while (card.GroupId == 0)
                card.GivePoint();
            var cardDate = card.GroupChangeDate;

            card.ResetPoints();
            Assert.AreEqual(0, card.Points);
            Assert.AreEqual(0, card.GroupId);
            Assert.AreNotEqual(cardDate, card.GroupChangeDate);
        }

        [Test]
        public void TouchDate()
        {
            Card card = new Card();
            DateTime initialDate = card.GroupChangeDate;

            card.TouchDate();
            Assert.Greater(card.GroupChangeDate, initialDate);
        }

        [Test]
        public void CreateCardInDB()
        {
            Card card = new Card {
                Title = "My Card",
                Description = "Description",
            };

            SaveOrUpdate(card);

            using (this.Session.BeginTransaction()) {
                var dbCard = Retrieve<Card>();
                Assert.AreEqual(1, dbCard.Count);
                Assert.AreEqual(card.Title, dbCard[0].Title);
                Assert.AreEqual(card.Description, dbCard[0].Description);
                Assert.AreEqual(card.CreationDate, dbCard[0].CreationDate);
                Assert.AreEqual(card.Points, dbCard[0].Points);
                Assert.AreEqual(card.TargetPoints, dbCard[0].TargetPoints);
                Assert.AreEqual(card.GroupId, dbCard[0].GroupId);
                Assert.AreEqual(card.GroupChangeDate, dbCard[0].GroupChangeDate);
                Assert.AreEqual(card.Visible, dbCard[0].Visible);
            }
        }

        [Test]
        public void CheckCascadeWithImages()
        {
            Image image = new Image { Name = "The Image" };
            Card card = new Card { Title = "Test Image in DB" };
            card.AddImage(image);

            SaveOrUpdate(card);

            using (this.Session.BeginTransaction()) {
                var dbCards = Retrieve<Card>();
                Assert.AreEqual(1, dbCards[0].Images.Count);
                Assert.AreEqual(image.Name, dbCards[0].Images[0].Name);
            }
        }

        [Test]
        public void DefaultValuesFromDB()
        {
            Card card = new Card();
            SaveOrUpdate(card);

            using (this.Session.BeginTransaction()) {
                var dbCards = Retrieve<Card>();
                Assert.AreEqual(Card.DefaultTitle, dbCards[0].Title);
                Assert.IsNull(dbCards[0].Description);
                Assert.IsNotNull(dbCards[0].Images);
                Assert.AreEqual(0, dbCards[0].Images.Count);
                Assert.AreNotEqual(new DateTime(), dbCards[0].CreationDate);
                Assert.AreEqual(0, dbCards[0].Points);
                Assert.AreEqual(Card.DefaultTargetPoints, dbCards[0].TargetPoints);
                Assert.AreEqual(0, dbCards[0].GroupId);
                Assert.AreEqual(dbCards[0].CreationDate, dbCards[0].GroupChangeDate);
                Assert.IsNull(dbCards[0].Lesson);
                Assert.IsTrue(dbCards[0].Visible);
            }
        }
    }
}


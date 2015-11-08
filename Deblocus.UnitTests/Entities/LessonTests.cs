//
//  LessonTests.cs
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
    public class LessonTests : DatabaseTests
    {
        [Test]
        public void PropertiesExist()
        {
            Lesson lesson = new Lesson {
                Title = "A title"
            };

            Assert.AreEqual("A title", lesson.Title);
            Assert.AreEqual(0, lesson.Cards.Count);
            Assert.IsNull(lesson.Subject);
        }

        [Test]
        public void IntegrationWithCards()
        {
            Lesson lesson = new Lesson();
            Card card = new Card { Title = "Card title" };
            lesson.AddCard(card);

            Assert.AreEqual(1, lesson.Cards.Count);
            Assert.AreSame(card, lesson.Cards[0]);
            Assert.AreSame(lesson, card.Lesson);
        }

        [Test]
        public void DefaultValues()
        {
            Lesson lesson = new Lesson();
            Assert.AreEqual(Lesson.DefaultTitle, lesson.Title);
            Assert.IsNotNull(lesson.Cards);
            Assert.AreEqual(0, lesson.Cards.Count);
            Assert.IsNull(lesson.Subject);
        }

        [Test]
        public void TryToSetNullTitle()
        {
            Lesson lesson = new Lesson();
            lesson.Title = null;
            Assert.IsNotNull(lesson.Title);
            Assert.AreEqual(Lesson.DefaultTitle, lesson.Title);
        }

        [Test]
        public void TryToSetEmptyTitle()
        {
            Lesson lesson = new Lesson();
            lesson.Title = string.Empty;
            Assert.IsNotNullOrEmpty(lesson.Title);
            Assert.AreEqual(Lesson.DefaultTitle, lesson.Title);
        }

        [Test]
        public void CreateLessonInDB()
        {
            Lesson lesson = new Lesson { Title = "The title" };
            SaveOrUpdate(lesson);

            using (this.Session.BeginTransaction()) {
                var dbLessons = Retrieve<Lesson>();
                Assert.AreEqual(1, dbLessons.Count);
                Assert.AreEqual(lesson.Title, dbLessons[0].Title);
            }
        }

        [Test]
        public void CheckCascadeWithCards()
        {
            Lesson lesson = new Lesson { Title = "The title" };
            Card card = new Card { Title = "Card title" };
            lesson.AddCard(card);

            SaveOrUpdate(lesson);

            using (this.Session.BeginTransaction()) {
                var dbLessons = Retrieve<Lesson>();
                Assert.AreEqual(1, dbLessons[0].Cards.Count);
                Assert.AreEqual(card.Title, dbLessons[0].Cards[0].Title);
            }
        }

        [Test]
        public void DefaultValuesFromDB()
        {
            Lesson lesson = new Lesson();
            SaveOrUpdate(lesson);

            using (this.Session.BeginTransaction()) {
                var dbLessons = Retrieve<Lesson>();
                Assert.AreEqual(Lesson.DefaultTitle, dbLessons[0].Title);
                Assert.IsNotNull(dbLessons[0].Cards);
                Assert.AreEqual(0, dbLessons[0].Cards.Count);
                Assert.IsNull(dbLessons[0].Subject);
            }
        }
    }
}


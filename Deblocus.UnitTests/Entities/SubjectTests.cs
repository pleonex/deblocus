//
//  SubjectTests.cs
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
    public class SubjectTests : DatabaseTests
    {
        [Test]
        public void PropertiesExist()
        {
            Subject subject = new Subject { Title = "A subject" };
            Assert.AreEqual("A subject", subject.Title);
            Assert.AreEqual(0, subject.Lessons.Count);
        }

        [Test]
        public void IntegrationWithLessons()
        {
            Subject subject = new Subject();
            Lesson lesson = new Lesson { Title = "Lesson title" };
            subject.AddLesson(lesson);

            Assert.AreEqual(1, subject.Lessons.Count);
            Assert.AreSame(lesson, subject.Lessons[0]);
            Assert.AreSame(subject, lesson.Subject);
        }

        [Test]
        public void DefaultValues()
        {
            Subject subject = new Subject();
            Assert.AreEqual(Subject.DefaultTitle, subject.Title);
            Assert.IsNotNull(subject.Lessons);
            Assert.AreEqual(0, subject.Lessons.Count);
        }

        [Test]
        public void TryToSetNullTitle()
        {
            Subject subject = new Subject();
            subject.Title = null;
            Assert.IsNotNull(subject.Title);
            Assert.AreEqual(Subject.DefaultTitle, subject.Title);
        }

        [Test]
        public void TryToSetEmptyTitle()
        {
            Subject subject = new Subject();
            subject.Title = string.Empty;
            Assert.IsNotNullOrEmpty(subject.Title);
            Assert.AreEqual(Subject.DefaultTitle, subject.Title);
        }

        [Test]
        public void CreateSubjectInDB()
        {
            Subject subject = new Subject { Title = "The title" };
            SaveOrUpdate(subject);

            using (this.Session.BeginTransaction()) {
                var dbSubjects = Retrieve<Subject>();
                Assert.AreEqual(1, dbSubjects.Count);
                Assert.AreEqual(subject.Title, dbSubjects[0].Title);
            }
        }

        [Test]
        public void CheckCascadeWithLessons()
        {
            Subject subject = new Subject { Title = "The title" };
            Lesson lesson = new Lesson { Title = "Card title" };
            subject.AddLesson(lesson);

            SaveOrUpdate(subject);

            using (this.Session.BeginTransaction()) {
                var dbSubjects = Retrieve<Subject>();
                Assert.AreEqual(1, dbSubjects[0].Lessons.Count);
                Assert.AreEqual(lesson.Title, dbSubjects[0].Lessons[0].Title);
            }
        }

        [Test]
        public void DefaultValuesFromDB()
        {
            Subject subject = new Subject();
            SaveOrUpdate(subject);

            using (this.Session.BeginTransaction()) {
                var dbSubjects = Retrieve<Subject>();
                Assert.AreEqual(Subject.DefaultTitle, dbSubjects[0].Title);
                Assert.IsNotNull(dbSubjects[0].Lessons);
                Assert.AreEqual(0, dbSubjects[0].Lessons.Count);
            }
        }
    }
}


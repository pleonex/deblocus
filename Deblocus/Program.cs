//
//  Program.cs
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
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Deblocus.Entities;

namespace Deblocus
{
    public class Program
    {
        public static void Main()
        {
            // Let's create a subject
            Subject mathSubject = new Subject { Title = "Maths" };

            // Now let's add some lessons
            Lesson vector2DLesson = new Lesson { Title = "2D Vectors" };
            Lesson matrixLesson = new Lesson { Title = "Matrix" };
            mathSubject.AddLesson(vector2DLesson);
            mathSubject.AddLesson(matrixLesson);

            // Now some cards to the vector lesson
            Card vectorDefinition = new Card {
                Title = "Vector Definition",
                Description = "The definition of a\nvector is blahblah.",
                TargetPoints = 5
            };
            Card dotProduct = new Card {
                Title = "Dot Product",
                Description = "The dot product is computed by...",
                TargetPoints = 5
            };
            vector2DLesson.AddCard(vectorDefinition);
            vector2DLesson.AddCard(dotProduct);

            // And another for the matrix lesson
            Card matrixSum = new Card {
                Title = "Matrix Sum",
                Description = "The sum of matrix is equals to...",
                TargetPoints = 5
            };
            matrixLesson.AddCard(matrixSum);

            // Save the subject - It will cascade the elements
            DatabaseManager.Instance.SaveOrUpdate(mathSubject);

            // Retrieve all the subjects and print the elements
            var subjects = DatabaseManager.Instance.Retrieve<Subject>();
            foreach (var subject in subjects)
                PrintSubject(subject);

            Console.WriteLine("Done!");
        }

        private static void PrintSubject(Subject subject)
        {
            Console.WriteLine("Subject: {0} {1}", subject.Id, subject.Title);
            foreach (var lesson in subject.Lessons) {
                Console.WriteLine(
                    "\tLesson: {0} {1} {2}",
                    lesson.Id,
                    lesson.Subject == subject,
                    lesson.Title);

                foreach (var card in lesson.Cards) {
                    Console.WriteLine(
                        "\t\tCard: {0} {1} {2}",
                        card.Id,
                        card.Lesson == lesson,
                        card.Title);
                    Console.WriteLine("\t\t\tCreationDate: {0}", card.CreationDate);
                    Console.WriteLine("\t\t\tGroupChangeDate: {0}", card.GroupChangeDate);
                    Console.WriteLine("\t\t\tGroup: {0}", card.GroupId);
                    Console.WriteLine("\t\t\tPoints: {0}", card.Points);
                    Console.WriteLine("\t\t\tTargetPoints: {0}", card.TargetPoints);
                    Console.WriteLine("\t\t\t{0}", card.Description);
                }
            }
        }
    }
}


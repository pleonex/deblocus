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
using NHibernate;
using Deblocus.Entities;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using System.IO;
using NHibernate.Tool.hbm2ddl;

namespace Deblocus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sessionFactory = CreateSessionFactory();

            using (var session = sessionFactory.OpenSession()) {
                using (var transaction = session.BeginTransaction()) {
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
                        CreationDate = DateTime.Now,
                        GroupChangeDate = DateTime.Now,
                        Group = 0,
                        Points = 0,
                        TargetPoints = 5
                    };
                    Card dotProduct = new Card {
                        Title = "Dot Product",
                        Description = "The dot product is computed by...",
                        CreationDate = DateTime.Now,
                        GroupChangeDate = DateTime.Now,
                        Group = 0,
                        Points = 0,
                        TargetPoints = 5
                    };
                    vector2DLesson.AddCard(vectorDefinition);
                    vector2DLesson.AddCard(dotProduct);

                    // And another for the matrix lesson
                    Card matrixSum = new Card {
                        Title = "Matrix Sum",
                        Description = "The sum of matrix is equals to...",
                        CreationDate = DateTime.Now,
                        GroupChangeDate = DateTime.Now,
                        Group = 0,
                        Points = 0,
                        TargetPoints = 5
                    };
                    matrixLesson.AddCard(matrixSum);

                    // Save the subject - It will cascade the elements
                    session.SaveOrUpdate(mathSubject);

                    // Commit the operation
                    transaction.Commit();
                }

                // Retrieve all the subjects and print the elements
                using (session.BeginTransaction()) {
                    var subjects = session.CreateCriteria<Subject>().List<Subject>();
                    foreach (var subject in subjects) {
                        PrintSubject(subject);
                    }
                }

                Console.WriteLine("Closing session...");
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(MonoSQLiteConfiguration.Standard.UsingFile("database.db"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            // Delete the existing DB on each run
            if (File.Exists("database.db"))
                File.Delete("database.db");

            // This NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it.
            new SchemaExport(config).Create(false, true);
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
                    Console.WriteLine("\t\t\tGroup: {0}", card.Group);
                    Console.WriteLine("\t\t\tPoints: {0}", card.Points);
                    Console.WriteLine("\t\t\tTargetPoints: {0}", card.TargetPoints);
                    Console.WriteLine("\t\t\t{0}", card.Description);
                }
            }
        }
    }

    public class MonoSQLiteDriver : NHibernate.Driver.ReflectionBasedDriver  
    {  
        public MonoSQLiteDriver() 
            : base(
                "Mono.Data.Sqlite",
                "Mono.Data.Sqlite",  
                "Mono.Data.Sqlite.SqliteConnection",  
                "Mono.Data.Sqlite.SqliteCommand")  
        {  
        }  

        public override bool UseNamedPrefixInParameter {  
            get {  
                return true;  
            }  
        }  

        public override bool UseNamedPrefixInSql {  
            get {  
                return true;  
            }  
        }  

        public override string NamedPrefix {  
            get {  
                return "@";  
            }  
        }  

        public override bool SupportsMultipleOpenReaders {  
            get {  
                return false;  
            }  
        }  
    }  

    public class MonoSQLiteConfiguration : PersistenceConfiguration<MonoSQLiteConfiguration>
    {
        public static MonoSQLiteConfiguration Standard
        {
            get { return new MonoSQLiteConfiguration(); }
        }

        public MonoSQLiteConfiguration()
        {
            Driver<MonoSQLiteDriver>();
            Dialect<NHibernate.Dialect.SQLiteDialect>();
            Raw("query.substitutions", "true=1;false=0");
        }

        public MonoSQLiteConfiguration InMemory()
        {
            Raw("connection.release_mode", "on_close");
            return ConnectionString(c => c
                .Is("Data Source=:memory:;Version=3;New=True;"));

        }

        public MonoSQLiteConfiguration UsingFile(string fileName)
        {
            return ConnectionString(c => c
                .Is(string.Format("Data Source={0};Version=3;New=True;", fileName)));
        }

        public MonoSQLiteConfiguration UsingFileWithPassword(string fileName, string password)
        {
            return ConnectionString(c => c
                .Is(string.Format("Data Source={0};Version=3;New=True;Password={1};", fileName, password)));
        }
    }
}


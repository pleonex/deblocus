//
//  DatabaseTests.cs
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
using NUnit.Framework;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;

namespace Deblocus.UnitTests
{
    // From: http://www.tigraine.at/2009/05/29/fluent-nhibernate-gotchas-when-testing-with-an-in-memory-database/
    public abstract class DatabaseTests
    {
        private ISessionFactory sessionFactory;
        private Configuration sessionConfiguration;
        private ISession session;

        protected ISession Session {
            get { return session; }
        }

        /// <summary>
        /// Sets up the card tests by creating a DB in memory.
        /// </summary>
        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            sessionFactory = Fluently.Configure()
                .Database(MonoSQLiteConfiguration.Standard.InMemory())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DatabaseManager>())
                .ExposeConfiguration(cfg => sessionConfiguration = cfg)
                .BuildSessionFactory();
        }

        /// <summary>
        /// Sets up a test by stating a DB session and re-creating the schema since
        /// as the DB is delete, it lost the schemas.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            session = sessionFactory.OpenSession();
            var export = new SchemaExport(sessionConfiguration);
            export.Execute(false, true, false, session.Connection, null);
        }

        /// <summary>
        /// Tears down a test by closing the session.
        /// This will delete the in-memory DB.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            session.Dispose();
        }

        /// <summary>
        /// Tears down fixture by disposing the session factory.
        /// </summary>
        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            sessionFactory.Dispose();
        }

        protected void SaveOrUpdate(params object[] objs)
        {
            using (var transaction = this.Session.BeginTransaction()) {
                foreach (object obj in objs)
                    this.Session.SaveOrUpdate(obj);
                transaction.Commit();
            }
        }

        protected IList<T> Retrieve<T>()
            where T : class
        {
            return this.Session.CreateCriteria<T>().List<T>();
        }
    }
}


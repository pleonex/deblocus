//
//  DatabaseManager.cs
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
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Deblocus
{
    public sealed class DatabaseManager : IDisposable
    {
        private static volatile DatabaseManager instance;
        private static readonly object syncRoot = new object();

        private readonly ISessionFactory sessionFactory;
        private readonly ISession session;

        private DatabaseManager()
        {
            sessionFactory = CreateSessionFactory();
            session = sessionFactory.OpenSession();
        }

        ~DatabaseManager()
        {
            Dispose(false);
        }

        public static DatabaseManager Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null)
                            instance = new DatabaseManager();
                    }
                }

                return instance;
            }
        }

        public static string DatabasePath {
            get { return "database.db"; }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(MonoSQLiteConfiguration.Standard.UsingFile(DatabasePath))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DatabaseManager>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool freeManagedResourcesAlso)
        {
            if (freeManagedResourcesAlso) {
                session.Dispose();
                sessionFactory.Dispose();
            }
        }

        public void MakeTransaction(Action<ISession> expression)
        {
            using (var transaction = BeginTransaction()) {
                expression(session);
                transaction.Commit();
            }
        }

        public void SaveOrUpdate(params object[] objects)
        {
            MakeTransaction(aSession => {
                foreach (var obj in objects)
                    aSession.SaveOrUpdate(obj);
            });
        }

        public IList<T> Retrieve<T>()
            where T : class
        {
            IList<T> list = null;
            MakeTransaction(aSession => list = aSession.CreateCriteria<T>().List<T>());
            return list;
        }

        private ITransaction BeginTransaction()
        {
            return session.BeginTransaction();
        }
    }
}


//
//  MonoSQLiteConfiguration.cs
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
using FluentNHibernate.Cfg.Db;

namespace Deblocus
{
    // From: http://stackoverflow.com/questions/7626251/using-nhibernate-and-mono-data-sqlite
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


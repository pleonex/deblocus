//
//  CardMap.cs
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
using FluentNHibernate.Mapping;
using Deblocus.Entities;

namespace Deblocus.Mappings
{
    public class CardMap : ClassMap<Card>
    {
        public CardMap()
        {
            Id(x => x.Id);
            Map(x => x.Title);
            Map(x => x.Description);
            HasMany(x => x.Images)
                .Inverse()
                .Cascade.All();
            Map(x => x.CreationDate);
            Map(x => x.Points);
            Map(x => x.TargetPoints);
            Map(x => x.GroupId);
            Map(x => x.GroupChangeDate);
            Map(x => x.Visible);
            References(x => x.Lesson);
        }
    }
}


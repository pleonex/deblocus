//
//  CardsController.cs
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
using Xwt;
using Deblocus.Entities;

namespace Deblocus.Controllers
{
    public class CardsController
    {
        public CardsController(Window window, Table panel,
            LessonsController lessonsController)
        {
            Window = window;
            Panel = panel;

            lessonsController.LessonChange += OnLessonChanged;
        }

        public Window Window { get; private set; }
        public Table Panel { get; private set; }
        public Lesson CurrentLesson { get; private set; }

        private void OnLessonChanged(Lesson lesson)
        {

        }
    }
}


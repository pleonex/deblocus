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
using Deblocus.Views;

namespace Deblocus.Controllers
{
    public class CardsController
    {
        private const int MaxColumns = 4;

        public CardsController(Table panel, LessonsController lessonsController)
        {
            Panel = panel;
            lessonsController.LessonChange += OnLessonChanged;
        }

        public Table Panel { get; private set; }
        public Lesson CurrentLesson { get; private set; }

        private void OnLessonChanged(Lesson lesson)
        {
            CurrentLesson = lesson;
            Panel.Clear();

            int x = 0;
            int y = 0;
            foreach (Card card in lesson.Cards) {
                Panel.Add(new CardView(card), x, y);

                x++;
                if (x >= MaxColumns) {
                    x = 0;
                    y++;
                }
            }
        }
    }
}


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
using System.Linq;
using Xwt;
using Deblocus.Entities;
using Deblocus.Views;

namespace Deblocus.Controllers
{
    public class CardsController
    {
        private const int MaxColumns = 3;
        private int row;
        private int column;

        public CardsController(Table panel, Button btnAddCard,
            MiniCardContextMenu cardMenu, CoursesController lessonsController)
        {
            Panel = panel;
            lessonsController.LessonChange += OnLessonChanged;

            ButtonAddCard = btnAddCard;
            btnAddCard.Sensitive = false;
            btnAddCard.Clicked += CreateCard;

            ContextMenu = cardMenu;
            cardMenu.ContentUpdate += (sender) => {
                DatabaseManager.Instance.SaveOrUpdate(CurrentLesson);
                UpdateView();
            };
        }

        public Table Panel { get; private set; }
        public Button ButtonAddCard { get; private set; }
        public Lesson CurrentLesson { get; private set; }
        public MiniCardContextMenu ContextMenu { get; private set; }

        public void UpdateView()
        {
            Panel.Clear();
            row = 0;
            column = 0;

            if (CurrentLesson != null) {
                foreach (var cardGroup in CurrentLesson.Cards
                        .GroupBy(c => c.GroupId)
                        .OrderBy(g => g.Key)
                        .Where(g => g.Key != 4))
                    foreach (var card in cardGroup
                            .OrderBy(c => c.GroupChangeDate)
                            .Where(c => c.Visible))
                        AddCard(card);
            }
        }

        private void AddCard(Card card)
        {
            var miniCard = new MiniCardView(card);
            miniCard.Clicked += OnCardClicked;
            Panel.Add(miniCard, column, row);

            ContextMenu.Attach(miniCard);

            column++;
            if (column >= MaxColumns) {
                column = 0;
                row++;
            }
        }

        private void OnLessonChanged(Lesson lesson)
        {
            ButtonAddCard.Sensitive = (lesson != null);
            CurrentLesson = lesson;
            UpdateView();
        }

        private void CreateCard(object sender, EventArgs e)
        {
            Card card = new Card();
            CurrentLesson.AddCard(card);
            DatabaseManager.Instance.SaveOrUpdate(CurrentLesson);

            UpdateView();
        }

        private void OnCardClicked(dynamic sender, ButtonEventArgs args)
        {
            if (args.Button == PointerButton.Left)
                new CardViewController(sender.ParentWindow, sender.Card);
        }
    }
}


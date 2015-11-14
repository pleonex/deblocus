//
//  CardView.cs
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
using Xwt.Drawing;

namespace Deblocus.Views
{
    public class CardView : VBox
    {
        public CardView(Deblocus.Entities.Card card)
        {
            Card = card;
            CreateComponents();
        }

        public Deblocus.Entities.Card Card { get; private set; }

        private void CreateComponents()
        {
            BackgroundColor = Colors.White;

            var lblTitle = new Label {
                Text = Card.Title,
                Font = Font.WithWeight(FontWeight.Bold).WithSize(20),
                TextAlignment = Alignment.Center,
                Wrap = WrapMode.Word
            };
            PackStart(lblTitle);

            var btnReturn = new Button("Return");
            btnReturn.Clicked += delegate { ((MainWindow)ParentWindow).RestoreContent();};
            PackEnd(btnReturn);
        }
    }
}


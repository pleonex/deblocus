//
//  MiniCardContextMenu.cs
//
//  Author:
//       Benito Palacios Sánchez (aka pleonex) <benito356@gmail.com>
//
//  Copyright (c) 2016 Benito Palacios Sánchez
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
using System.Linq.Expressions;
using Xwt.Backends;

namespace Deblocus.Views
{
    public delegate void ContextMenuHandle(object sender);

    public class MiniCardContextMenu
    {
        private MiniCardView activeCard;

        private readonly Menu menu;
        private readonly MenuItem hideItem;
        private readonly MenuItem showItem;

        public MiniCardContextMenu()
        {
            menu = new Menu();

            var resetPoints = new MenuItem("Reset points");
            resetPoints.Clicked += (sender, e) => 
                ItemEvent(activeCard.Card.ResetPoints);
            menu.Items.Add(resetPoints);

            hideItem = new MenuItem("Hide Card");
            hideItem.Clicked += (sender, e) => 
                ItemEvent(() => activeCard.Card.Visible = false);
            menu.Items.Add(hideItem);

            showItem = new MenuItem("Show Card");
            showItem.Clicked += (sender, e) =>
                ItemEvent(() => activeCard.Card.Visible = true);
            menu.Items.Add(showItem);
        }

        public event ContextMenuHandle ContentUpdate;

        public void Attach(MiniCardView miniCard)
        {
            miniCard.ButtonReleased += delegate(object sender, ButtonEventArgs e) {
                if (e.Button == PointerButton.Right)
                    this.PopUp(miniCard);
            };
        }

        public void PopUp(MiniCardView miniCard)
        {
            activeCard = miniCard;

            hideItem.Visible = miniCard.Card.Visible;
            showItem.Visible = !miniCard.Card.Visible;

            menu.Popup();
        }

        private void ItemEvent(Action expr)
        {
            expr.Invoke();
            ContentUpdate?.Invoke(this);
        }
    }
}


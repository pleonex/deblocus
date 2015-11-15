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
        private Button btnPass;
        private Button btnFail;
        private Button btnReturn;
        private Button btnEdit;

        public CardView(Deblocus.Entities.Card card)
        {
            Card = card;
            CreateComponents();
        }

        public Deblocus.Entities.Card Card { get; private set; }

        public event EventHandler ButtonPassClicked {
            add { btnPass.Clicked += value; }
            remove { btnPass.Clicked -= value; }
        }

        public event EventHandler ButtonFailClicked {
            add { btnFail.Clicked += value; }
            remove { btnFail.Clicked -= value; }
        }

        public event EventHandler ButtonReturnClicked {
            add { btnReturn.Clicked += value; }
            remove { btnReturn.Clicked -= value; }
        }

        public event EventHandler ButtonEditClicked {
            add { btnEdit.Clicked += value; }
            remove { btnEdit.Clicked -= value; }
        }

        private void CreateComponents()
        {
            BackgroundColor = Colors.White;

            PackStart(CreateTopBar());

            var bodyBox = new HPaned();
            bodyBox.Panel1.Content = CreateDescriptionView();
            bodyBox.Panel2.Content = CreateImagesView();
            PackStart(bodyBox, true);

            PackEnd(CreateButtonsBar());
        }

        private Widget CreateTopBar()
        {
            var topBox = new HBox();

            var lblTitle = new Label {
                Text = Card.Title,
                Font = Font.WithWeight(FontWeight.Bold).WithSize(20),
                TextAlignment = Alignment.Center,
                Wrap = WrapMode.Word };
            topBox.PackStart(lblTitle, true);

            btnEdit = new Button(StockIcons.Warning);
            topBox.PackEnd(btnEdit);

            return topBox;
        }

        private Widget CreateDescriptionView()
        {
            var mkDescription = new MarkdownView {
                Markdown = Card.Description ?? "",
            };

            return new ScrollView(mkDescription);
        }

        private Widget CreateImagesView()
        {
            var imagesBox = new VBox();

            foreach (var img in Card.Images)
                imagesBox.PackStart(new ImageView(img.GetImage()));

            if (Card.Images.Count == 0)
                imagesBox.PackStart(new Label("No Images") { Font = Font.WithSize(15) });

            return new ScrollView(imagesBox);
        }

        private Widget CreateButtonsBar()
        {
            var bar = new Table();
            const int colspan = 3;

            btnPass = new Button("Got it!");
            bar.Add(btnPass, 0, 0, colspan: colspan, hexpand: true, vexpand: true);

            btnFail = new Button("Fail");
            bar.Add(btnFail, colspan, 0, colspan: colspan, hexpand: true, vexpand: true);

            btnReturn = new Button("Return");
            bar.Add(btnReturn, colspan * 2, 0, vexpand: true);

            bar.HeightRequest = 50;
            return bar;
        }
    }
}


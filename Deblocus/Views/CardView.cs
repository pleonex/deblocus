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
using System.Linq;
using Xwt;
using Xwt.Drawing;

namespace Deblocus.Views
{
    public class CardView : VBox
    {
        private Button btnPass;
        private Button btnFail;
        private Button btnReturn;
        private ToggleButton btnEdit;
        private Button btnAddImage;

        private Label lblTitle;
        private TextEntry txtTitle;
        private MarkdownView lblDescription;
        private TextArea txtDescription;
        private VBox imagesBox;

        public CardView(Deblocus.Entities.Card card)
        {
            Card = card;
            CreateComponents();
            UpdateView();
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

        public event EventHandler ButtonEditToggled {
            add { btnEdit.Toggled += value; }
            remove { btnEdit.Toggled -= value; }
        }

        public event EventHandler ButtonAddImageClicked {
            add { btnAddImage.Clicked += value; }
            remove { btnAddImage.Clicked -= value; }
        }

        public event EventHandler<ButtonEventArgs> ImageClicked {
            add
            {
                foreach (var imgCanvas in imagesBox.Children.OfType<ImageCanvas>())
                    imgCanvas.ButtonReleased += value;
            }

            remove
            {
                foreach (var imgCanvas in imagesBox.Children.OfType<ImageCanvas>())
                    imgCanvas.ButtonReleased -= value;
            }
        }

        public void UpdateView()
        {
            bool isEditMode = btnEdit.Active;

            // Save changed data
            if (!isEditMode) {
                Card.Title = txtTitle.Text;
                lblTitle.Text = Card.Title;

                Card.Description = txtDescription.Text;
                lblDescription.Markdown = Card.Description;

                DatabaseManager.Instance.SaveOrUpdate(Card);
            }

            // Toggle visibility
            lblTitle.Visible = !isEditMode;
            txtTitle.Visible = isEditMode;

            lblDescription.Visible = !isEditMode;
            txtDescription.Visible = isEditMode;

            btnAddImage.Visible = isEditMode;
            UpdateImagesView();

            btnPass.Sensitive = !isEditMode;
            btnFail.Sensitive = !isEditMode;
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

            lblTitle = new Label {
                Text = Card.Title,
                Font = Font.WithWeight(FontWeight.Bold).WithSize(20),
                TextAlignment = Alignment.Center,
                Wrap = WrapMode.Word,
                Visible = false
            };
            topBox.PackStart(lblTitle, true);

            txtTitle = new TextEntry {
                Text = Card.Title,
                Font = lblTitle.Font,
                TextAlignment = Alignment.Center,
                Visible = false
            };
            topBox.PackStart(txtTitle, true);

            btnEdit = new ToggleButton(StockIcons.Warning);
            topBox.PackEnd(btnEdit);

            return topBox;
        }

        private Widget CreateDescriptionView()
        {
            var virtualBox = new VBox();

            lblDescription = new MarkdownView {
                Markdown = Card.Description ?? "",
                Visible = false
            };
            virtualBox.PackStart(lblDescription, true);

            txtDescription = new TextArea {
                Text = Card.Description ?? "",
                Visible = false
            };
            virtualBox.PackStart(txtDescription, true);

            return new ScrollView(virtualBox);
        }

        private Widget CreateImagesView()
        {
            imagesBox = new VBox();

            btnAddImage = new Button(StockIcons.Add, "Add image");
            btnAddImage.Visible = false;

            UpdateImagesView();

            return new ScrollView(imagesBox) {
                HorizontalScrollPolicy = ScrollPolicy.Never,
                VerticalScrollPolicy   = ScrollPolicy.Always
            };
        }

        private void UpdateImagesView()
        {           
            imagesBox.Clear();

            imagesBox.PackStart(btnAddImage);

            foreach (var img in Card.Images)
                imagesBox.PackStart(new ImageCanvas(img.GetImage()), false);

            if (Card.Images.Count == 0)
                imagesBox.PackStart(new Label("No Images") { Font = Font.WithSize(15) });
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


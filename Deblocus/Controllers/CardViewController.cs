﻿//
//  CardViewerController.cs
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
using System.IO;
using Deblocus.Views;
using Xwt;

namespace Deblocus.Controllers
{
    public class CardViewController
    {
        public CardViewController(MainWindow window, Deblocus.Entities.Card card)
        {
            Card = card;

            View = new CardView(card);
            View.ButtonReturnClicked += ButtonReturnClicked;
            View.ButtonFailClicked   += ButtonFailClicked;
            View.ButtonPassClicked   += ButtonPassClicked;
            View.ButtonEditToggled   += ButtonEditToggled;
            View.ButtonAddImageClicked += ButtonAddImageClicked;
            View.ButtonAddImageFromClipboardClicked += ButtonAddImageFromClipboardClicked;
            View.ImageClicked += ImageClicked;

            Window = window;
            Window.ChangeContent(View);
        }

        public Deblocus.Entities.Card Card { get; private set; }
        public CardView View { get; private set; }
        public MainWindow Window { get; private set; }

        private void ButtonPassClicked(object sender, EventArgs e)
        {
            Card.GivePoint();
            DatabaseManager.Instance.SaveOrUpdate(Card);
            Window.RestoreContent();
        }

        private void ButtonFailClicked(object sender, EventArgs e)
        {
            Card.TouchDate();
            DatabaseManager.Instance.SaveOrUpdate(Card);
            Window.RestoreContent();
        }

        private void ButtonReturnClicked(object sender, EventArgs e)
        {
            // Warn user becasuse it's trying to return without saving changes.
            if (View.IsEditMode) {
                var result = MessageDialog.AskQuestion(
                    "Changes not saved!",
                    "Are you sure you want to return while you are in edit mode?\n" +
                    "The changes won't be saved!",
                    Command.Yes, Command.No);
                if (result != Command.Yes)
                    return;
            }

            Window.RestoreContent();
        }

        private void ButtonEditToggled(object sender, EventArgs e)
        {
            View.UpdateView();
        }

        private void ButtonAddImageClicked(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog {
                Title = "Select an image",
                Multiselect = true,

            };
            dialog.Filters.Add(new FileDialogFilter("Image files",
                "*.png", "*.bmp", "*.jpeg", "*.jpg"));

            if (!dialog.Run(View.ParentWindow))
                return;

            foreach (var filename in dialog.FileNames) {
                Card.AddImage(new Deblocus.Entities.Image {
                    Data = System.IO.File.ReadAllBytes(filename)
                });
            }

            View.UpdateView();
        }

        private void ButtonAddImageFromClipboardClicked(object sender, EventArgs e)
        {
            
            if (!Clipboard.ContainsImage())
                return;

            // Get the image from the clipboard and save to a temp buffer.
            var image = Clipboard.GetImage();
            var imageData = new MemoryStream();
            image.Save(imageData, Xwt.Drawing.ImageFileType.Png);

            Card.AddImage(new Deblocus.Entities.Image {
                Data = imageData.ToArray()
            });
            imageData.Dispose();

            View.UpdateView();
        }
            
        private void ImageClicked(dynamic sender, ButtonEventArgs e)
        {
            new ImageViewDialog(sender.Image).Run(sender.ParentWindow);
        }
    }
}


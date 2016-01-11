//
//  MainWindow.cs
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
using System.Reflection;
using Deblocus.Controllers;
using Xwt;
using Xwt.Drawing;

namespace Deblocus.Views
{
    public class MainWindow : Window
    {
        private static readonly Color LightBlue = Color.FromBytes(149, 167, 185);

        private Widget mainContent;
        private TreeView coursesView;
        private Button btnAddCard;
        private Button btnAddSubject;
        private Button btnAddLesson;
        private Table tableCards;
        private MiniCardContextMenu cardMenu;

        private readonly CoursesController lessonsController;
        private readonly CardsController cardsController;

        public MainWindow()
        {
            CreateComponents();

            lessonsController = new CoursesController(
                coursesView,
                btnAddSubject,
                btnAddLesson);
            
            cardsController = new CardsController(
                tableCards,
                btnAddCard,
                cardMenu,
                lessonsController);
        }

        public void ChangeContent(Widget newContent)
        {
            Content = newContent;
        }

        public void RestoreContent()
        {
            Content.Dispose();
            Content = mainContent;
            cardsController.UpdateView();
        }

        private void CreateComponents()
        {
            Width  = 1150;
            Height = 600;
            Icon = ResourcesManager.GetImage("icon.png");

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Title  = string.Format("Deblocus - v{0}.{1}.{2}",
                version.Major, version.Minor, version.Build);

            CloseRequested += HandleCloseRequested;

            var menuPanelDivision = new HPaned();
            menuPanelDivision.BackgroundColor = LightBlue;
            menuPanelDivision.Panel1.Content = MakeSidebar();
            menuPanelDivision.Panel2.Content = MakePanel();

            // Set the content
            Padding = new WidgetSpacing();
            Content = mainContent = menuPanelDivision;
        }

        private Widget MakeSidebar()
        {
            coursesView = new TreeView();
            coursesView.BackgroundColor = Colors.White;
            coursesView.HeadersVisible = false;
            coursesView.WidthRequest = 250;

            btnAddSubject = new Button(ResourcesManager.GetImage("book_add.png"), "Subject");
            btnAddLesson  = new Button(ResourcesManager.GetImage("report_add.png"), "Lesson");
            btnAddLesson.Sensitive = false;
            btnAddCard = new Button(ResourcesManager.GetImage("note_add.png"), "Card");
            btnAddCard.Sensitive = false;

            var topMenu = new HBox();
            topMenu.BackgroundColor = LightBlue;
            topMenu.MarginTop  = 5;
            topMenu.MarginLeft = 15;
            topMenu.PackStart(btnAddSubject);
            topMenu.PackStart(btnAddLesson);
            topMenu.PackStart(btnAddCard);

            var vbox = new VBox();
            vbox.BackgroundColor = LightBlue;
            vbox.PackStart(topMenu);
            vbox.PackStart(coursesView, true, true);
            return vbox;
        }

        private Widget MakePanel()
        {
            tableCards = new Table();
            tableCards.WidthRequest = 900;
            tableCards.BackgroundColor = LightBlue;
            tableCards.DefaultColumnSpacing = 20;
            tableCards.Margin = 5;

            cardMenu = new MiniCardContextMenu();

            return tableCards;
        }

        private void HandleCloseRequested(object sender, CloseRequestedEventArgs e)
        {
            Application.Exit();
        }
    }
}


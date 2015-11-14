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

        private ComboBox comboSubject;
        private ComboBox comboLesson;
        private Button btnSettings;
        private Table tableCards;


        public MainWindow()
        {
            CreateComponents();

            var subjectsController = new SubjectsController(this, comboSubject);
            var lessonsController  = new LessonsController(this, comboLesson,
                subjectsController);
            new CardsController(this, tableCards, lessonsController);

            subjectsController.Update();

            tableCards.Add(new CardView(), 0, 0);
            tableCards.Add(new CardView(), 1, 0);
        }

        private void CreateComponents()
        {
            Width  = 800;
            Height = 600;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Title  = string.Format("Deblocus - v{0}.{1}.{2}",
                version.Major, version.Minor, version.Build);

            CloseRequested += HandleCloseRequested;

            var menuPanelDivision = new VBox();
            menuPanelDivision.BackgroundColor = LightBlue;
            menuPanelDivision.PackStart(MakeMenuBar(), margin: 0);
            menuPanelDivision.PackStart(MakePanel(), true, margin: 0);

            // Set the content
            Padding = new WidgetSpacing();
            Content = menuPanelDivision;
        }

        private Widget MakeMenuBar()
        {
            var menuBox = new HBox();
            menuBox.MinHeight = 30;
            menuBox.BackgroundColor = Colors.LightPink;
            menuBox.Margin = new WidgetSpacing();

            var lblSubject = new Label("Subject:");
            lblSubject.MarginLeft = 10;
            menuBox.PackStart(lblSubject);

            comboSubject = new ComboBox();
            menuBox.PackStart(comboSubject, vpos: WidgetPlacement.Center);

            var lblLesson = new Label("Lesson:");
            lblLesson.MarginLeft = 10;
            menuBox.PackStart(lblLesson);

            comboLesson = new ComboBox();
            comboLesson.WidthRequest = 120;
            menuBox.PackStart(comboLesson, vpos: WidgetPlacement.Center);

            btnSettings = new Button(StockIcons.Information);
            btnSettings.MarginRight = 10;
            btnSettings.Style = ButtonStyle.Borderless;
            btnSettings.VerticalPlacement = WidgetPlacement.Center;
            menuBox.PackEnd(btnSettings);

            return menuBox;
        }

        private Widget MakePanel()
        {
            tableCards = new Table();
            tableCards.BackgroundColor = LightBlue;
            tableCards.DefaultColumnSpacing = 20;

            return tableCards;
        }

        private void HandleCloseRequested(object sender, CloseRequestedEventArgs e)
        {
            Application.Exit();
        }
    }
}


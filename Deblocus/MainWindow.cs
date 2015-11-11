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
using Xwt;
using System.Collections.Generic;
using Deblocus.Entities;

namespace Deblocus
{
    public partial class MainWindow
    {
        private readonly DatabaseManager database;
        private readonly IList<Subject> subjects;

        public MainWindow()
        {
            database = DatabaseManager.Instance;
            subjects = database.Retrieve<Subject>();

            CreateComponents();
            UpdateSubjects();

            comboSubject.SelectionChanged += OnSelectedSubject;
        }

        private void UpdateSubjects()
        {
            comboSubject.Items.Clear();
            foreach (var subj in subjects)
                comboSubject.Items.Add(subj, subj.Title);
            comboSubject.Items.Add(null, "New subject");
        }

        private void OnSelectedSubject(object sender, EventArgs e)
        {
            if (comboSubject.SelectedIndex == -1)
                return;

            var selectedItem = comboSubject.SelectedItem;
            if (selectedItem == null)
                CreateSubject();
            else
                UpdateLessons(selectedItem as Subject);
        }

        private void CreateSubject()
        {
            var subject = new Subject() { Title = "Test" }; // TODO: Open dialog
            subjects.Add(subject);
            database.SaveOrUpdate(subject);

            UpdateSubjects();
            comboSubject.SelectedItem = subject;
        }

        private void UpdateLessons(Subject subj)
        {
            comboLesson.Items.Clear();
            foreach (var lesson in subj.Lessons)
                comboLesson.Items.Add(lesson.Title);
            comboLesson.Items.Add(null, "New lesson");
        }

        private void HandleCloseRequested(object sender, CloseRequestedEventArgs e)
        {
            Application.Exit();
        }
    }
}


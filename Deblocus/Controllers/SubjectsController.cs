//
//  SubjectController.cs
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

namespace Deblocus.Controllers
{
    public delegate void SubjectChangedHandler(Subject subject);

    public class SubjectsController
    {
        public SubjectsController(ComboBox subjectBox)
        {
            SubjectBox = subjectBox;
            Subjects = DatabaseManager.Instance.Retrieve<Subject>();

            SubjectBox.SelectionChanged += OnSelectedSubject;
        }

        public IList<Subject> Subjects { get; private set; }
        public ComboBox SubjectBox { get; private set; }

        public event SubjectChangedHandler SubjectChanged;

        public void Update()
        {
            SubjectBox.Items.Clear();
            foreach (var subj in Subjects)
                SubjectBox.Items.Add(subj, subj.Title);
            SubjectBox.Items.Add(null, "New subject");
        }

        private void Create()
        {
            var subject = new Subject() { Title = "Test" }; // TODO: Open dialog
            Subjects.Add(subject);
            DatabaseManager.Instance.SaveOrUpdate(subject);

            Update();
        }

        private void OnSubjectChanged(Subject subject)
        {
            SubjectChanged?.Invoke(subject);
        }

        private void OnSelectedSubject(object sender, EventArgs e)
        {
            if (SubjectBox.SelectedIndex == -1)
                return;

            var selectedItem = SubjectBox.SelectedItem;
            if (selectedItem == null)
                Create();
            else
                OnSubjectChanged(selectedItem as Subject);
        }
    }
}


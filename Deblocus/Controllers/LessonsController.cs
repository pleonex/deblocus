//
//  LessonController.cs
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
using Deblocus.Entities;
using Xwt;
using Deblocus.Views;

namespace Deblocus.Controllers
{
    public delegate void LessonChangedHandle(Lesson lesson);

    public class LessonsController
    {
        public LessonsController(ComboBox lessonsBox, SubjectsController subjectController)
        {
            LessonsBox = lessonsBox;

            lessonsBox.SelectionChanged += OnSelectedLessonChanged;
            subjectController.SubjectChanged += OnSubjectChanged;
        }

        public ComboBox LessonsBox { get; private set; }
        public Subject CurrentSubject { get; private set; }

        public event LessonChangedHandle LessonChange;

        private void OnSubjectChanged(Subject subject)
        {
            CurrentSubject = subject;

            LessonsBox.Items.Clear();
            foreach (var lesson in subject.Lessons)
                LessonsBox.Items.Add(lesson, lesson.Title);
            LessonsBox.Items.Add(null, "New lesson");
        }

        void OnSelectedLessonChanged(object sender, EventArgs e)
        {
            if (LessonsBox.SelectedIndex == -1) {
                OnLessonChanged(null);
                return;
            }

            var selectedItem = LessonsBox.SelectedItem;
            if (selectedItem == null)
                Create();
            else
                OnLessonChanged(selectedItem as Lesson);
        }
            
        private void Create()
        {
            var questionDialog = new QuestionDialog("Lesson");
            if (questionDialog.Run(LessonsBox.ParentWindow) == Command.Ok) {            
                var lesson = new Lesson() { Title = questionDialog.Result };
                CurrentSubject.AddLesson(lesson);
                DatabaseManager.Instance.SaveOrUpdate(CurrentSubject);

                OnSubjectChanged(CurrentSubject);
            }

            LessonsBox.SelectedIndex = -1;
            questionDialog.Dispose();
        }

        private void OnLessonChanged(Lesson lesson)
        {
            LessonChange?.Invoke(lesson);
        }
    }
}


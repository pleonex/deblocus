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
using Deblocus.Views;
using Xwt;

namespace Deblocus.Controllers
{
    public delegate void LessonChangedHandle(Lesson lesson);

    public class CoursesController
    {
        private static readonly Xwt.Drawing.Image SubjectIcon =
            ResourcesManager.GetImage("book.png");
        private static readonly Xwt.Drawing.Image LessonIcon  =
            ResourcesManager.GetImage("report.png");

        private readonly TreeView treeView;
        private readonly TreeStore store;
        private readonly DataField<Xwt.Drawing.Image> imageCol;
        private readonly DataField<string> nameCol;
        private readonly DataField<Lesson> lessonCol;
        private readonly DataField<Subject> subjectCol;

        private readonly Button btnAddLesson;

        public CoursesController(TreeView treeView, Button btnAddSubject,
            Button btnAddLesson)
        {
            this.treeView = treeView;
            imageCol   = new DataField<Xwt.Drawing.Image>();
            nameCol    = new DataField<string>();
            subjectCol = new DataField<Subject>();
            lessonCol  = new DataField<Lesson>();
            store = new TreeStore(imageCol, nameCol, subjectCol, lessonCol);
            treeView.DataSource = store;
            treeView.Columns.Add("", imageCol, nameCol);

            UpdateView();
            treeView.SelectionChanged += OnTreeSelectionChanged;

            this.btnAddLesson = btnAddLesson;
            btnAddSubject.Clicked += CreateSubject;
            btnAddLesson.Clicked += CreateLesson;
        }

        public event LessonChangedHandle LessonChange;

        private void UpdateView()
        {
            foreach (var subject in DatabaseManager.Instance.Retrieve<Subject>())
                AddSubject(subject);
        }

        private void AddSubject(Subject subject)
        {
            var navigator = store.AddNode()
                .SetValue(imageCol, SubjectIcon)
                .SetValue(nameCol, subject.Title)
                .SetValue(subjectCol, subject);

            foreach (var lesson in subject.Lessons)
                AddLesson(navigator, lesson);
        }

        private void AddLesson(TreeNavigator navigator, Lesson lesson)
        {
            navigator.AddChild()
                .SetValue(imageCol, LessonIcon)
                .SetValue(nameCol, lesson.Title)
                .SetValue(subjectCol, lesson.Subject)
                .SetValue(lessonCol, lesson)
                .MoveToParent();
        }

        private void OnTreeSelectionChanged(object sender, EventArgs e)
        {
            btnAddLesson.Sensitive = false;

            if (treeView.SelectedRow == null)
                return;
            
            var navigator = store.GetNavigatorAt(treeView.SelectedRow);
            var lesson = navigator.GetValue(lessonCol);
            btnAddLesson.Sensitive = navigator.GetValue(subjectCol) != null;

            if (lesson != null && LessonChange != null)
                LessonChange.Invoke(lesson);
        }

        private void CreateSubject(object sender, EventArgs e)
        {
            var questionDialog = new QuestionDialog("Subject");
            if (questionDialog.Run(treeView.ParentWindow) == Command.Ok) {            
                var subject = new Subject { Title = questionDialog.Result };
                DatabaseManager.Instance.SaveOrUpdate(subject);

                AddSubject(subject);
            }

            questionDialog.Dispose();
        }

        private void CreateLesson(object sender, EventArgs e)
        {
            var questionDialog = new QuestionDialog("Lesson");
            if (questionDialog.Run(treeView.ParentWindow) == Command.Ok) {            
                var lesson = new Lesson { Title = questionDialog.Result };

                // Append lesson to the current selected subject
                var subjectNavigator = SearchSubjectNode();
                var selectedSubject = subjectNavigator.GetValue(subjectCol);
                selectedSubject.AddLesson(lesson);
                DatabaseManager.Instance.SaveOrUpdate(selectedSubject);

                AddLesson(subjectNavigator, lesson);
            }

            questionDialog.Dispose();
        }

        private TreeNavigator SearchSubjectNode()
        {
            var navigator = store.GetNavigatorAt(treeView.SelectedRow);

            // If we are at a lesson row, go to subject.
            if (navigator.GetValue(lessonCol) != null)
                navigator.MoveToParent();
            
            return navigator;
        }
    }
}


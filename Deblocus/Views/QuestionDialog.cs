//
//  QuestionDialog.cs
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

namespace Deblocus.Views
{
    public class QuestionDialog : Dialog
    {
        private TextEntry txtEntry;

        public QuestionDialog(string field)
        {
            CreateComponents(field);
        }

        public string Result {
            get { return txtEntry.Text; }
        }

        private void CreateComponents(string field)
        {
            Title = field;
            txtEntry = new TextEntry();

            var table = new Table();
            table.Add(new Label(field + ":"), 0, 0);
            table.Add(txtEntry, 1, 0);
            Content = table;

            Buttons.Add(new DialogButton(Command.Ok));
            Buttons.Add(new DialogButton(Command.Cancel));
        }
    }
}


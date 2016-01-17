//
//  GradientColor.cs
//
//  Author:
//       Benito Palacios Sánchez (aka pleonex) <benito356@gmail.com>
//
//  Copyright (c) 2016 Benito Palacios Sánchez
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
using System.Collections.ObjectModel;
using Xwt.Drawing;

namespace Deblocus.Views
{
    public class GradientColor
    {
        private Color[] colors;

        public GradientColor(int numColors, params Color[] fixedColors)
        {
            if (fixedColors == null || fixedColors.Length < 2)
                throw new ArgumentNullException("fixedColors", "2 or more colors needed");

            NumColors = numColors;
            Calculate(fixedColors);
        }

        public int NumColors { get; private set; }

        public ReadOnlyCollection<Color> Colors {
            get { return new ReadOnlyCollection<Color>(colors); }
        }

        public Color this[int index] {
            get {
                if (index < 0 || index >= colors.Length)
                    throw new ArgumentOutOfRangeException();

                return colors[index];
            }
        }

        private void Calculate(Color[] fixedColors)
        {
            colors = new Color[NumColors];

            // Number of steps between fixed colors.
            // TODO: Improve with interpolation.
            int numStep = (int)Math.Ceiling(1.0 * NumColors / fixedColors.Length);

            // Create the array of color component steps between fixed color.
            var steps = new double[fixedColors.Length - 1, 3];
            for (int i = 0; i < fixedColors.Length - 1; i++) {
                steps[i, 0] = (fixedColors[i + 1].Red   - fixedColors[i].Red)   / numStep;
                steps[i, 1] = (fixedColors[i + 1].Green - fixedColors[i].Green) / numStep;
                steps[i, 2] = (fixedColors[i + 1].Blue  - fixedColors[i].Blue)  / numStep;
            }

            for (int i = 0; i < NumColors; i++) {
                double porcentage = i % numStep;
                int fixedIndex = i / numStep;
                if (fixedIndex >= fixedColors.Length - 1) { // Happens with odd numbers
                    fixedIndex = fixedColors.Length - 2;
                    porcentage = numStep;
                }


                colors[i] = new Color(
                    fixedColors[fixedIndex].Red   + steps[fixedIndex, 0] * porcentage,
                    fixedColors[fixedIndex].Green + steps[fixedIndex, 1] * porcentage,
                    fixedColors[fixedIndex].Blue  + steps[fixedIndex, 2] * porcentage);
            }
        }
    }
}


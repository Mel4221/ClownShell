﻿//
// ${Melquiceded Balbi Villanueva}
//
// Author:
//       ${Melquiceded} <${melquiceded.balbi@gmail.com}>
//
// Copyright (c) ${2089} MIT
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using QuickTools.QCore;
using QuickTools.QIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownShell.ErrorHandler
{
    public class ErrorHandeler
    {

        public enum ErrorType
        {
            NotValidAction,
            NotValidType,
            NotValidParameter,
            ExecutionError
        }

        public void DisplayError(ErrorType type, string message)
        {

            string error = $"####\n There Was an error with the given type of error: '{type}' '{message}' \n####";
            Log.Event("ErrorHandeler", error);
            QuickTools.QColors.Color.Red(error);
        }
        public void DisplayError(ErrorType type, string[] givenCommand)
        {

            string error = $"####\n There Was an error with the given type of error: '{type}' '{IConvert.ArrayToText(givenCommand)}' \n####";
            Log.Event("ErrorHandeler", error);
            QuickTools.QColors.Color.Red(error);
        }
    }
}

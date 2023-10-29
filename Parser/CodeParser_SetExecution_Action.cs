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
using QuickTools.QIO;
using QuickTools.QData;
using QuickTools.QCore;
using QuickTools.QConsole;
using System.Diagnostics;
using ClownShell.Init;
using ClownShell.ErrorHandler;
using ClownShell.ScripRunner;
using ClownShell.Helpers; 
namespace ClownShell.Parser
{


    public partial class CodeParser
    {
        /// <summary>
        /// runs the action without any other 
        /// </summary>
        /// <param name="action"></param>
        public void SetExecution(string action)
        {
            
            
            // Get.Wait(Get.DataPath());
            this.cache = new DataCacher();
            this.runner = new ScriptRunner();
            this.error = new ErrorHandeler();
            ProcessStartInfo process;

            this.Target = Get.FixPath(ShellLoop.CurrentPath);
            if (Helper.IsExecutable(action)) {
                runner.Run(() => {
                    process = new ProcessStartInfo($"{this.Target}\\index.html");
                    process.CreateNoWindow = true;
                    process.UseShellExecute = false;
                    Process.Start(process);
                }); 
                return; 
            }
            switch (action)
            {
                case "exit":
                    Environment.Exit(0);
                    return;
                case "console-clear":
                case "clear":
                    runner.Run(() => { Get.Clear(); });
                    break;
                case "reset-path":
                        ShellLoop.CurrentPath = Directory.GetCurrentDirectory();
                    break;
                case "clear-cache":
                case "cache-reset":
                    runner.Run(() => { cache.ClearCache(); });
                    break;
                case "clear-logs":
                    runner.Run(() => { Log.ClearLogs(); });
                    break;
                case "ls":
                     runner.Run(() => { Get.Ls(this.Target); });
                   // Get.Ls(ShellLoop.CurrentPath);
                    break;
                case "get-input":
                case "input":
                    runner.Run(() => {
                        cache.Cach("EntryInput", Get.Input("Type Something: ").Text);
                    });
                    break;
                case "select":
                case "-S":
                    runner.Run(() => {
                        string[] files = new FilesMaper().GetFiles(this.Target);
                        string[] folders = Directory.GetDirectories(this.Target);
                        string[] both = new string[files.Length+folders.Length];
                        if (files.Length > 0)
                        {
                            for (int current = 0; current < files.Length; current++)
                            {
                                both[current] = Get.FileNameFromPath(files[current]);
                            }
                        }
                        int bothLength = both.Length - 1;  
                        if(folders.Length > 0)
                        {
                            for (int current = 0; current < folders.Length; current++)
                            {
                                string path = folders[current];
                                both[bothLength] = path.Substring(path.LastIndexOf(Get.Slash()) + 1);
                                bothLength--;
                            }
                        }
            
                        Options option = new Options(both);
                        option.Label = this.Target;
                        option.SelectorL = "> ";
                        option.SelectorR = ""; 
                        int selection = option.Pick();
                        string str = null;
                        str = this.Target[this.Target.Length - 1].ToString() == Get.Slash() ? "" : Get.Slash(); 
                        ShellLoop.SelectedOject = $">{this.Target}{Get.Slash()}{both[selection]}";
                        //Get.Yellow(this.Target);
                        //Get.Wait(ShellLoop.SelectedOject);
                    });
                    break;
                case "select?":
                case "selected":
                case "-S?":
                    runner.Run(() => {
                        Get.Blue();
                        Get.WriteL(" ");
                        Get.Write($"Object Selected: ");
                        Get.Yellow();
                        string str = ShellLoop.SelectedOject == "" || ShellLoop.SelectedOject  == null? "NONE" : ShellLoop.SelectedOject;
                        Get.Write(str);
                    });
                    break;
                case "clear-selected":
                case "clear-S":
                     runner.Run(() => {
                         ShellLoop.SelectedOject = null;
                    });
                    break;
                case "history":
                    runner.Run(() => {
                      
                        MiniDB db =  new ShellLoop().GetHistory();
                            Get.WriteL(" ");
                            db.DataBase.ForEach((item) => {
                            Get.Green();
                            Get.Write($"No: {item.Id} ");
                            Get.Yellow();
                            Get.Write($"Command: {item.Value} ");
                            Get.Blue();
                            Get.Write($"Date: {item.Relation} ");
                            Get.WriteL(" ");
                        });
                    });
                    break;
                case "disks":
                    runner.Run(() => { Get.PrintDisks(); });
                    break;
                case "get-logs":
                    runner.Run(() => {
                        ShellLoop.CurrentPath = Get.DataPath("logs");
                    });
                    break;
                case "shutdown":
                    runner.Run(() => {
                        switch (Get.IsWindow())
                        { case true:
                                process = new ProcessStartInfo("shutdown", "/s /t 0");
                                process.CreateNoWindow = true;
                                process.UseShellExecute = false;
                                Process.Start(process);
                                break;
                            default:
                                process = new ProcessStartInfo("shutdown", "0");
                                process.CreateNoWindow = true;
                                process.UseShellExecute = false;
                                Process.Start(process);
                                break; 
                        }
                      
                     
                    });
                    break;
                case "reboot":
                    runner.Run(() => {//shutdown -r -t 0
                         switch (Get.IsWindow())
                        {
                            case true:
                                process = new ProcessStartInfo("shutdown", "-r -t 0");
                                process.CreateNoWindow = true;
                                process.UseShellExecute = false;
                                Process.Start(process);
                                break;
                            default:
                                process = new ProcessStartInfo("reboot", "0");
                                process.CreateNoWindow = true;
                                process.UseShellExecute = false;
                                Process.Start(process);
                                break;
                        }

                    });
                    break;
                case "cmd":
                    runner.Run(() => {
                        Process cmd = new Process();

                        cmd.StartInfo.FileName = "cmd";//"cmd.exe";
                                                       //cmd.StartInfo.Arguments;
                                                       //cmd.StartInfo.RedirectStandardInput = true;
                        cmd.StartInfo.RedirectStandardOutput = false;  // true;
                        cmd.StartInfo.CreateNoWindow = false;
                        cmd.StartInfo.UseShellExecute = false;
                        //cmd.StartInfo.Arguments = "ping www.google.com"; //Helper.ResolvePath(this).Target;

                        cmd.Start();
                        cmd.WaitForExit();
                        /* execute "dir" */

                        //cmd.StandardInput.WriteLine(this.SubTarget);
                        //cmd.StandardInput.Flush();
                        //cmd.StandardInput.Close();
                        //Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                    });
                    break;
                case "powershell":
                    runner.Run(() => {
                        Process cmd = new Process();

                        cmd.StartInfo.FileName = "powershell";//"cmd.exe";
                                                       //cmd.StartInfo.Arguments;
                                                       //cmd.StartInfo.RedirectStandardInput = true;
                        cmd.StartInfo.RedirectStandardOutput = false;  // true;
                        cmd.StartInfo.CreateNoWindow = false;
                        cmd.StartInfo.UseShellExecute = false;
                        //cmd.StartInfo.Arguments = "ping www.google.com"; //Helper.ResolvePath(this).Target;

                        cmd.Start();
                        cmd.WaitForExit();
                        /* execute "dir" */

                        //cmd.StandardInput.WriteLine(this.SubTarget);
                        //cmd.StandardInput.Flush();
                        //cmd.StandardInput.Close();
                        //Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                    }); 
                    break;
                default:
                    error.DisplayError(ErrorHandeler.ErrorType.NotValidAction, this.Code);
                    break;
            }
        }

    }
}

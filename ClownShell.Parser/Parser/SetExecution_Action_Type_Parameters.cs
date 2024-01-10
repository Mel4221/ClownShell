﻿using System;
using System.IO; 
using System.Collections.Generic;
using System.Threading;
using ErrorHandelers;
using QuickTools.QData;
using ScriptRunner;
using System.Diagnostics;
using States;
using QuickTools.QCore;
using QuickTools.QIO;
using Parser.Types.Functions;
using Parser.Types;
using QuickTools.QSecurity.FalseIO;
using System.Runtime;

namespace Parser
{
    public partial class CodeParser
    {

		/*
			The mechanisims that is in charge of transforming the - 
			Variable pointer into it's acual value has to be changed
		 */
        public void SetExecution(string action,string type, string[] parameters)
        {	
			
			ErrorHandeler error = new ErrorHandeler();
			Runner runner = new Runner();
			//ProcessStartInfo process;
			string[] param = parameters;
			//Print.List(param); 
			ShellTrace.AddTrace($"Execution Started With Action: {action} Type: {type} Parameters: {IConvert.ArrayToText(param)}");
			//string file, path,outFile;

            Get.Green($"{this.Action} {this.Type} {IConvert.ArrayToText(this.Parameters)}");
            Get.Blue($"{action} {type} {IConvert.ArrayToText(param)}");


            string path; 
			switch (action)
              {
                case "ls":
                case "list-files":
                    runner.Run(() => {
                        path = param[0]; 
                        /*
                        if (this.IsRootPath(path))
                        {
                            Get.Ls(type,null);
                            return;
                        }
        */
                        if (type == "-l")
                        {
                            Get.Ls(path, null);
                            return;
                        }


                        if (type == ".")
                        {
                            Get.Ls(Shell.CurrentPath);
                            return;
                        }

                        if (Directory.Exists(this.GetPathWithType(type)))
                        {
                            Get.Ls(this.GetPathWithType(type));
                            return;
                        }
                        if (Helper.HasSpecialFolder(type) != null)
                        {
                            Get.Ls(Helper.HasSpecialFolder(type));
                            return;
                        }
                        if (type.Contains(".") && type.Length >= 2)
                        {
                            //path = ShellLoop.CurrentPath[ShellLoop.CurrentPath.Length-1]==Get.Slash()[0] ? $"{ShellLoop.CurrentPath}{type}" : $"{ShellLoop.CurrentPath}{Get.Slash()}{type}";
                            Get.Ls(this.GetPathWithType(type));
                            return;
                        }
                        else
                        {
                            Get.Ls(type);
                        }


                        //Get.Yellow($"{this.Target}     ClearTarget: {this.SubTarget} Type: {type}");
                        //Get.Blue(Path.GetDirectoryName(this.Target));
                        // Get.Yellow(ShellLoop.RelativePath);
                        // Get.Wait(type);
                        //this.SubTarget = type; 
                        //CodeParser helper = Helper.ResolvePath(this);
                        //this.Target = helper.Target;
                        //this.SubTarget = helper.SubTarget;
                        //Get.Cyan($"Target: {this.Target} SubTarget: {this.SubTarget}");

                        //if(type == "-l")
                        //{
                        //    Get.Ls(helper.Target, "");
                        //    return; 
                        //}
                        //if (type.Contains('*'))
                        //{
                        //    //Get.Wait($"{this.Target.Substring(0,this.Target.LastIndexOf("*"))} {type.Substring(1)}");
                        //    // Get.Wait(type);  //Get.FileExention(type)
                        //    //Get.Wait(this.Target.Substring(this.Target.LastIndexOf(Get.Slash())));
                        //    //this get all the files that has this given type 
                        //    Get.Ls(this.Target,this.SubTarget,true);
                        //    return;
                        //}
                        //else
                        //{
                        //    if (Directory.Exists(this.Target))
                        //    {
                        //        Get.Ls(this.Target);
                        //        return;
                        //    }

                        //}
                    });
                    break;
                case "status":
                case "porcent":
                    runner.Run(() => {
                        if (!Get.IsNumber(type) || !Get.IsNumber(parameters[0]))
                            {
                                error.DisplayError(ErrorType.NotValidParameter);
                                 return;
                            }
                            Get.Green(Get.Status(type, parameters[0]));
                    });
                    break;
                case "install":
					error.DisplayError(ErrorType.NotImplemented);
					break;
				case "trojan":
					runner.Run(() => {
						error.DisplayError(ErrorType.NotImplemented);

						/*
				trojan file.txt 
				pack
				unpack


			        */

						//Trojan trojan;
						//string payload;
						/*
						file = null;
						outFile = null;
						path = param[0]; 
						if(this.IsRootPath(path))
						{
							file = path;
						}if(file == null)
						{
							file = this.GetPathWithType(path); 
						}if(!File.Exists(file))
						{
							error.DisplayError(ErrorType.NotValidType, $"The file was not found: {file}");
							return;
						}if (param.Length == 3)
						{

							if (this.IsRootPath(param[2]))
							{
								outFile = param[2];
							}
							if (outFile == null){
								outFile = this.GetPathWithType(param[2]);	
							}
						}

						//trojan pack vide.mp4 > file.txt 
						switch (type)
						{
							case "pack":
							case "-p":
								error.DisplayError(ErrorType.NotImplemented);
								break;
							case "unpack":
							case "-u":
								error.DisplayError(ErrorType.NotImplemented);
								break;
							case "info":
							case "-i":
								error.DisplayError(ErrorType.NotImplemented);
								break;
							default:
								error.DisplayError(ErrorType.NotValidParameter);	
								break;
						}*/
					});
					break;
				case "int":
				case "long":
				case "double":
				case "float":
					runner.Run(() => {
						bool isNumber;
						double number;
						isNumber = double.TryParse(param[1],out number);
						if(!isNumber && number < double.MaxValue && number > double.MinValue)
						{
							ShellTrace.AddTrace($"The Parameter '{param[1]}' was not recognized as a valid number");
							error.DisplayError(ErrorType.NotValidParameter, $"Invalid Parameter: {ShellTrace.GetTrace()}");
							return;
						}
						Shell.VStack.SetVariable(new Variable()
						{
							Name=type,
							Value=param[1],
							IsEmpty=false
						});
					});
					break;
				case "list":
				case "array":
				//[] = {};
					break;
				case "shell-path":
					runner.Run(() => {
						Shell.CurrentPath = param[0];
					});
					break;
				case "var":
					runner.Run(() => {
						//Get.Yellow($"{type} = {param[1]};");
						//var y = ls;
						//var x = input;
						//action  type
						//shell-path = d:/path/folder/
						//var     user = "melquiceded balbi villanueva"
						Shell.VStack.SetVariable(new Variable(){
							Name=type,
							Value=param[1],
							IsEmpty=false
						});	
					});
					break;
				case "const":
					runner.Run(() => {
						//Get.Yellow($"{type} = {param[1]};");
						//var y = ls;
						//var x = input;
						//action  type     
						//var     user = "melquiceded balbi villanueva"
						Shell.VStack.SetVariable(new Variable()
						{
							Name=type,
							Value=param[1],
							IsEmpty=false,
							IsConstant = true
						});
					});
					break;
				case "input":
                    runner.Run(() => {
						//input => 
						//input = variable
						//action type param[0]

						if(Shell.VStack.Exist(new Variable() {Name = param[0] }))
						{
							error.DisplayError(ErrorType.VariableAlreadySet, $"Variable already Set at {action} {type} '{param[0]}'");
							return;
						}
						Shell.VStack.SetVariable(new Variable()
						{
							Name = param[0],
							Value = Get.Input().Text,
							IsConstant = false,
							IsEmpty = false
						});
					});
					break;
				case "rm":
					runner.Run(() =>
					{
						//rm -r *
						FilesMaper maper;
						List<Error> errors = new List<Error>(); 
						string[] files;
						string[] dirs; 
						if (type == "-r")
						{
							if(param[0][0] == '*')
							{
								maper = new FilesMaper(Shell.CurrentPath);
								maper.AllowDebugger = true; 
								maper.Map(); 
								files = maper.Files.ToArray();
								dirs = maper.Directories.ToArray();

								
							 
								for(int file = files.Length; file  > 0; file--)
								{
									if(File.Exists(files[file - 1]))
									{
										try
										{
											File.SetAttributes(files[file-1], FileAttributes.Archive); 
											File.Delete(files[file - 1]);
											Get.Red($"FILE DELETED: {files[file - 1]}");
										}catch(Exception ex)
										{
											errors.Add(new Error()
											{
												Type = $"FailToDeleteFile: {files[file -1]}",
												Message = ex.Message
											});
										}
									}
								}
									
								for (int dir = dirs.Length; dir  > 0; dir--)
									{
										if (Directory.Exists(dirs[dir - 1]))
										{
											try
											{
											DirectoryInfo info = new DirectoryInfo(dirs[dir-1]);
											info.Delete(true);
											//	Directory.Delete(dirs[dir - 1]);
												Get.Blue($"DIR DELETED: {dirs[dir - 1]}");
											}catch(Exception ex)
											{
												errors.Add(new Error() { 
													Type = $"FailToDeleteDirectory: {dirs[dir-1]}",
													Message = ex.Message
												});
											}
										}
									}
									foreach (Error err in errors) Get.Yellow(err.ToString());
							}
						}
					});
					break;
				case "echo":
					//Get.Wait($"{this.IsRootPath(type)} {this.IsRootPath(param[1])}");
					runner.Run(() => {
						// Get.Wait($"{this.GetPathWithType(param[1])}");

						//echo "this text" > file.txt
						//Print.List(param); 

						if (this.IsRootPath(type) && this.IsRootPath(param[1]))
						{
							//Get.Wait("Rooted");
							Binary.CopyBinaryFile(type, param[1]);

							return;
						}
						else
						{

							//Get.Yellow($"{this.GetPathWithType(param[1])} > {type}");
							string str = type.Replace('"'.ToString(), "");
							Writer.Write(this.GetPathWithType(param[1]), str);
							Get.Yellow($"{type} > {param[1]} {Get.FileSize(Get.Bytes(str))}");
						}
					});
					break;
				default:
					//name = "new value"
					//number = 200
					//number = "829-978-2244"
					//2 / 4
					//var * 7.
					//x = var * 7
					//$x = $b
					//var x = 2
					//*3 / 2
					//b = x
					//var name = value
					//echo *name 
					//free name
					
					//Get.Blue($"{this.Action.Substring(1)} {CodeTypes.IsVariable(this.Action.Substring(1))}");
					if(CodeTypes.IsVariable(this.Action.Substring(1)))
					{
						CodeTypes types = new CodeTypes(this.Action, this.Type, this.Parameters);
						types.RunAssingment();
						return; 
					}if(Functions.IsFunction(action))
					{
						error.DisplayError(ErrorType.NotImplemented);
						return;
					}
					
					ShellTrace.AddTrace($"Action Was not Recognized as a valid Action");
					error.DisplayError(ErrorType.NotValidAction, $"At: Execution Action With Type and parameters '{action}' {type} {IConvert.ArrayToText(param)}Trace: \n{ShellTrace.GetTrace()}");

					break; 
              }
        }
    }
}

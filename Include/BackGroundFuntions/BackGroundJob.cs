﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickTools.QCore; 
using System.Threading;
using System.Collections;
using System.Runtime.CompilerServices;

namespace ClownShell.BackGroundFunctions
{


    public class Job
    {
        public Thread BThread { get; set; }
        public string Name { get; set; } = string.Empty; 
        public int ID { get; set; } = 0; 
        public Action JobAction { get; set; }
        public bool AllowToBeKilled { get; set; } = true; 
        public string TextStatus { get; set; } = string.Empty;
        public double Status { get; set; } = 0; 
        public string Info = string.Empty;
        public override string ToString()
        {
            return $"ID: [{this.ID}] Job Name: [{this.Name}] Status: [{this.Status}] Job Info: [{this.Info}] AlowToBeKilled: [{this.AllowToBeKilled}]";
        }
    }
    public static class BackGroundJob 
    {
        public static bool MonitorStarted { get; set; } 
        private static Thread MonitorThread { get; set; } 
        private static int Indexer { get; set; } = 0;
        private static List<Job> Jobs { get; set; } = new List<Job>(); 
        public static bool HasJobs { get; set; } = false; 
        

        private static void RemoveTerminatedJobs()
        {
            for(int job = 0; job < Jobs.Count; job++)
            {
                if (!Jobs[job].BThread.IsAlive)
                {
                    Jobs.RemoveAt(job); 
                }
            }
        }
        private static void WatchJobs()
        {
            while (true)
            {
                RemoveTerminatedJobs();
                switch (Jobs.Count)
                {
                    case 0:
                        HasJobs = false; 
                        break;
                    default:
                        HasJobs = true; 
                        break;
                }
                Thread.Sleep(500); 
            }
        }

        public static void PrintRunningJobs()
        {
            if(Jobs.Count == 0)
            {
                Get.White("No Background Jobs");
                return;
            }
            //Top Table
            Get.White();
            Get.Write($"[ID] ");
            Get.Green();
            Get.Write($"[Name] ");
            Get.White();
            Get.Write($"[Description] ");
            Get.Red();
            Get.Write($"[Killable] ");
            Get.Yellow();
            Get.Write($"[IsAlive]\n");
            foreach (Job job in Jobs)
            {

                //Button Line
                Get.White();
                Get.Write($"[{job.ID}] ");
                Get.Green();
                Get.Write($"[{job.Name}] ");
                Get.White();
                Get.Write($"[{job.Info}] ");
                Get.Red(); 
                Get.Write($"[{job.AllowToBeKilled}] ");
                Get.Yellow();
                Get.Write($"[{job.BThread.IsAlive}]\n");
            }
        }
        public static string GetJobInfo(int id)
        {
           if(id > Jobs.Count || id < 0)
            {
                return string.Empty;
            }
            else
            {
                return Jobs[id].ToString(); 
            }
            
        }
        public static void AddJob(Job job)
        {
            Indexer++; 
            Jobs.Add(new Job()
            {
                Name = job.Name,
                ID = Indexer,
                JobAction = job.JobAction,
                AllowToBeKilled = job.AllowToBeKilled,
                TextStatus = job.TextStatus,
                Status = job.Status,
                Info = job.Info
            });
            Get.White($"BackGroundJob Added ID:[{Indexer}]");
        }
        public static void Run(int id)
        {
            if (Jobs != null)
            {
                if (!Jobs[id].BThread.IsAlive)
                {
                    Jobs[id].BThread = new Thread(() => {
                        Jobs[id].JobAction();
                    });
                    Jobs[id].BThread.Start();
                }
            }
        }

        public static void KillAll()
        {
            if (Jobs != null)
            {
                Jobs.ForEach((item) => {
                    if (!item.AllowToBeKilled)
                    {
                        throw new Exception("Item Not Allwed To be Killed");
                    }
                    if (item.BThread.IsAlive)
                    {
                        item.BThread.Abort();
                    }
                });
                Jobs.Clear();
            }
        }
        public static void Kill(int id)
        {
            if (Jobs != null)
            {
                for (int current = 0; current < Jobs.Count; current++)
                {
                    if (Jobs[current].ID == id)
                    {
                        if (!Jobs[current].AllowToBeKilled)
                        {
                            throw new Exception("The job don't support to be killed");
                        }


                        if (Jobs[current].BThread.IsAlive)
                        {
                            try
                            {
                                Jobs[current].BThread.Abort();
                                Get.Red($"Job Killed ID:[{Indexer}] Name: [{Jobs[current].Name}] Info: [{Jobs[current].Info}]");
                                Jobs.RemoveAt(current);
                            }
                            catch (Exception ex)
                            {
                                Get.Alert($"The job could not be killed for the fallowing reason \n{ex}");
                            }
                        }
                    }
                }

            }
        }

        public static void Pause(int id)
        {
            if (Jobs != null)
            {
                for (int current = 0; current < Jobs.Count; current++)
                {
                    if (Jobs[current].ID == id)
                    {
                        if (Jobs[current].BThread.IsAlive)
                        {
#pragma warning disable CS0618 // Type or member is obsolete
                            Jobs[current].BThread.Suspend();
#pragma warning restore CS0618 // Type or member is obsolete
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resume the background worker
        /// </summary>
        /// <param name="id"></param>
        public static void Resume(int id)
        {
            if (Jobs != null)
            {
                for (int current = 0; current < Jobs.Count; current++)
                {
                    if (Jobs[current].ID == id)
                    {
                        if (Jobs[current].BThread.IsAlive)
                        {
                            Jobs[current].BThread.Resume();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Run the background jobs
        /// </summary>
        public static void RunJobs()
        {
           
            if (Jobs != null)
            {
                Jobs.ForEach((item) => {
                    if (item.JobAction != null)
                    {
                        item.BThread = new Thread(() =>
                        {
                            item.JobAction();
                        });
                        item.BThread.Start();
                    }
                });
                if (!MonitorStarted)
                {
                    MonitorThread = new Thread(() => {
                        WatchJobs();
                    });
                    MonitorThread.Start(); 
                }
                
            }
        }

    }


}

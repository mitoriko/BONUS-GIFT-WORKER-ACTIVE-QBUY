﻿using Com.ACBC.Framework.Database;
using QuartzRedis.Buss;
using QuartzRedis.Common;
using System;

namespace QuartzRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            Global.Startup();
            TaskJob.Worker();
            TaskJob.Subscribe();
            Console.ReadLine();
        }
    }
}

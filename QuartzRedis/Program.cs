using Com.ACBC.Framework.Database;
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
            TaskJobBuss taskJobBuss = new TaskJobBuss();
            taskJobBuss.doWork("");
            TaskJob.Subscribe();
            Console.ReadLine();
        }
    }
}

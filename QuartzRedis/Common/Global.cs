﻿using Com.ACBC.Framework.Database;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzRedis.Common
{
    public class Global
    {

#if DEBUG
        public const string ENV = "DEV";
#else
        public const string ENV = "PRO";
#endif
        public const string GROUP = "Task-Gift";

        public const string TASK_JOB = "GIFT-QBUY";

        public const string TASK_PREFIX = "Task";

        public const string CONFIG_TOPIC = "ConfigServerTopic";
        public const string TASK_TOPIC = "TaskTopic";

        public const string TOPIC_MESSAGE = "update";

        public const int REDIS_DB = 11;

        static Action<ChannelMessage> action = new Action<ChannelMessage>(onMessageHandle);


        public static void Startup()
        {
            GetConfig(true);
        }

        static void Subscribe()
        {
            var redis = RedisManager.getRedisConn();
            var queue = redis.GetSubscriber().Subscribe(CONFIG_TOPIC + "." + ENV + "." + GROUP);

            queue.OnMessage(action);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "已订阅" + CONFIG_TOPIC + "." + ENV + "." + GROUP + "配置更新");
        }

        public static void onMessageHandle(ChannelMessage channelMessage)
        {
            if (channelMessage.Message.ToString() == TOPIC_MESSAGE)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "收到配置更新通知");
                GetConfig(false);
            }
        }

        static void GetConfig(bool isFirst)
        {
            string url = "http://ConfigServer/api/config/Config/Pro";
#if DEBUG
            url = "http://" + ConfigServer + "/api/config/Config/Dev";
#endif
            ConfigParam configParam = new ConfigParam
            {
                env = ENV,
                group = GROUP
            };
            RequestParam requestParam = new RequestParam
            {
                method = "GetConfig",
                param = configParam
            };
            string body = JsonConvert.SerializeObject(requestParam);
            try
            {
                string resp = Utils.PostHttp(url, body);
                ResponseObj responseObj = JsonConvert.DeserializeObject<ResponseObj>(resp);

                foreach (ConfigItem item in responseObj.data)
                {
                    Environment.SetEnvironmentVariable(item.key, item.value);
                }

                DatabaseOperation.TYPE = new DBManager();
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "加载配置信息完成");
                if (isFirst)
                {
                    Subscribe();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(url);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "> " + "加载配置信息失败");
            }
        }

        public static string Redis
        {
            get
            {
                return Environment.GetEnvironmentVariable("Redis");
            }
        }

        public static string ConfigServer
        {
            get
            {
                return Environment.GetEnvironmentVariable("ConfigServer");
            }
        }

        public static string DBUrl
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBUrl");
            }
        }

        public static string DBUser
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBUser");
            }
        }

        public static string DBPort
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBPort");
            }
        }

        public static string DBPassword
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBPassword");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzRedis
{
    public class ConfigItem
    {
        public string key;
        public string value;
    }

    public class ConfigParam
    {
        public string env;
        public string group;
    }

    public class RequestParam
    {
        public string method;
        public object param;
    }

    public class ResponseObj
    {
        public bool success;
        public ResponseMsg msg;
        public List<ConfigItem> data;
    }

    public class ResponseMsg
    {
        public string code;
        public string msg;
    }

    public class ActiveQBuyItem
    {
        public string activeQBuyId;
        public string activeId;
        public double beforeStart;
        public int lastDays;
        public int checkNum;
        public int consumeNum;
        public int minConsume;
        public string storeId;
        public DateTime dateFrom;
        public DateTime dateTo;
    }

    public class MemberCheckStore
    {
        public string storeId;
        public string memberId;
        public DateTime checkTime;
        public string consume;
    }

    public class Member
    {
        public string memberName;
        public string memberId;
        public string heart;
        public string memberImg;
    }

    public class Goods
    {
        public string goodsId;
        public string goodsName;
        public string goodsImg;
        public double goodsPrice;
        public int goodsStock;
    }

    public class Store
    {
        public string storeId;
        public string storeCode;
        public string storeAddr;
    }

}

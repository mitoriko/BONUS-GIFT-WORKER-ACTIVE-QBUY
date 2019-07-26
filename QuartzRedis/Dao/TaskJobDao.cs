using Com.ACBC.Framework.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace QuartzRedis.Dao
{
    public class TaskJobDao
    {
        public List<ActiveQBuyItem> GetActiveQBuy()
        {
            List<ActiveQBuyItem> list = new List<ActiveQBuyItem>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(TaskJobSqls.SELECT_ACTIVE_QBUY_LIST);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    ActiveQBuyItem item = new ActiveQBuyItem
                    {
                        activeId = dr["ACTIVE_ID"].ToString(),
                        activeQBuyId = dr["ACTIVE_QBUY_ID"].ToString(),
                        beforeStart = Convert.ToDouble(dr["BEFORE_START"]),
                        lastDays = Convert.ToInt32(dr["LAST_DAYS"]),
                        checkNum = Convert.ToInt32(dr["CHECK_NUM"]),
                        consumeNum = Convert.ToInt32(dr["CONSUME_NUM"]),
                        minConsume = Convert.ToInt32(dr["MIN_CONSUME"]),
                        storeId = dr["STORE_ID"].ToString()
                    };

                    list.Add(item);
                }
            }


            return list;
        }
    }

    public class TaskJobSqls
    {
        public const string SELECT_ACTIVE_QBUY_LIST = ""
            + "SELECT * "
            + "FROM T_BUSS_ACTIVE A, T_BUSS_ACTIVE_QBUY B "
            + "WHERE A.ACTIVE_TYPE = 2 "
            + "AND A.ACTIVE_ID = B.ACTIVE_ID "
            + "AND A.ACTIVE_TIME_FROM < NOW() "
            + "AND A.ACTIVE_TIME_TO > NOW() "
            + "AND A.ACTIVE_STATE = 1";
    }
}

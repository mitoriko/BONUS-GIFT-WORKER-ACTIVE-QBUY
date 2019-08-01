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
            DataTable dt = DatabaseOperation.ExecuteSelectDS(sql, "T").Tables[0];
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
                        storeId = dr["ACTIVE_STORE"].ToString(),
                        dateFrom = Convert.ToDateTime(dr["ACTIVE_TIME_FROM"]),
                        dateTo = Convert.ToDateTime(dr["ACTIVE_TIME_TO"]),
                    };

                    list.Add(item);
                }
            }


            return list;
        }

        public List<string> GetMemberCheckStores(
            string storeId, 
            string dateFrom, 
            string dateTo, 
            int minConsume,
            int consumeNum,
            int checkNum,
            string activeId)
        {
            List<string> listCheck = new List<string>();
            List<string> list = new List<string>();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(
                TaskJobSqls.SELECT_CHECK_STORE_BY_STORE_AND_GROUP_MEMBER_FOR_CHECK, 
                storeId, 
                dateFrom, 
                dateTo,
                checkNum,
                activeId);
            string sql = builder.ToString();
            DataTable dt = DatabaseOperation.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var checkMember = dr["MEMBER_ID"].ToString();
                    listCheck.Add(checkMember);
                }
            }

            builder.Clear();
            builder.AppendFormat(
                TaskJobSqls.SELECT_CHECK_STORE_BY_STORE_AND_GROUP_MEMBER_FOR_CONSUME,
                storeId,
                dateFrom,
                dateTo,
                minConsume,
                consumeNum,
                activeId);
            sql = builder.ToString();
            dt = DatabaseOperation.ExecuteSelectDS(sql, "T").Tables[0];
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var consumeMember = dr["MEMBER_ID"].ToString();
                    if(listCheck.Contains(consumeMember))
                    {
                        list.Add(consumeMember);
                    }
                    
                }
            }

            return list;
        }

        public bool InsertQBuy(
            string activeId,
            string activeQBuyId,
            string storeId,
            string memberId,
            double beforeStart,
            int lastDays
            )
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(
                TaskJobSqls.INSERT_QBUY_LIST,
                activeId,
                activeQBuyId,
                storeId,
                memberId,
                beforeStart,
                lastDays);
            string sql = builder.ToString();
            return DatabaseOperation.ExecuteDML(sql);
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
        public const string SELECT_CHECK_STORE_BY_STORE_AND_GROUP_MEMBER_FOR_CONSUME = ""
            + "SELECT MEMBER_ID,COUNT(*) "
            + "FROM T_BUSS_MEMBER_CHECK_STORE A "
            + "WHERE STORE_ID = {0} "
            + "AND DATE_FORMAT(CHECK_TIME,'%Y%m%d%H%i%s') BETWEEN "
            + "'{1}' AND '{2}' "
            + "AND CHECK_TIME > IFNULL((SELECT MAX(S.GET_TIME) FROM T_BUSS_QBUY S WHERE S.ACTIVE_ID = {5} AND S.MEMBER_ID = A.MEMBER_ID), 0) "
            + "AND CONSUME >= {3} "
            + "GROUP BY MEMBER_ID "
            + "HAVING COUNT(*) >= {4} ";
        public const string SELECT_CHECK_STORE_BY_STORE_AND_GROUP_MEMBER_FOR_CHECK = ""
            + "SELECT MEMBER_ID,COUNT(*) "
            + "FROM T_BUSS_MEMBER_CHECK_STORE A "
            + "WHERE STORE_ID = {0} "
            + "AND DATE_FORMAT(CHECK_TIME,'%Y%m%d%H%i%s') BETWEEN "
            + "'{1}' AND '{2}' "
            + "AND CHECK_TIME > IFNULL((SELECT MAX(S.GET_TIME) FROM T_BUSS_QBUY S WHERE S.ACTIVE_ID = {4} AND S.MEMBER_ID = A.MEMBER_ID), 0) "
            + "GROUP BY MEMBER_ID "
            + "HAVING COUNT(*) >= {3} ";
        public const string INSERT_QBUY_LIST = ""
            + "INSERT INTO T_BUSS_QBUY(ACTIVE_ID,ACTIVE_QBUY_ID,STORE_ID,MEMBER_ID,STATE,START_TIME,END_TIME,GET_TIME) "
            + "VALUES({0},{1},{2},{3},0,DATE_ADD(NOW(),INTERVAL {4} MINUTE),DATE_ADD(NOW(),INTERVAL {5} DAY),NOW())";
    }
}

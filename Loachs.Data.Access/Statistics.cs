using System;
using System.Collections.Generic;
using System.Data;
using Loachs.Entity;
using Mono.Data.Sqlite;

namespace Loachs.Data.Access
{
    public class Statistics : IStatistics
    {
        public bool UpdateStatistics(StatisticsInfo statistics)
        {
            string cmdText = @"update [loachs_sites] set 
                                PostCount=@PostCount,
                                CommentCount=@CommentCount,
                                VisitCount=@VisitCount,
                                TagCount=@TagCount";
            SqliteParameter[] prams =
            {
                SqliteDbHelper.MakeInParam("@PostCount", DbType.Int32, 4, statistics.PostCount),
                SqliteDbHelper.MakeInParam("@CommentCount", DbType.Int32, 4, statistics.CommentCount),
                SqliteDbHelper.MakeInParam("@VisitCount", DbType.Int32, 4, statistics.VisitCount),
                SqliteDbHelper.MakeInParam("@TagCount", DbType.Int32, 4, statistics.TagCount)
            };

            return SqliteDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams) == 1;
        }

        public StatisticsInfo GetStatistics()
        {
            string cmdText = "select  * from [loachs_sites] limit 1";

            string insertText =
                "insert into [loachs_sites] ([PostCount],[CommentCount],[VisitCount],[TagCount],[setting]) values ( '0','0','0','0','<?xml version=\"1.0\" encoding=\"utf-8\"?><SettingInfo xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"></SettingInfo>')";

            List<StatisticsInfo> list = DataReaderToList(SqliteDbHelper.ExecuteReader(cmdText));

            if (list.Count == 0)
            {
                SqliteDbHelper.ExecuteNonQuery(insertText);
            }
            list = DataReaderToList(SqliteDbHelper.ExecuteReader(cmdText));

            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        ///     转换实体
        /// </summary>
        /// <param name="read">OleDbDataReader</param>
        /// <returns>TermInfo</returns>
        private static List<StatisticsInfo> DataReaderToList(SqliteDataReader read)
        {
            List<StatisticsInfo> list = new List<StatisticsInfo>();
            while (read.Read())
            {
                StatisticsInfo site = new StatisticsInfo
                {
                    PostCount = Convert.ToInt32(read["PostCount"]),
                    CommentCount = Convert.ToInt32(read["CommentCount"]),
                    VisitCount = Convert.ToInt32(read["VisitCount"]),
                    TagCount = Convert.ToInt32(read["TagCount"])
                };

                list.Add(site);
            }
            read.Close();
            return list;
        }
    }
}
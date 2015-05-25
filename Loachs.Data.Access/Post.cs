using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Loachs.Common;
using Loachs.Entity;
using Mono.Data.Sqlite;

namespace Loachs.Data.Access
{
    public class Post : IPost
    {
        /// <summary>
        ///     添加
        /// </summary>
        /// <param name="postinfo">实体</param>
        /// <returns>成功返回新记录的ID,失败返回 0</returns>
        public int InsertPost(PostInfo postinfo)
        {
            CheckSlug(postinfo);
            string cmdText = @"insert into [loachs_posts]
                                (
                               [CategoryId],[Title],[Summary],[Content],[Slug],[UserId],[CommentStatus],[CommentCount],[ViewCount],[Tag],[UrlFormat],[Template],[Recommend],[Status],[TopStatus],[HideStatus],[CreateDate],[UpdateDate]
                                )
                                values
                                (
                                @CategoryId,@Title,@Summary,@Content,@Slug,@UserId,@CommentStatus,@CommentCount,@ViewCount,@Tag,@UrlFormat,@Template,@Recommend,@Status,@TopStatus,@HideStatus,@CreateDate,@UpdateDate
                                )";
            SqliteParameter[] prams =
            {
                SqliteDbHelper.MakeInParam("@CategoryId", DbType.Int32, 4, postinfo.CategoryId),
                SqliteDbHelper.MakeInParam("@Title", DbType.String, 255, postinfo.Title),
                SqliteDbHelper.MakeInParam("@Summary", DbType.String, 0, postinfo.Summary),
                SqliteDbHelper.MakeInParam("@Content", DbType.String, 0, postinfo.Content),
                SqliteDbHelper.MakeInParam("@Slug", DbType.String, 255, postinfo.Slug),
                SqliteDbHelper.MakeInParam("@UserId", DbType.Int32, 4, postinfo.UserId),
                SqliteDbHelper.MakeInParam("@CommentStatus", DbType.Int32, 1, postinfo.CommentStatus),
                SqliteDbHelper.MakeInParam("@CommentCount", DbType.Int32, 4, postinfo.CommentCount),
                SqliteDbHelper.MakeInParam("@ViewCount", DbType.Int32, 4, postinfo.ViewCount),
                SqliteDbHelper.MakeInParam("@Tag", DbType.String, 255, postinfo.Tag),
                SqliteDbHelper.MakeInParam("@UrlFormat", DbType.Int32, 1, postinfo.UrlFormat),
                SqliteDbHelper.MakeInParam("@Template", DbType.String, 50, postinfo.Template),
                SqliteDbHelper.MakeInParam("@Recommend", DbType.Int32, 1, postinfo.Recommend),
                SqliteDbHelper.MakeInParam("@Status", DbType.Int32, 1, postinfo.Status),
                SqliteDbHelper.MakeInParam("@TopStatus", DbType.Int32, 1, postinfo.TopStatus),
                SqliteDbHelper.MakeInParam("@HideStatus", DbType.Int32, 1, postinfo.HideStatus),
                SqliteDbHelper.MakeInParam("@CreateDate", DbType.Date, 8, postinfo.CreateDate),
                SqliteDbHelper.MakeInParam("@UpdateDate", DbType.Date, 8, postinfo.UpdateDate)
            };
            SqliteDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
            int newId =
                StringHelper.ObjectToInt(
                    SqliteDbHelper.ExecuteScalar("select   [PostId] from [Loachs_Posts] order by [PostId] desc limit 1"));
            //if (newId > 0)
            //{
            //    SqliteDbHelper.ExecuteNonQuery(string.Format("update [loachs_users] set [postcount]=[postcount]+1 where [userid]={0}", postinfo.UserId));
            //    SqliteDbHelper.ExecuteNonQuery("update [loachs_sites] set [postcount]=[postcount]+1");
            //    SqliteDbHelper.ExecuteNonQuery(string.Format("update [loachs_terms] set [count]=[count]+1 where [termid]={0}", postinfo.CategoryId));
            //}
            return newId;
        }

        /// <summary>
        ///     修改
        /// </summary>
        /// <param name="postinfo">实体</param>
        /// <returns>修改的行数</returns>
        public int UpdatePost(PostInfo postinfo)
        {
            CheckSlug(postinfo);

            //PostInfo oldPost = GetPost(postinfo.PostId);        //修改前

            //if (oldPost.CategoryId != postinfo.CategoryId)
            //{

            //    SqliteDbHelper.ExecuteNonQuery(string.Format("update [loachs_terms] set [count]=[count]-1 where [termid]={0}", oldPost.CategoryId));

            //    SqliteDbHelper.ExecuteNonQuery(string.Format("update [loachs_terms] set [count]=[count]+1 where [termid]={0}", postinfo.CategoryId));

            //}


            string cmdText =
                "update [loachs_posts] set  [CategoryId]=@CategoryId,[Title]=@Title,[Summary]=@Summary,[Content]=@Content,[Slug]=@Slug,[UserId]=@UserId,[CommentStatus]=@CommentStatus,[CommentCount]=@CommentCount,[ViewCount]=@ViewCount,[Tag]=@Tag,[UrlFormat]=@UrlFormat,[Template]=@Template,[Recommend]=@Recommend,[Status]=@Status,[TopStatus]=@TopStatus,[HideStatus]=@HideStatus,[CreateDate]=@CreateDate,[UpdateDate]=@UpdateDate where [PostId]=@PostId";
            SqliteParameter[] prams =
            {
                SqliteDbHelper.MakeInParam("@CategoryId", DbType.Int32, 4, postinfo.CategoryId),
                SqliteDbHelper.MakeInParam("@Title", DbType.String, 255, postinfo.Title),
                SqliteDbHelper.MakeInParam("@Summary", DbType.String, 0, postinfo.Summary),
                SqliteDbHelper.MakeInParam("@Content", DbType.String, 0, postinfo.Content),
                SqliteDbHelper.MakeInParam("@Slug", DbType.String, 255, postinfo.Slug),
                SqliteDbHelper.MakeInParam("@UserId", DbType.Int32, 4, postinfo.UserId),
                SqliteDbHelper.MakeInParam("@CommentStatus", DbType.Int32, 1, postinfo.CommentStatus),
                SqliteDbHelper.MakeInParam("@CommentCount", DbType.Int32, 4, postinfo.CommentCount),
                SqliteDbHelper.MakeInParam("@ViewCount", DbType.Int32, 4, postinfo.ViewCount),
                SqliteDbHelper.MakeInParam("@Tag", DbType.String, 255, postinfo.Tag),
                SqliteDbHelper.MakeInParam("@UrlFormat", DbType.Int32, 1, postinfo.UrlFormat),
                SqliteDbHelper.MakeInParam("@Template", DbType.String, 50, postinfo.Template),
                SqliteDbHelper.MakeInParam("@Recommend", DbType.Int32, 1, postinfo.Recommend),
                SqliteDbHelper.MakeInParam("@Status", DbType.Int32, 1, postinfo.Status),
                SqliteDbHelper.MakeInParam("@TopStatus", DbType.Int32, 1, postinfo.TopStatus),
                SqliteDbHelper.MakeInParam("@HideStatus", DbType.Int32, 1, postinfo.HideStatus),
                SqliteDbHelper.MakeInParam("@CreateDate", DbType.Date, 8, postinfo.CreateDate),
                SqliteDbHelper.MakeInParam("@UpdateDate", DbType.Date, 8, postinfo.UpdateDate),
                SqliteDbHelper.MakeInParam("@PostId", DbType.Int32, 4, postinfo.PostId)
            };
            return SqliteDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="postid">主键</param>
        /// <returns>删除的行数</returns>
        public int DeletePost(int postid)
        {
            PostInfo oldPost = GetPost(postid); //删除前

            string cmdText = "delete from [loachs_posts] where [PostId] = @PostId";
            SqliteParameter[] prams =
            {
                SqliteDbHelper.MakeInParam("@PostId", DbType.Int32, 4, postid)
            };
            int result = SqliteDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);


            //if (oldPost != null)
            //{
            //    SqliteDbHelper.ExecuteNonQuery(string.Format("update [loachs_users] set [postcount]=[postcount]-1 where [userid]={0}", oldPost.UserId));
            //    SqliteDbHelper.ExecuteNonQuery("update [loachs_sites] set [postcount]=[postcount]-1");
            //    SqliteDbHelper.ExecuteNonQuery(string.Format("update [loachs_terms] set [count]=[count]-1 where [termid]={0}", oldPost.CategoryId));
            //}

            return result;
        }

        /// <summary>
        ///     获取实体
        /// </summary>
        /// <param name="postid">主键</param>
        /// <returns></returns>
        public PostInfo GetPost(int postid)
        {
            string cmdText = "select  * from [loachs_posts] where [PostId] = @PostId limit 1";
            SqliteParameter[] prams =
            {
                SqliteDbHelper.MakeInParam("@PostId", DbType.Int32, 4, postid)
            };


            List<PostInfo> list = DataReaderToCommentList(SqliteDbHelper.ExecuteReader(cmdText, prams));

            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        ///     获取实体
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public PostInfo GetPost(string slug)
        {
            string cmdText = "select   * from [loachs_posts] where [slug] = @slug limit 1";
            SqliteParameter[] prams =
            {
                SqliteDbHelper.MakeInParam("@slug", DbType.String, 200, slug)
            };


            List<PostInfo> list = DataReaderToCommentList(SqliteDbHelper.ExecuteReader(cmdText, prams));

            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns>IList</returns>
        public List<PostInfo> GetPostList()
        {
            string cmdText = "select * from [loachs_posts] order by [postid] desc";
            return DataReaderToCommentList(SqliteDbHelper.ExecuteReader(cmdText));
        }

        public List<PostInfo> GetPostList(int pageSize, int pageIndex, out int recordCount, int categoryId, int tagId,
            int userId, int recommend, int status, int topstatus, int hidestatus, string begindate, string enddate,
            string keyword)
        {
            string condition = " 1=1 ";

            if (categoryId != -1)
            {
                condition += " and categoryId=" + categoryId;
            }
            if (tagId != -1)
            {
                condition += " and tag like '%{" + tagId + "}%'";
            }
            if (userId != -1)
            {
                condition += " and userid=" + userId;
            }
            if (recommend != -1)
            {
                condition += " and recommend=" + recommend;
            }
            if (status != -1)
            {
                condition += " and status=" + status;
            }

            if (topstatus != -1)
            {
                condition += " and topstatus=" + topstatus;
            }

            if (hidestatus != -1)
            {
                condition += " and hidestatus=" + hidestatus;
            }

            if (!string.IsNullOrEmpty(begindate))
            {
                condition += " and createdate>=datetime('" + begindate + "')";
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                condition += " and createdate<datetime('" + enddate + "')";
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                condition += string.Format(" and (summary like '%{0}%' or title like '%{0}%'  )", keyword);
            }

            string cmdTotalRecord = "select count(1) from [loachs_posts] where " + condition;

            //   throw new Exception(cmdTotalRecord);

            recordCount = StringHelper.ObjectToInt(SqliteDbHelper.ExecuteScalar(CommandType.Text, cmdTotalRecord));


            string cmdText = SqliteDbHelper.GetPageSql("[Loachs_Posts]", "[PostId]", "*", pageSize, pageIndex, 1,
                condition);


            return DataReaderToCommentList(SqliteDbHelper.ExecuteReader(cmdText));
        }

        public List<PostInfo> GetPostListByRelated(int postId, int rowCount)
        {
            string tags = string.Empty;

            PostInfo post = GetPost(postId);

            if (post != null && post.Tag.Length > 0)
            {
                tags = post.Tag;


                tags = tags.Replace("}", "},");
                string[] idList = tags.Split(',');

                string where = idList.Where(tagId => !string.IsNullOrEmpty(tagId)).Aggregate(" (", (current, tagId) => current + string.Format("  [tags] like '%{0}%' or ", tagId));
                where += " 1=2 ) and [status]=1 and [postid]<>" + postId;

                string cmdText =
                    string.Format("select  * from [loachs_posts] where {1} order by [postid] desc limit {0}", rowCount,
                        where);

                return DataReaderToCommentList(SqliteDbHelper.ExecuteReader(cmdText));
            }
            return new List<PostInfo>();
        }

        ///// <summary>
        ///// 根据别名获取文章ID
        ///// </summary>
        ///// <param name="slug"></param>
        ///// <returns></returns>
        //public int GetPostId(string slug)
        //{
        //    string cmdText = "select [postid] from [loachs_posts] where [slug]=@slug";
        //    SqliteParameter[] prams = {  
        //                           SqliteDbHelper.MakeInParam("@slug",DbType.String,200,slug),

        //                            };
        //    return StringHelper.ObjectToInt(SqliteDbHelper.ExecuteScalar(CommandType.Text, cmdText, prams));

        //}

        public List<ArchiveInfo> GetArchive()
        {
            string cmdText =
                "select strftime('%Y%m',createdate) as [date] ,  count(*) as [count] from [loachs_posts] where [status]=1 and [hidestatus]=0  group by  strftime('%Y%m',createdate)   order by strftime('%Y%m',createdate)  desc";

            List<ArchiveInfo> list = new List<ArchiveInfo>();
            using (SqliteDataReader read = SqliteDbHelper.ExecuteReader(cmdText))
            {
                while (read.Read())
                {
                    ArchiveInfo archive = new ArchiveInfo();
                    string date = read["date"].ToString().Substring(0, 4) + "-" +
                                  read["date"].ToString().Substring(4, 2);
                    archive.Date = Convert.ToDateTime(date);
                    // archive.Title = read["date"].ToString();
                    archive.Count = StringHelper.ObjectToInt(read["count"]);
                    list.Add(archive);
                }
            }
            return list;
        }

        public int UpdatePostViewCount(int postId, int addCount)
        {
            string cmdText = "update [loachs_posts] set [viewcount] = [viewcount] + @addcount where [postid]=@postid";
            SqliteParameter[] prams =
            {
                SqliteDbHelper.MakeInParam("@addcount", DbType.Int32, 4, addCount),
                SqliteDbHelper.MakeInParam("@postid", DbType.Int32, 4, postId)
            };
            return SqliteDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
        }

        /// <summary>
        ///     检查别名是否重复
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        private bool CheckSlug(PostInfo post)
        {
            if (string.IsNullOrEmpty(post.Slug))
            {
                return true;
            }
            while (true)
            {
                string cmdText = string.Empty;
                if (post.PostId == 0)
                {
                    cmdText = string.Format("select count(1) from [loachs_posts] where [slug]='{0}'  ", post.Slug);
                }
                else
                {
                    cmdText = string.Format(
                        "select count(1) from [loachs_posts] where [slug]='{0}'   and [postid]<>{1}", post.Slug,
                        post.PostId);
                }
                int r = Convert.ToInt32(SqliteDbHelper.ExecuteScalar(cmdText));
                if (r == 0)
                {
                    return true;
                }
                post.Slug += "-2";
            }
        }

        /// <summary>
        ///     数据转换
        /// </summary>
        /// <param name="read"></param>
        /// <returns></returns>
        private List<PostInfo> DataReaderToCommentList(SqliteDataReader read)
        {
            List<PostInfo> list = new List<PostInfo>();
            while (read.Read())
            {
                PostInfo postinfo = new PostInfo
                {
                    PostId = StringHelper.ObjectToInt(read["PostId"]),
                    CategoryId = StringHelper.ObjectToInt(read["CategoryId"]),
                    Title = Convert.ToString(read["Title"]),
                    Summary = Convert.ToString(read["Summary"]),
                    Content = Convert.ToString(read["Content"]),
                    Slug = Convert.ToString(read["Slug"]),
                    UserId = StringHelper.ObjectToInt(read["UserId"]),
                    CommentStatus = StringHelper.ObjectToInt(read["CommentStatus"]),
                    CommentCount = StringHelper.ObjectToInt(read["CommentCount"]),
                    ViewCount = StringHelper.ObjectToInt(read["ViewCount"]),
                    Tag = Convert.ToString(read["Tag"]),
                    UrlFormat = StringHelper.ObjectToInt((read["UrlFormat"])),
                    Template = Convert.ToString(read["Template"]),
                    Recommend = StringHelper.ObjectToInt(read["Recommend"]),
                    Status = StringHelper.ObjectToInt(read["Status"]),
                    TopStatus = StringHelper.ObjectToInt(read["TopStatus"]),
                    HideStatus = StringHelper.ObjectToInt(read["HideStatus"]),
                    CreateDate = Convert.ToDateTime(read["CreateDate"]),
                    UpdateDate = Convert.ToDateTime(read["UpdateDate"])
                };

                list.Add(postinfo);
            }
            read.Close();
            return list;
        }
    }
}
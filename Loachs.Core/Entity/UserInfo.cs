using System;
using Loachs.Business;
using Loachs.Common;

namespace Loachs.Entity
{
    /// <summary>
    ///     用户实体
    /// </summary>
    public class UserInfo : IComparable<UserInfo>
    {
        /// <summary>
        ///     用户ID
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        ///     用户类型
        /// </summary>
        public int Type { set; get; }

        /// <summary>
        ///     用户账号
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        ///     显示名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        ///     密码
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        ///     邮箱
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        ///     个人网站
        /// </summary>
        public string SiteUrl { set; get; }

        /// <summary>
        ///     头像地址
        /// </summary>
        public string AvatarUrl { set; get; }

        /// <summary>
        ///     描述
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        ///     用户状态
        ///     1:使用,0:停用
        /// </summary>
        public int Status { set; get; }

        /// <summary>
        ///     统计日志数
        /// </summary>
        public int PostCount { set; get; }

        /// <summary>
        ///     评论数
        /// </summary>
        public int CommentCount { set; get; }

        /// <summary>
        ///     排序
        /// </summary>
        public int Displayorder { set; get; }

        /// <summary>
        ///     创建日期
        /// </summary>
        public DateTime CreateDate { set; get; }

        public int CompareTo(UserInfo other)
        {
            if (Displayorder != other.Displayorder)
            {
                return Displayorder.CompareTo(other.Displayorder);
            }
            return UserId.CompareTo(other.UserId);
        }

        #region 非字段
        
        /// <summary>
        ///     地址
        /// </summary>
        public string Url
        {
            get
            {
                string url = string.Empty;

                if (Utils.IsSupportUrlRewriter == false)
                {
                    url = string.Format("{0}default.aspx?type=author&username={1}", ConfigHelper.SiteUrl,
                        StringHelper.UrlEncode(UserName));
                }
                else
                {
                    return ConfigHelper.SiteUrl + "author/" + StringHelper.UrlEncode(UserName) +
                           SettingManager.GetSetting().RewriteExtension;
                }
                return Utils.CheckPreviewThemeUrl(url);
            }
        }

        /// <summary>
        ///     连接
        /// </summary>
        public string Link
        {
            get { return string.Format("<a href=\"{0}\" title=\"作者:{1}\">{1}</a>", Url, Name); }
        }

        #endregion
    }
}
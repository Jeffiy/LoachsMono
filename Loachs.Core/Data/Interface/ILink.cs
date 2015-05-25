using System.Collections.Generic;
using Loachs.Entity;

namespace Loachs.Data
{
    /// <summary>
    ///     连接接口
    /// </summary>
    public interface ILink
    {
        /// <summary>
        ///     添加连接
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        int InsertLink(LinkInfo link);

        /// <summary>
        ///     修改连接
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        int UpdateLink(LinkInfo link);

        /// <summary>
        ///     删除连接
        /// </summary>
        /// <param name="linkId"></param>
        /// <returns></returns>
        int DeleteLink(int linkId);

        //        LinkInfo GetLink(int linkId);

        /// <summary>
        ///     获取全部连接
        /// </summary>
        /// <returns></returns>
        List<LinkInfo> GetLinkList();
    }
}
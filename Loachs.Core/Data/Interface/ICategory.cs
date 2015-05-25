using System.Collections.Generic;
using Loachs.Entity;

namespace Loachs.Data
{
    /// <summary>
    ///     分类接口
    /// </summary>
    public interface ICategory
    {
        /// <summary>
        ///     添加分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        int InsertCategory(CategoryInfo category);

        /// <summary>
        ///     修改分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        int UpdateCategory(CategoryInfo category);

        /// <summary>
        ///     删除分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        int DeleteCategory(int categoryId);

        /// <summary>
        ///     获取分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        CategoryInfo GetCategory(int categoryId);

        /// <summary>
        ///     获取所有分类
        /// </summary>
        /// <returns></returns>
        List<CategoryInfo> GetCategoryList();
    }
}
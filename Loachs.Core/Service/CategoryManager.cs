using System.Collections.Generic;
using System.Linq;
using Loachs.Data;
using Loachs.Entity;

namespace Loachs.Business
{
    /// <summary>
    ///     分类管理
    /// </summary>
    public static class CategoryManager
    {
        private static readonly ICategory Dao = DataAccess.CreateCategory();

        /// <summary>
        ///     分类列表
        /// </summary>
        private static List<CategoryInfo> _categories;

        /// <summary>
        ///     lock
        /// </summary>
        private static readonly object LockHelper = new object();

        static CategoryManager()
        {
            LoadCategory();
        }

        /// <summary>
        ///     初始化
        /// </summary>
        public static void LoadCategory()
        {
            if (_categories == null)
            {
                lock (LockHelper)
                {
                    if (_categories == null)
                    {
                        _categories = Dao.GetCategoryList();
                    }
                }
            }
        }

        /// <summary>
        ///     添加分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static int InsertCategory(CategoryInfo category)
        {
            int categoryId = Dao.InsertCategory(category);
            category.CategoryId = categoryId;

            _categories.Add(category);
            _categories.Sort();

            return categoryId;
        }

        /// <summary>
        ///     修改分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static int UpdateCategory(CategoryInfo category)
        {
            _categories.Sort();
            return Dao.UpdateCategory(category);
        }

        /// <summary>
        ///     更新文章数
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static int UpdateCategoryCount(int categoryId, int addCount)
        {
            if (categoryId == 0)
            {
                return 0;
            }

            CategoryInfo category = GetCategory(categoryId);
            if (category != null)
            {
                category.Count += addCount;

                return UpdateCategory(category);
            }
            return 0;
        }

        /// <summary>
        ///     删除分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static int DeleteCategory(int categoryId)
        {
            _categories.RemoveAll(cate => cate.CategoryId == categoryId);

            return Dao.DeleteCategory(categoryId);
        }

        /// <summary>
        ///     获取分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static CategoryInfo GetCategory(int categoryId)
        {
            return _categories.FirstOrDefault(t => t.CategoryId == categoryId);
        }

        /// <summary>
        ///     获取分类
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public static CategoryInfo GetCategory(string slug)
        {
            return _categories.FirstOrDefault(t => !string.IsNullOrEmpty(slug) && t.Slug.ToLower() == slug.ToLower());
        }

        /// <summary>
        ///     获取分类ID
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public static int GetCategoryId(string slug)
        {
            foreach (CategoryInfo t in _categories.Where(t => !string.IsNullOrEmpty(slug) && t.Slug.ToLower() == slug.ToLower()))
            {
                return t.CategoryId;
            }
            return -1;
        }

        /// <summary>
        ///     获取分类名称
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static string GetCategoryName(int categoryId)
        {
            foreach (CategoryInfo t in _categories.Where(t => t.CategoryId == categoryId))
            {
                return t.Name;
            }
            return string.Empty;
        }

        /// <summary>
        ///     获取全部分类
        /// </summary>
        /// <returns></returns>
        public static List<CategoryInfo> GetCategoryList()
        {
            return _categories;
        }
    }
}
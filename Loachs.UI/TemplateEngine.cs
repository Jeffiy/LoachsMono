using System.IO;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using NVelocity.Runtime;

namespace Loachs.Web
{
    /// <summary>
    ///     模板封装类
    /// </summary>
    public class TemplateHelper
    {
        private readonly IContext _context;
        private readonly VelocityEngine _velocity;

        public TemplateHelper(string templatePath)
        {
            _velocity = new VelocityEngine();

            //使用设置初始化VelocityEngine
            ExtendedProperties props = new ExtendedProperties();

            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, templatePath);
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");

            //   props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "gb2312");
            //    props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");

            //  props.SetProperty(RuntimeConstants.RESOURCE_MANAGER_CLASS, "NVelocity.Runtime.Resource.ResourceManagerImpl\\,NVelocity");

            _velocity.Init(props);
            //RuntimeConstants.RESOURCE_MANAGER_CLASS 
            //为模板变量赋值
            _context = new VelocityContext();
        }

        /// <summary>
        ///     给模板变量赋值
        /// </summary>
        /// <param name="key">模板变量</param>
        /// <param name="value">模板变量值</param>
        public void Put(string key, object value)
        {
            _context.Put(key, value);
        }

        /// <summary>
        ///     生成字符
        /// </summary>
        /// <param name="templateFile">模板文件名</param>
        public string BuildString(string templateFile)
        {
            //从文件中读取模板
            Template template = _velocity.GetTemplate(templateFile);

            //合并模板
            StringWriter writer = new StringWriter();
            template.Merge(_context, writer);
            return writer.ToString();
        }
    }
}
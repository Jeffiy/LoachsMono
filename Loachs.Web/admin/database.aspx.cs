using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Loachs.Common;

namespace Loachs.Web
{
    public partial class admin_database : AdminPage
    {
        /// <summary>
        ///     备份路径
        /// </summary>
        protected string BackupPath = ConfigHelper.SitePath + "app_data/backup/";

        /// <summary>
        ///     当前数据库信息
        /// </summary>
        protected FileInfo DbInfo;

        /// <summary>
        ///     数据库路径
        /// </summary>
        protected string DbPath;

        /// <summary>
        ///     数据库大小
        /// </summary>
        protected string DbSize;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("数据管理");

            if (!Directory.Exists(Server.MapPath(BackupPath)))
            {
                Directory.CreateDirectory(Server.MapPath(BackupPath));
            }

            //        DbPath = Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection);
            //   System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection));
            //   DbSize = PageUtils.ConvertUnit(file.Length);
            DbInfo = new FileInfo(Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection));


            string name = RequestHelper.QueryString("name");
            switch (OperateString)
            {
                //下载
                case "down":

                    FileInfo downFile = new FileInfo(Server.MapPath(BackupPath + name));
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + downFile.Name);
                    Response.TransmitFile(downFile.FullName);
                    Response.End();
                    break;
                //删除
                case "delete":

                    File.Delete(Server.MapPath(BackupPath + name));
                    Response.Redirect("database.aspx?result=3");
                    break;
                //还原
                case "restore":

                    File.Copy(Server.MapPath(BackupPath + name), DbInfo.FullName, true);
                    Response.Redirect("database.aspx?result=4");
                    break;
                default:
                    break;
            }

            ShowResult();
        }

        /// <summary>
        ///     显示结果
        /// </summary>
        protected void ShowResult()
        {
            int result = RequestHelper.QueryInt("result");
            switch (result)
            {
                case 3:
                    ShowMessage("备份删除成功!");
                    break;
                case 4:
                    ShowMessage("还原成功!");
                    break;
                case 5:
                    ShowMessage("数据库压缩成功!");
                    break;
                default:
                    break;
            }
        }

        ///// <summary>
        ///// 压缩数据库
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnCompact2_Click(object sender, EventArgs e)
        //{
        //    string DbPath1, DbPath2, DbConn1, DbConn2;

        //    DbPath1 = Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection);// Server.MapPath("../App_Data/DataBase.mdb");//原数据库路径
        //    DbPath2 = Server.MapPath(ConfigHelper.SitePath + "/app_data/sdfskfg32523.mdb");// Server.MapPath("../App_Data/DataBase2.mdb");//压缩后的数据库路径
        //    DbConn1 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DbPath1;
        //    DbConn2 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DbPath2;

        //    try
        //    {
        //        JetEngine DatabaseEngin = new JetEngine();
        //        DatabaseEngin.CompactDatabase(DbConn1, DbConn2);//压缩

        //        File.Copy(DbPath2, DbPath1, true);//将压缩后的数据库覆盖原数据库
        //        File.Delete(DbPath2);//删除压缩后的数据库
        //        Response.Redirect("database.aspx?result=5");
        //    }
        //    catch
        //    {
        //        //   Response.Write("数据库压缩失败，请重试!");
        //    }
        //}

        protected void btnCompact_Click(object sender, EventArgs e)
        {
            string datapath = Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection);
            string connectStr =
                "Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Registry Path=;Jet OLEDB:Database Locking Mode=0;Data Source=" +
                datapath +
                ";Password=;Jet OLEDB:Engine Type=5;Jet OLEDB:Global Bulk Transactions=1;Provider=\"Microsoft.Jet.OLEDB.4.0\";Jet OLEDB:System database=;Jet OLEDB:SFP=False;Extended Properties=;Mode=Share Deny None;Jet OLEDB:New Database Password=;Jet OLEDB:Create System Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;Jet OLEDB:Encrypt Database=False";

            CompactAccessDB(connectStr, datapath);

            Response.Redirect("database.aspx?result=5");
        }

        /// <summary>
        ///     压缩数据库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="mdwfilename"></param>
        public void CompactAccessDB(string connectionString, string mdwfilename)
        {
            string tempDbPath = Server.MapPath(ConfigHelper.SitePath + "/app_data/temp_sdfskfg32523.mdb"); //临时

            object objJRO =
                Activator.CreateInstance(Type.GetTypeFromProgID("JRO.JetEngine"));

            object[] oParams = new object[]
            {
                connectionString,
                "Provider=Microsoft.Jet.OLEDB.4.0;Data" + " Source=" + tempDbPath + ";Jet OLEDB:Engine Type=5"
            };

            objJRO.GetType().InvokeMember("CompactDatabase", BindingFlags.InvokeMethod, null, objJRO, oParams);

            File.Delete(mdwfilename);
            File.Move(tempDbPath, mdwfilename);

            Marshal.ReleaseComObject(objJRO);
            objJRO = null;
        }

        protected void btnBak_Click(object sender, EventArgs e)
        {
            string bakDir = Server.MapPath(BackupPath);
            if (!Directory.Exists(bakDir))
            {
                Directory.CreateDirectory(bakDir);
            }
            string destFileName = bakDir + "backup_" + DateTime.Now.ToString("yyyyMMddHHmmss_") + DbInfo.Name;
                // Path.Combine(Path.GetDirectoryName(dbFilePath), DateTime.Now.ToString("yyMMdd-HHmmss.db3"));
            File.Copy(DbInfo.FullName, destFileName, true);

            ShowMessage("备份完成");
        }
    }
}
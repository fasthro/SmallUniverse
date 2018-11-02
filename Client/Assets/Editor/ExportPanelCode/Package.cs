using SmallUniverse.Utils;
using System.Collections.Generic;
using System.IO;

namespace SmallUniverse.GameEditor.ExportPanelCode
{
    public class Package
    {
        // 工程内所有包的路径<id, PackageInfo>索引
        private Dictionary<string, PackageInfo> pDic = new Dictionary<string, PackageInfo>();

        public Package()
        {
            var packages = Directory.GetDirectories(PathUtils.UIEditorPath() + "assets/", "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < packages.Length; i++)
            {
                PackageInfo info = new PackageInfo(packages[i]);
                pDic.Add(info.id, info);
            }
        }

        /// <summary>
        /// 获取包内资源路径
        /// </summary>
        /// <param name="pkgId"> 包Id </param>
        /// <param name="src"> 资源id </param>
        /// <returns></returns>
        public string GetSrcPath(string pkgId, string src)
        {
            if (pDic.ContainsKey(pkgId))
            {
                return pDic[pkgId].GetSrcPath(src);
            }
            return "";
        }
    }
}
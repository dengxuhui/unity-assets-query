using System;
using System.Collections.Generic;
using System.IO;
using AssetsQuery.Scripts.tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AssetsQuery.Scripts.func
{
    /// <summary>
    /// 搜索未使用的图片
    /// </summary>
    internal static class FuncFindUnusedIMG
    {
        private static void FilterFromPrefabs(ref Dictionary<string, bool> allImgGuidDic)
        {
            var rules = AssetsQueryAssetManager.GetRules();
            var rootDir = FileTool.GetFullPath(rules.prefabRootDirectoryData.directoryPath,
                rules.prefabRootDirectoryData.relativeType);
            var uiFiles = Directory.GetFiles(rootDir, "*.prefab", SearchOption.AllDirectories);
            var cloneDic = new Dictionary<string, bool>(allImgGuidDic);
            for (var i = 0; i < uiFiles.Length; i++)
            {
                var filePath = uiFiles[i];
                if (string.IsNullOrEmpty(filePath)) continue;
                string metaContent = File.ReadAllText(filePath);
                foreach (var _ in cloneDic)
                {
                    allImgGuidDic.TryGetValue(_.Key, out var exist);
                    if (exist) continue;
                    if (metaContent.IndexOf(_.Key, StringComparison.Ordinal) > 0)
                    {
                        allImgGuidDic[_.Key] = true;
                    }
                }
            }
        }


        private static Dictionary<string, bool> CollectAllIMGAssets()
        {
            var dic = new Dictionary<string, bool>();

            var rules = AssetsQueryAssetManager.GetRules();
            var iData = rules.imageRootDirectoryData;
            var rootDir = FileTool.GetFullPath(iData.directoryPath, iData.relativeType);
            if (!Directory.Exists(rootDir))
            {
                Debug.LogError($"directory is not exist:{rootDir}");
                return dic;
            }

            var allFiles = Directory.GetFiles(rootDir);
            for (var i = 0; i < allFiles.Length; i++)
            {
                var path = allFiles[i];
                var extension = Path.GetExtension(path);
                if (string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    path = path.Replace(Application.dataPath, "Assets");
                    var guid = AssetDatabase.AssetPathToGUID(path);
                    if (!string.IsNullOrEmpty(guid))
                    {
                        dic.Add(guid, false);
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 搜寻没有使用的图片
        /// </summary>
        public static void Start()
        {
            var allImgGuidDic = CollectAllIMGAssets();
            FilterFromPrefabs(ref allImgGuidDic);
            Debug.Log("over");
        }
    }
}
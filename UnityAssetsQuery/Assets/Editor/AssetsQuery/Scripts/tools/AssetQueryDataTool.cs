// ------------
// -author:AER-
// ------------


using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace AssetsQuery.Scripts.tools
{
    /// <summary>
    /// 数据更新工具
    /// </summary>
    internal static class AssetQueryDataTool
    {
        /// <summary>
        /// 整理图片白名单数据 删除无效文件
        /// </summary>
        internal static void OrganizeIMGWhiteList()
        {
            var rules = AssetsQueryAssetManager.GetRules();
            var rootDir = FileTool.GetFullPath(rules.imageRootDirectoryData.directoryPath,
                rules.imageRootDirectoryData.relativeType);
            if (!Directory.Exists(rootDir))
            {
                return;
            }

            var whiteArray = rules.imgWhiteList;
            if (whiteArray.Length <= 0)
            {
                return;
            }

            var deleteIndexList = new List<int>();
            //去重列表
            var pathList = new List<string>();
            for (var i = 0; i < whiteArray.Length; i++)
            {
                if (pathList.IndexOf(whiteArray[i].path) > 0)
                {
                    deleteIndexList.Add(i);
                }
                else if (whiteArray[i].type == WhiteListType.File)
                {
                    var fullPath = Path.Combine(rootDir, whiteArray[i].path);
                    if (!File.Exists(fullPath))
                    {
                        deleteIndexList.Add(i);
                    }
                }

                pathList.Add(whiteArray[i].path);
            }

            if (deleteIndexList.Count <= 0)
            {
                return;
            }

            var index = 0;
            var newWhiteArray = new WhiteListData[whiteArray.Length - deleteIndexList.Count];
            for (var i = 0; i < newWhiteArray.Length; i++)
            {
                while (deleteIndexList.IndexOf(index) > 0)
                {
                    index++;
                }

                if (index >= whiteArray.Length)
                {
                    break;
                }

                newWhiteArray[i] = whiteArray[index];
                index++;
            }

            rules.imgWhiteList = newWhiteArray;
            EditorUtility.SetDirty(rules);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 通过index移除白名单
        /// </summary>
        /// <param name="removeIndex"></param>
        internal static void RemoveFromWhiteListByIndex(int removeIndex)
        {
            var rules = AssetsQueryAssetManager.GetRules();
            var rootDir = FileTool.GetFullPath(rules.imageRootDirectoryData.directoryPath,
                rules.imageRootDirectoryData.relativeType);
            if (!Directory.Exists(rootDir))
            {
                return;
            }

            if (removeIndex >= rules.imgWhiteList.Length)
            {
                return;
            }

            var oldArray = rules.imgWhiteList;
            var data = oldArray[removeIndex];
            if (data.type == WhiteListType.Directory)
            {
                Debug.LogError(
                    "can not remove directory type white list by this way,if you want remove it,please remove at AssetsQueryRule.asset");
                return;
            }

            var newArray = new WhiteListData[oldArray.Length - 1];
            var index = 0;
            for (var i = 0; i < newArray.Length; i++)
            {
                if (index == removeIndex)
                {
                    index++;
                }

                if (index >= newArray.Length)
                {
                    break;
                }

                newArray[i] = oldArray[index];
                index++;
            }

            rules.imgWhiteList = newArray;
            EditorUtility.SetDirty(rules);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 通过路径添加文件到白名单
        /// </summary>
        /// <param name="relativeAssetsPath"></param>
        internal static bool AddImageFileToWhiteListByPath(string relativeAssetsPath)
        {
            if (string.IsNullOrEmpty(relativeAssetsPath))
            {
                return false;
            }

            var fullPath = FileTool.GetFullPath(relativeAssetsPath, RelativeType.Project);
            if (!File.Exists(fullPath))
            {
                return false;
            }

            var rules = AssetsQueryAssetManager.GetRules();
            var imgDir = rules.imageRootDirectoryData;
            var fullImgDir = FileTool.GetFullPath(imgDir.directoryPath, imgDir.relativeType);
            fullImgDir = fullImgDir.Replace(Application.dataPath, "Assets");
            fullImgDir = FileTool.ConvertSlash(fullImgDir);
            relativeAssetsPath = FileTool.ConvertSlash(relativeAssetsPath);
            var relativeAtlasPath = relativeAssetsPath.Replace(fullImgDir, "");

            if (relativeAtlasPath.IndexOf("/", StringComparison.Ordinal) == 0)
            {
                relativeAtlasPath = relativeAtlasPath.Remove(0, 1);
            }
            if (relativeAtlasPath.LastIndexOf("/", StringComparison.Ordinal) == relativeAtlasPath.Length - 1)
            {
                relativeAtlasPath = relativeAtlasPath.Remove(relativeAtlasPath.Length - 1,1);
            }
            var data = new WhiteListData { type = WhiteListType.File,path = relativeAtlasPath};
            var oldArray = rules.imgWhiteList;
            var list = new List<WhiteListData>(oldArray) {data};
            var newArray = list.ToArray();
            rules.imgWhiteList = newArray;
            EditorUtility.SetDirty(rules);
            AssetDatabase.SaveAssets();
            return true;
        }
    }
}
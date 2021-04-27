using System;
using System.Collections.Generic;
using System.IO;
using AssetsQuery.Scripts.MultiLanguage;
using AssetsQuery.Scripts.tools;
using AssetsQuery.Scripts.window;
using UnityEditor;
using UnityEngine;

// ReSharper disable All

namespace AssetsQuery.Scripts.func
{
    /// <summary>
    /// 搜索未使用的图片
    /// </summary>
    internal static class FuncFindUnusedIMG
    {
        private static void FilterFromPrefabs(ref Dictionary<string, bool> allImgGuidDic)
        {
            ShowProgress("开始查询prefabs", 0.0f);
            var rules = AssetsQueryAssetManager.GetRules();
            var rootDir = FileTool.GetFullPath(rules.prefabRootDirectoryData.directoryPath,
                rules.prefabRootDirectoryData.relativeType);
            var uiFiles = Directory.GetFiles(rootDir, "*.prefab", SearchOption.AllDirectories);
            var cloneDic = new Dictionary<string, bool>(allImgGuidDic);
            if (uiFiles.Length <= 0)
            {
                return;
            }

            for (var i = 0; i < uiFiles.Length; i++)
            {
                var filePath = uiFiles[i];
                ShowProgress($"查询Prefab:{filePath}", (float) i / uiFiles.Length);
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

        
        /// <summary>
        /// 从运行时监听器过滤数据
        /// </summary>
        /// <param name="allImgGuidDic"></param>
        private static void FilterFromMonitorDatas(ref Dictionary<string, bool> allImgGuidDic)
        {
            var cloneDic = new Dictionary<string, bool>(allImgGuidDic);
            var strData = EditorPrefs.GetString(AssetsQueryGlobalConst.ImageUseMonitorSaveKey);
            if (string.IsNullOrEmpty(strData))
            {
                return;
            }
            //guid存储池
            var saveArray = strData.Split(',');
            Debug.Log("save array:" + strData);
        }

        /// <summary>
        /// 搜集所有图片资源，排除白名单中的资源
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, bool> CollectAllIMGAssets()
        {
            var dic = new Dictionary<string, bool>();

            var rules = AssetsQueryAssetManager.GetRules();
            var iData = rules.imageRootDirectoryData;
            var rootDir = FileTool.GetFullPath(iData.directoryPath, iData.relativeType);
            rootDir = FileTool.ConvertSlash(rootDir);
            if (!Directory.Exists(rootDir))
            {
                Debug.LogError($"directory is not exist:{rootDir}");
                return dic;
            }

            var allFiles = Directory.GetFiles(rootDir, "*", SearchOption.AllDirectories);
            if (allFiles.Length <= 0)
            {
                return dic;
            }

            var whiteList = rules.imgWhiteList;
            var fileWhiteList = new List<string>();
            var dirWhiteList = new List<string>();
            for (var i = 0; i < whiteList.Length; i++)
            {
                var t = whiteList[i];
                var p = Path.Combine(rootDir, t.path);
                p = FileTool.ConvertSlash(p);
                if (t.type == WhiteListType.Directory)
                {
                    if (p.LastIndexOf("/") == p.Length - 1)
                    {
                        p = p.Remove(p.Length - 1);
                    }

                    dirWhiteList.Add(p);
                }
                else if (t.type == WhiteListType.File)
                {
                    fileWhiteList.Add(p);
                }
            }

            for (var i = 0; i < allFiles.Length; i++)
            {
                var imgPath = allFiles[i];
                ShowProgress($"查询路径:{imgPath}", (float) i / allFiles.Length);
                var extension = Path.GetExtension(imgPath);
                var imgDir = Path.GetDirectoryName(imgPath);
                if (string.IsNullOrEmpty(imgDir))
                {
                    continue;
                }

                imgPath = FileTool.ConvertSlash(imgPath);
                imgDir = FileTool.ConvertSlash(imgDir);
                var valid = true;
                for (var i1 = 0; i1 < fileWhiteList.Count; i1++)
                {
                    if (imgPath == fileWhiteList[i1])
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                {
                    continue;
                }

                for (var i1 = 0; i1 < dirWhiteList.Count; i1++)
                {
                    if (imgDir.IndexOf(dirWhiteList[i1], StringComparison.Ordinal) >= 0)
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                {
                    continue;
                }


                if (string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    imgPath = imgPath.Replace(Application.dataPath, "Assets");
                    var guid = AssetDatabase.AssetPathToGUID(imgPath);
                    if (!string.IsNullOrEmpty(guid))
                    {
                        dic.Add(guid, false);
                    }
                }
            }

            return dic;
        }

        #region 进度

        private static void ShowProgress(string content, float progress)
        {
            EditorUtility.DisplayProgressBar(LanguageMgr.Read("find_unused_progress_title"), content, progress);
        }

        private static void HideProgress()
        {
            EditorUtility.ClearProgressBar();
        }

        #endregion

        /// <summary>
        /// 展示结果
        /// </summary>
        /// <param name="guidDic"></param>
        private static void DisplayResult(ref Dictionary<string, bool> guidDic)
        {
            //未使用guid列表
            var unusedList = new List<string>();
            foreach (var kv in guidDic)
            {
                if (!kv.Value)
                {
                    unusedList.Add(kv.Key);
                }
            }

            if (unusedList.Count <= 0)
            {
                EditorUtility.DisplayDialog(LanguageMgr.Read("img_query_result"),
                    LanguageMgr.Read("not_exist_unused_img_dialog_window"), "OK");
                return;
            }

            IMGUnusedQueryResultWindow.ShowWindow(unusedList);
        }

        /// <summary>
        /// 搜寻没有使用的图片
        /// </summary>
        public static void Start()
        {
            ShowProgress("开始..", 0.0f);
            var allImgGuidDic = CollectAllIMGAssets();
            //从prefab中删除
            FilterFromPrefabs(ref allImgGuidDic);
            FilterFromMonitorDatas(ref allImgGuidDic);
            HideProgress();
            DisplayResult(ref allImgGuidDic);
        }
    }
}
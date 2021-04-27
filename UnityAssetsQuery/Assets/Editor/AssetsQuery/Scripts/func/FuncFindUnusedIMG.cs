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
            if (allFiles.Length <= 0)
            {
                return dic;
            }
            for (var i = 0; i < allFiles.Length; i++)
            {
                var path = allFiles[i];
                ShowProgress($"查询路径:{path}", (float) i / allFiles.Length);
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
            FilterFromPrefabs(ref allImgGuidDic);
            HideProgress();
            DisplayResult(ref allImgGuidDic);
        }
    }
}
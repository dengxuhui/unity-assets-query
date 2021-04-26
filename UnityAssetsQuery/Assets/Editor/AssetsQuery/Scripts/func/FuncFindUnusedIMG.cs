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
        /// <summary>
        /// 从xlsx中查询图片引用
        /// </summary>
        /// <param name="fullDirPath"></param>
        /// <param name="usingList"></param>
        private static void CollectFromXlsx(string fullDirPath, ref Dictionary<string, string> usingDic)
        {
            if (!Directory.Exists(fullDirPath))
            {
                Debug.LogError($"directory is not exist:{fullDirPath}");
                return;
            }
        }

        /// <summary>
        /// 从lua文件中查找图片引用
        /// </summary>
        /// <param name="fullDirPath"></param>
        /// <param name="usingList"></param>
        private static void CollectFromLua(string fullDirPath, ref Dictionary<string, string> usingDic)
        {
            if (!Directory.Exists(fullDirPath))
            {
                Debug.LogError($"directory is not exist:{fullDirPath}");
                return;
            }
        }

        private static void CollectFromPrefab(string fullDirPath, ref Dictionary<string, string> usingDic)
        {
            if (!Directory.Exists(fullDirPath))
            {
                Debug.LogError($"directory is not exist:{fullDirPath}");
                return;
            }

            var uiFiles = Directory.GetFiles(fullDirPath, "*.prefab", SearchOption.AllDirectories);
            for (var i = 0; i < uiFiles.Length; i++)
            {
                var filePath = uiFiles[i];
                if (string.IsNullOrEmpty(filePath)) continue;
                filePath = filePath.Replace("\\", "/");
                var szBuildFileSrc = filePath.Replace(Application.dataPath, "Assets");
                var gameObject = AssetDatabase.LoadAssetAtPath(szBuildFileSrc, typeof(object)) as GameObject;
                if (gameObject == null) continue;
                var imgArray = gameObject.GetComponentsInChildren<Image>(true);
                var rawArray = gameObject.GetComponentsInChildren<RawImage>(true);
                for (var i1 = 0; i1 < imgArray.Length; i1++)
                {
                    var img = imgArray[i1];
                    if (img.sprite == null) continue;
                    var path = AssetDatabase.GetAssetPath(img.sprite);
                    if (string.IsNullOrEmpty(path) || usingDic.ContainsKey(path)) continue;
                    usingDic.Add(path, path);
                }

                for (var i1 = 0; i1 < rawArray.Length; i1++)
                {
                    var raw = rawArray[i1];
                    if (raw.texture == null) continue;
                    var path = AssetDatabase.GetAssetPath(raw.texture);
                    if (string.IsNullOrEmpty(path) || usingDic.ContainsKey(path)) continue;
                    usingDic.Add(path, path);
                }
            }

            Debug.Log("Prefab collect over..");
        }

        /// <summary>
        /// 搜寻没有使用的图片
        /// </summary>
        public static void Start()
        {
            var rules = AssetsQueryAssetManager.GetRules();
            var queryArray = rules.QueryDatas;
            //存在的是全路径
            var existingDic = new Dictionary<string, bool>();
            var otherUsingDic = new Dictionary<string, string>();
            //prefab中的引用可以直接查询到全路径
            var prefabUsingDic = new Dictionary<string, string>();
            //当前存在
            for (var i = 0; i < queryArray.Length; i++)
            {
                var queryData = queryArray[i];
                var imgRootData = queryData.imageRootDirectory;
                var imgDir = FileTool.GetFullPath(imgRootData.directoryPath, imgRootData.relativeType);
                if (!Directory.Exists(imgDir))
                {
                    Debug.LogError($"directory is not exist:{imgDir}");
                    continue;
                }

                var allFiles = Directory.GetFiles(imgDir);
                for (var i1 = 0; i1 < allFiles.Length; i1++)
                {
                    var path = allFiles[i1];
                    var extension = Path.GetExtension(path);
                    if (string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase))
                    {
                        path = path.Replace("\\", "/");
                        var szBuildFileSrc = path.Replace(Application.dataPath, "Assets");
                        existingDic.Add(szBuildFileSrc, false);
                    }
                }
                var refQueryDatas = queryData.queryDatas;
                for (var i1 = 0; i1 < refQueryDatas.Length; i1++)
                {
                    var refQueryData = refQueryDatas[i1];
                    var dirPath = FileTool.GetFullPath(refQueryData.directoryData.directoryPath,
                        refQueryData.directoryData.relativeType);
                    if (refQueryData.queryWhat == QueryWhat.Lua)
                    {
                        CollectFromLua(dirPath, ref otherUsingDic);
                    }
                    else if (refQueryData.queryWhat == QueryWhat.Xlsx)
                    {
                        CollectFromXlsx(dirPath, ref otherUsingDic);
                    }
                    else if (refQueryData.queryWhat == QueryWhat.Prefab)
                    {
                        CollectFromPrefab(dirPath, ref prefabUsingDic);
                    }
                }
            }
        }
    }
}
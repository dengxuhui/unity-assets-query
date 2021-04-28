// ------------
// -author:AER-
// ------------

using System.Collections.Generic;
using System.IO;
using AssetsQuery.Scripts.MultiLanguage;
using AssetsQuery.Scripts.tools;
using UnityEditor;
using UnityEngine;

namespace AssetsQuery.Scripts.window
{
    /// <summary>
    /// 图片白名单窗口
    /// </summary>
    internal class IMGFileWhiteListWindow : EditorWindow
    {
        public static void ShowWindow()
        {
            var window = GetWindow<IMGFileWhiteListWindow>();
            window.titleContent = new GUIContent(LanguageMgr.Read("white_list_window_title"));
            window.Focus();
            window.position = new Rect(new Vector2((float) Screen.width / 2, (float) Screen.height / 2),
                new Vector2((float) Screen.width, (float) Screen.height));
        }

        #region 私有字段

        private WhiteListData[] m_whiteArray;
        private Vector2 m_scrollPos = Vector2.zero;
        #endregion

        private void RefreshData()
        {
            if (m_whiteArray == null)
            {
                AssetQueryDataTool.OrganizeIMGWhiteList();
                var rules = AssetsQueryAssetManager.GetRules();
                m_whiteArray = rules.imgWhiteList;
            }
        }

        private void OnGUI()
        {
            RefreshData();
            var rules = AssetsQueryAssetManager.GetRules();
            var rootDir = FileTool.GetFullPath(rules.imageRootDirectoryData.directoryPath,
                rules.imageRootDirectoryData.relativeType);
            if (!Directory.Exists(rootDir))
            {
                EditorGUILayout.LabelField($"error,not exist::{rootDir}");
                return;
            }

            rootDir = rootDir.Replace(Application.dataPath, "Assets");
            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
            for (var i = 0; i < m_whiteArray.Length; i++)
            {
                if (m_whiteArray[i].type == WhiteListType.Directory)
                {
                    continue;
                }
                var fullPath = Path.Combine(rootDir, m_whiteArray[i].path);
                if (string.IsNullOrEmpty(fullPath))
                {
                    continue;
                }
                var asset = AssetDatabase.LoadAssetAtPath<Texture>(fullPath);
                if(asset == null) continue;
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(LanguageMgr.Read("remove_from_white_list")))
                {
                    m_whiteArray = null;
                    AssetQueryDataTool.RemoveFromWhiteListByIndex(i);
                    break;
                }
                EditorGUILayout.ObjectField("", asset, typeof(TextAsset), false);
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
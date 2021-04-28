using System;
using System.Collections.Generic;
using AssetsQuery.Scripts.MultiLanguage;
using AssetsQuery.Scripts.tools;
using UnityEditor;
using UnityEngine;

// ReSharper disable All

namespace AssetsQuery.Scripts.window
{
    internal class IMGUnusedQueryResultWindow : EditorWindow
    {
        internal static void ShowWindow(List<string> unusedGuidList)
        {
            if (unusedGuidList == null || unusedGuidList.Count <= 0)
            {
                return;
            }

            m_guidList = unusedGuidList;
            var window = GetWindow<IMGUnusedQueryResultWindow>();
            window.titleContent = new GUIContent(LanguageMgr.Read("img_query_result"));
            window.Focus();
        }

        #region window字段

        private static List<string> m_guidList;
        private Vector2 m_scrollPos = Vector2.zero;

        #endregion

        private void OnGUI()
        {
            if (m_guidList == null)
            {
                Close();
                return;
            }

            EditorGUILayout.LabelField(LanguageMgr.Read("query_image_result_title"));
            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
            for (var i = 0; i < m_guidList.Count; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(m_guidList[i]);
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }

                var asset = AssetDatabase.LoadAssetAtPath<Texture>(path);
                if (asset == null) continue;
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("取消删除"))
                {
                    m_guidList.RemoveAt(i);
                    break;
                }

                if (GUILayout.Button("添加至白名单"))
                {
                    if (AssetQueryDataTool.AddImageFileToWhiteListByPath(path))
                    {
                        m_guidList.RemoveAt(i);
                        break;
                    }
                }

                EditorGUILayout.ObjectField("", asset, typeof(TextAsset), false);
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            if (GUILayout.Button(LanguageMgr.Read("button_sure_delete")))
            {
                DoDeleteOperate();
            }
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        private void DoDeleteOperate()
        {
            if (m_guidList == null || m_guidList.Count <= 0)
            {
                return;
            }

            for (var i = 0; i < m_guidList.Count; i++)
            {
                var guid = m_guidList[i];
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (!string.IsNullOrEmpty(path))
                {
                    AssetDatabase.DeleteAsset(path);
                    m_guidList.RemoveAt(i);
                    i--;
                }
            }

            if (m_guidList.Count <= 0)
            {
                Close();
            }

            EditorUtility.DisplayDialog(LanguageMgr.Read("delete_over_title"),
                LanguageMgr.Read("delete_over_content"), "OK");
        }
    }
}
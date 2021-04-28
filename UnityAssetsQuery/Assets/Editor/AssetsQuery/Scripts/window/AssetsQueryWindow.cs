using AssetsQuery.Scripts.func;
using AssetsQuery.Scripts.MultiLanguage;
using UnityEditor;
using UnityEngine;

namespace AssetsQuery.Scripts.window
{
    /// <summary>
    /// 工具窗口
    /// </summary>
    public class AssetsQueryWindow : EditorWindow
    {
        /// <summary>
        /// 打开查找工具
        /// </summary>
        [MenuItem("Window/AssetsQuery", false, 0)]
        static void Init()
        {
            var window = GetWindow(typeof(AssetsQueryWindow));
            window.titleContent = new GUIContent(LanguageMgr.Read("main_window_title"));
            window.Focus();
        }

        /// <summary>
        /// 图片查找功能
        /// </summary>
        private bool _funcImageQuery = false;

        #region ui逻辑

        private void OnGUI()
        {
            #region 图片工具

            _funcImageQuery =
                EditorGUILayout.BeginToggleGroup("-------------------1.冗余图片查询-------------------", _funcImageQuery);

            //清理缓存
            EditorGUILayout.LabelField(LanguageMgr.Read("clear_cache_desc"));
            if (GUILayout.Button(LanguageMgr.Read("btn_text_clear_cache")))
            {
                EditorPrefs.DeleteKey(AssetsQueryGlobalConst.ImageUseMonitorSaveKey);
                EditorUtility.DisplayDialog("清除缓存", "清除缓存成功", "OK");
            }

            //查询
            EditorGUILayout.LabelField(LanguageMgr.Read("query_image_desc"));
            if (GUILayout.Button("查询未使用的图片"))
            {
                FuncFindUnusedIMG.Start();
            }
            
            //查看白名单
            EditorGUILayout.LabelField(LanguageMgr.Read("check_image_white_desc"));
            EditorGUILayout.LabelField(LanguageMgr.Read("check_image_white_func_desc"));
            if (GUILayout.Button(LanguageMgr.Read("btn_check_white_list")))
            {
                IMGFileWhiteListWindow.ShowWindow();
            }
            EditorGUILayout.EndToggleGroup();

            #endregion
        }

        #endregion
    }
}
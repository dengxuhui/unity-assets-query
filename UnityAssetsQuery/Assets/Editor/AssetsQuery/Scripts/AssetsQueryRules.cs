using AssetsQuery.Scripts.MultiLanguage;
using UnityEditor;
using UnityEngine;

namespace AssetsQuery.Scripts
{
    /// <summary>
    /// 配置文件
    /// </summary>
    internal class AssetsQueryRules : ScriptableObject
    {
        /// <summary>
        /// 编辑器使用的语言
        /// </summary>
        public EditorLanguage editorLanguage = EditorLanguage.Chinese;

        /// <summary>
        /// prefab根目录
        /// </summary>
        [Header("图片资源查询配置")]
        [Tooltip("设置UI prefab的根目录")]
        public RelativeDirectoryData prefabRootDirectoryData;
        /// <summary>
        /// 图片根目录
        /// </summary>
        [Tooltip("设置image 资源的根目录")]
        public RelativeDirectoryData imageRootDirectoryData;
        [Tooltip("设置图片白名单")]
        public WhiteListData[] imgWhiteList = new WhiteListData[0];
    }

    [CustomEditor(typeof(AssetsQueryRules))]
    internal class AssetsQueryRulesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button(LanguageMgr.Read("Save")))
            {
                if (GUI.changed)
                {
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
}
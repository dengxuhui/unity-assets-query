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
        public EditorLanguage EditorLanguage = EditorLanguage.Chinese;
        [Header("IMG图片未使用查询工具配置")] 
        public IMGQueryData[] QueryDatas = new IMGQueryData[0];
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
using System;
using System.Collections.Generic;
using System.IO;
using AssetsQuery.Scripts.tools;
using UnityEngine;

namespace AssetsQuery.Scripts.MultiLanguage
{
    /// <summary>
    /// 多语言管理器
    /// </summary>
    public static class LanguageMgr
    {
        /// <summary>
        ///
        /// </summary>
        private static EditorLanguage _usingLang = EditorLanguage.Chinese;

        /// <summary>
        /// 字段字典
        /// </summary>
        private static Dictionary<string, string> _usingDic;

        static LanguageMgr()
        {
            var rule = AssetsQueryAssetManager.GetRules();
            _usingLang = rule.EditorLanguage;
            RefreshLangDic();
        }

        private static void RefreshLangDic()
        {
            _usingDic = new Dictionary<string, string>();
            LanguageConst.LanguageCsvDic.TryGetValue(_usingLang, out var relativePath);
            if (string.IsNullOrEmpty(relativePath))
            {
                Debug.LogError(
                    $"{_usingLang.ToString()}:can not find the configure path,please add into class=>${typeof(LanguageConst).FullName}");
                return;
            }

            var fullPath = FileTool.GetFullPath(relativePath, RelativeType.Assets);
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"file not exist=>{fullPath}");
                return;
            }

            using (var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var sr = new StreamReader(stream);
                var text = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();

                string[] rows = text.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < rows.Length; i++)
                {
                    var row = rows[i];
                    var kv = row.Split('\t');
                    if (kv.Length < 2 || string.IsNullOrEmpty(kv[0]))
                    {
                        continue;
                    }

                    var key = kv[0];
                    var value = kv[1];
                    _usingDic.Add(key, value);
                }
            }
        }

        /// <summary>
        /// 读取字段
        /// </summary>
        /// <param name="langKey"></param>
        /// <returns></returns>
        public static string Read(string langKey)
        {
            if (_usingLang != AssetsQueryAssetManager.GetEditorLanguage())
            {
                _usingLang = AssetsQueryAssetManager.GetEditorLanguage();
                RefreshLangDic();
            }

            _usingDic.TryGetValue(langKey, out var field);
            return string.IsNullOrEmpty(field) ? string.Empty : field;
        }
    }
}
using System.Collections.Generic;

namespace AssetsQuery.Scripts.MultiLanguage
{
    /// <summary>
    /// 语言常量
    /// </summary>
    internal static class LanguageConst
    {
        //语言csv字典
        public static Dictionary<EditorLanguage, string> LanguageCsvDic = new Dictionary<EditorLanguage, string>()
        {
            //中文
            {EditorLanguage.Chinese, "Editor/AssetsQuery/Assets/Language/chinese.csv"},
            //英语
            {EditorLanguage.English, "Editor/AssetsQuery/Assets/Language/english.csv"}
        };
    }
}
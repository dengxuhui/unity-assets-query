namespace AssetsQuery.Scripts
{
    /// <summary>
    /// 搜寻类型
    /// </summary>
    internal enum QueryWhat
    {
        /// <summary>
        /// lua代码中查询
        /// </summary>
        Lua,
        /// <summary>
        /// 配置表中查询
        /// </summary>
        Xlsx,
        /// <summary>
        /// prefab中查询
        /// </summary>
        Prefab,
    }

    /// <summary>
    /// 编辑器语言
    /// </summary>
    internal enum EditorLanguage
    {
        /// <summary>
        /// 中文简体
        /// </summary>
        Chinese,
        /// <summary>
        /// 英语
        /// </summary>
        English,
    }
}
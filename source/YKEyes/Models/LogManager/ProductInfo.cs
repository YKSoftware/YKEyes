namespace YKEyes.Models
{
    using System;
    using System.Reflection;

    /// <summary>
    /// アセンブリ情報を提供します。
    /// </summary>
    internal static class ProductInfo
    {
        private static readonly Assembly assembly = Assembly.GetExecutingAssembly();
        private static readonly Lazy<string> titleLazy = new Lazy<string>(() => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute))).Title);
        private static readonly Lazy<string> descriptionLazy = new Lazy<string>(() => ((AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute))).Description);
        private static readonly Lazy<string> companyLazy = new Lazy<string>(() => ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute))).Company);
        private static readonly Lazy<string> productLazy = new Lazy<string>(() => ((AssemblyProductAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute))).Product);
        private static readonly Lazy<string> copyrightLazy = new Lazy<string>(() => ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute))).Copyright);
        private static readonly Lazy<string> versionLazy = new Lazy<string>(() => Version.ToString(3) + (IsBeta ? " β" : "") + (Version.Revision == 0 ? "" : " rev." + Version.Revision) + (IsDebug ? " Debug" : ""));

        /// <summary>
        /// アセンブリタイトルを取得します。
        /// </summary>
        public static string Title { get { return titleLazy.Value; } }

        /// <summary>
        /// アセンブリの詳細説明を取得します。
        /// </summary>
        public static string Description { get { return descriptionLazy.Value; } }

        /// <summary>
        /// プロダクト開発元を取得します。
        /// </summary>
        public static string Company { get { return companyLazy.Value; } }

        /// <summary>
        /// プロダクト名を取得します。
        /// </summary>
        public static string Product { get { return productLazy.Value; } }

        /// <summary>
        /// 著作権情報を取得します。
        /// </summary>
        public static string Copyright { get { return copyrightLazy.Value; } }

        /// <summary>
        /// バージョンを取得します。
        /// </summary>
        public static Version Version { get { return assembly.GetName().Version; } }

        /// <summary>
        /// バージョン文字列を取得します。
        /// </summary>
        public static string VersionString { get { return versionLazy.Value; } }

        /// <summary>
        /// ベータ仕様かどうかを取得します。
        /// </summary>
        public static bool IsBeta
        {
            get
            {
#if BETA
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// デバッグモードかどうかを取得します。
        /// </summary>
        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
    }
}

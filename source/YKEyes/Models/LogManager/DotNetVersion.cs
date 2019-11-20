namespace YKEyes.Models
{
    using Microsoft.Win32;
    using System;

    /// <summary>
    /// .NET Framework のバージョンを取得する機能を提供します。
    /// 詳細は下記 URL を参照。
    /// https://docs.microsoft.com/ja-jp/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
    /// </summary>
    public class DotNetVersion
    {
        private static readonly Lazy<string> versionLazy = new Lazy<string>(() =>
        {
            const string subKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            var version = "";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subKey))
            {
                if (ndpKey != null)
                {
                    version = GetVersionCore((int)ndpKey.GetValue("Release"));
                }
            }

            if (string.IsNullOrEmpty(version))
            {
                version = Environment.Version.ToString();
            }

            return version;
        });
        /// <summary>
        /// バージョンを取得します。
        /// </summary>
        public static string Version { get { return versionLazy.Value; } }

        /// <summary>
        /// レジストリの値からバージョン番号を取得します。
        /// </summary>
        /// <param name="releaseKey">レジストリに登録されている ReleaseKey を指定します。</param>
        /// <returns>.NET Framework のバージョンを文字列として返します。</returns>
        private static string GetVersionCore(int releaseKey)
        {
            if (releaseKey >= 461808) return "4.7.2 or later";
            if (releaseKey >= 461308) return "4.7.1";
            if (releaseKey >= 460798) return "4.7";
            if (releaseKey >= 394802) return "4.6.2";
            if (releaseKey >= 394254) return "4.6.1";
            if (releaseKey >= 393295) return "4.6";
            if (releaseKey >= 379893) return "4.5.2";
            if (releaseKey >= 378675) return "4.5.1";
            if (releaseKey >= 378389) return "4.5";

            return "Legacy Version";
        }
    }
}

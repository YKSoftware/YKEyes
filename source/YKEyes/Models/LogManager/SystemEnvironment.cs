namespace YKEyes.Models
{
    using System;
    using System.Linq;
    using System.Management;

    /// <summary>
    /// アセンブリの動作環境に関する情報を提供します。
    /// </summary>
    public class SystemEnvironment
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public SystemEnvironment()
        {
            try
            {
                using (var managementClass = new ManagementClass("Win32_OperatingSystem"))
                using (var managementObject = managementClass.GetInstances().OfType<ManagementObject>().FirstOrDefault())
                {
                    // あえて NullReferenceException を許容することで ErrorMessage にエラー内容を出力させる
                    //if (managementObject == null) return;

                    this.OS = managementObject["Caption"].ToString();
                    this.OSVersion = managementObject["Version"].ToString();
                    this.Architecture = managementObject["OSArchitecture"].ToString();

                    this.TotalPhysicalMemorySize = ((ulong)managementObject["TotalVisibleMemorySize"]).ToString("N0") + " KB";
                    this.FreePhysicalMemorySize = ((ulong)managementObject["FreePhysicalMemory"]).ToString("N0") + " KB";
                }

                using (var managementClass = new ManagementClass("Win32_Processor"))
                using (var managementObject = managementClass.GetInstances().OfType<ManagementObject>().FirstOrDefault())
                {
                    // あえて NullReferenceException を許容することで ErrorMessage にエラー内容を出力させる
                    //if (managementObject == null) return;

                    this.CPU = managementObject["Name"].ToString();
                }

                this.DotNetVersion = ".NET Framework Version: " + Models.DotNetVersion.Version;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// OS 名を取得します。
        /// </summary>
        public string OS { get; private set; }

        /// <summary>
        /// OS のバージョンを取得します。
        /// </summary>
        public string OSVersion { get; private set; }

        /// <summary>
        /// OS のアーキテクチャを取得します。
        /// </summary>
        public string Architecture { get; private set; }

        /// <summary>
        /// CPU 名を取得します。
        /// </summary>
        public string CPU { get; private set; }

        /// <summary>
        /// 総メモリサイズを取得します。
        /// </summary>
        public string TotalPhysicalMemorySize { get; private set; }

        /// <summary>
        /// 空きメモリサイズを取得します。
        /// </summary>
        public string FreePhysicalMemorySize { get; private set; }

        /// <summary>
        /// .NET Framework のバージョンを取得します。
        /// </summary>
        public string DotNetVersion { get; private set; }

        /// <summary>
        /// エラーメッセージを取得します。
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// インスタンスを文字列に変換します。
        /// </summary>
        /// <returns>変換した文字列を返します。</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                return string.Join(Environment.NewLine, new string[]
                {
                    this.ErrorMessage,
                });
            }

            return string.Join(Environment.NewLine, new string[]
            {
                "OS: " + this.OS,
                "OSVersion: " + this.OSVersion,
                "Architecture: " + this.Architecture,
                "Runtime: " + this.DotNetVersion,
                "",
                "CPU: " + this.CPU,
                "RAM (Total): " + this.TotalPhysicalMemorySize,
                "RAM (Free): " + this.FreePhysicalMemorySize,
            });
        }
    }
}

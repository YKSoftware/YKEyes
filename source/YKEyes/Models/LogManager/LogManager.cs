namespace YKEyes.Models
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal static class LogManager
    {
        /// <summary>
        /// エラーログを出力します。
        /// </summary>
        /// <param name="sender">エラー発生元を指定します。</param>
        /// <param name="exception">例外情報を指定します。</param>
        /// <param name="caller">呼び出し元の名称を指定します。</param>
        public static void WriteExceptionLog(object sender, Exception exception, [CallerMemberName]string caller = null)
        {
            try
            {
                var now = DateTime.Now;
                var path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),  // %LOCALAPPDATA%
                    ProductInfo.Company,
                    ProductInfo.Product,
                    "ErrorLog",
                    "ErrorLog_" + now.ToString("yyyyMMdd_HHmmssfff") + ".log"
                    );
                var message = string.Join(Environment.NewLine, new string[]
                {
                    "========= Exception Report from " + caller + " =========",
                    now.ToString(),
                    ProductInfo.Product + " Ver." + ProductInfo.VersionString,
                    "",
                    (new SystemEnvironment()).ToString(),
                    "",
                    "Sender: " + (sender is Type ? (sender as Type).FullName : (sender == null ? "null" : sender.GetType().FullName)),
                    "Exception: " + (exception == null ? "null" : exception.GetType().FullName),
                    "",
                    exception == null ? "" : exception.ToString(),
                });

                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.AppendAllText(path, message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}

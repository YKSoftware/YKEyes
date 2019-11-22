namespace YKEyes
{
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using YKEyes.Models;
    using YKEyes.ViewModels;
    using YKEyes.Views;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;

            YKToolkit.Controls.ThemeManager.Instance.Initialize("Dark Orange");
        }

        /// <summary>
        /// 起動時のコールバック処理をおこないます。
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var w = new MainView() { DataContext = new MainViewModel() };
            w.Show();
        }

        #region 未処理例外ハンドラ

        /// <summary>
        /// 現在のアプリケーションドメインで処理されない例外が発生した場合のイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            LogManager.WriteExceptionLog(sender, exception);
            Environment.Exit(0);
        }

        /// <summary>
        /// 未処理例外発生イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogManager.WriteExceptionLog(sender, e.Exception);
            e.Handled = true;
            this.Shutdown();
        }

        #endregion 未処理例外ハンドラ
    }
}

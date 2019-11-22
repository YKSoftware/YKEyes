namespace YKEyes.Views
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Eye.xaml の相互作用ロジック
    /// </summary>
    public partial class Eye : UserControl
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public Eye()
        {
            InitializeComponent();

            // プライマリスクリーンのサイズを保持
            var primaryScreen = System.Windows.Forms.Screen.PrimaryScreen;
            this._screenSize = new Size(primaryScreen.Bounds.Width, primaryScreen.Bounds.Height);

            // デザイン実行時に this.ActualWidth などにアクセスするとデザイナーで以下の例外が発生します。
            // System.InvalidOperationException
            //   この Visual は、PresentationSource に接続されていません。
            // このため、そもそも問題が発生しないようにマウスフックさせないようにします。
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this._mouseHook.MouseMove += MouseMoveHook;
                this._mouseHook.Hook();
            }
        }

        /// <summary>
        /// MouseMove をフックします。
        /// </summary>
        /// <param name="e">イベント引数</param>
        private void MouseMoveHook(YKToolkit.Controls.MouseHook.MSLLHOOKSTRUCT e)
        {
            // 自分の中心座標
            var origin = this.PointToScreen(new Point(this.ActualWidth / 2, this.ActualHeight / 2));
            System.Diagnostics.Debug.WriteLine("中心 = " + origin);

            // マウス座標
            System.Diagnostics.Debug.WriteLine("マウス = " + e.Point);
        }

        /// <summary>
        /// マウスフックするためのオブジェクト
        /// </summary>
        private YKToolkit.Controls.MouseHook _mouseHook = new YKToolkit.Controls.MouseHook();

        /// <summary>
        /// 画面サイズ
        /// </summary>
        private Size _screenSize;
    }
}

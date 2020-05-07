namespace YKEyes.Views
{
    using System;
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

            // デザイン実行時に this.ActualWidth などにアクセスするとデザイナーで以下の例外が発生します。
            // System.InvalidOperationException
            //   この Visual は、PresentationSource に接続されていません。
            // このため、そもそも問題が発生しないようにマウスフックさせないようにします。
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this._mouseHook.MouseMove += MouseMoveHook;
                this._mouseHook.Hook();

                this.SizeChanged += OnSizeChanged;
            }
        }

        /// <summary>
        /// SizeChanged イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this._maxOffset.X = 0.25 * this.ActualWidth;
            this._maxOffset.Y = 0.25 * this.ActualHeight;
        }

        /// <summary>
        /// MouseMove をフックします。
        /// </summary>
        /// <param name="e">イベント引数</param>
        private void MouseMoveHook(YKToolkit.Controls.MouseHook.MSLLHOOKSTRUCT e)
        {
            // 自分の中心座標
            var origin = this.CalcOrigin();

            // マウス座標
            var mouse = new Point(e.Point.X, e.Point.Y);

            MoveEye(origin, mouse);
        }

        /// <summary>
        /// 瞳を動かします。
        /// </summary>
        /// <param name="origin">中心座標</param>
        /// <param name="mouse">マウス座標</param>
        private void MoveEye(Point origin, Point mouse)
        {
            var offset = default(Vector);

            // 原点からマウス位置への直線の傾きパラメータを算出
            var diff = new Point(mouse.X - origin.X, mouse.Y - origin.Y);

            if (diff.X == 0)
            {
                // マウスが真上/真下の場合
                return;
            }
            else if (diff.Y == 0)
            {
                // マウスが真横の場合
                return;
            }
            else
            {
                // 楕円の径と補助計算
                var a = this.ActualWidth / 2.0;
                var b = this.ActualHeight / 2.0;
                var c = diff.Y / diff.X;
                var x = a * b / Math.Sqrt(a * a * c * c + b * b);
                if (mouse.X < origin.X) x *= -1;
                var y = c * x;
                offset = new Vector(x * Math.Abs(diff.X * _ratio), y * Math.Abs(diff.Y * _ratio));
            }

            // オフセットの限界値確認
                 if (offset.X > 0) offset.X = Math.Min(offset.X, this._maxOffset.X);
            else if (offset.X < 0) offset.X = Math.Max(offset.X, -this._maxOffset.X);
                 if (offset.Y > 0) offset.Y = Math.Min(offset.Y, this._maxOffset.Y);
            else if (offset.Y < 0) offset.Y = Math.Max(offset.Y, -this._maxOffset.Y);

            // オフセットする
            this.PupilOffset.X = offset.X;
            this.PupilOffset.Y = offset.Y;
        }

        /// <summary>
        /// 自分の中心座標を算出します。
        /// </summary>
        private Point CalcOrigin()
        {
            return this.PointToScreen(new Point(this.ActualWidth / 2, this.ActualHeight / 2));
        }

        /// <summary>
        /// マウスフックするためのオブジェクト
        /// </summary>
        private YKToolkit.Controls.MouseHook _mouseHook = new YKToolkit.Controls.MouseHook();

        /// <summary>
        /// 自分の中心座標から ±_maxOffset の範囲を可動範囲とします。
        /// </summary>
        private Point _maxOffset;

        /// <summary>
        /// 比率 1/400
        /// </summary>
        private const double _ratio = 0.0025;
    }
}

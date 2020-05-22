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
            this.pupil.Width = 0.2 * this.ActualWidth;
            this.pupil.Height = this.pupil.Width;
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
                // 楕円の中心座標を原点として考えたとき、
                //   x^2/a^2 + y^2/b^2 = 1
                //                   y = cx
                // の交点の座標は
                //   x = ± ab / (a^2 c^2 + b^2)^(1/2)
                //   y = cx
                // で表される。

                // マウス座標から楕円を縮小し、
                // その楕円と直線との交点をそのまま瞳の位置とする。

                // というわけでまず楕円を縮小するために径を考えます。
                // 境界線上に重ならないようにするためのマージンを含む最大値に比率を掛け算します。
                var ratio = 1.0 / Math.Sqrt(_ratio);
                var margin = this.pupil.Width;
                var a = (this.ActualWidth / 2.0 - margin) * Math.Min(1.0, Math.Abs(ratio * diff.X));
                var b = (this.ActualHeight / 2.0 - margin) * Math.Min(1.0, Math.Abs(ratio * diff.Y));

                // 直線の傾きと切片
                // 切片は楕円の中心から若干ずらすとよりリアルに見えます。
                // マウスが楕円の中心より上にある場合はやや上、下にある場合はやや下にずらします。
                // マウスが中心付近にある場合は楕円の中心を切片とします。
                var c = diff.Y / diff.X;
                var d = b / 3.0;
                     if (Math.Abs(diff.Y) < d) d = 0.0;
                else if (mouse.Y < origin.Y) d = -b / 3.0;

                // 縮小した楕円と直線の交点を求めます。
                var numerator1 = -2 * a * a * c * d;
                var numerator2 = 2 * a * b * Math.Sqrt(a * a * c * c + b * b - d * d);
                if (mouse.X < origin.X) numerator2 *= -1;
                var denominator = 2 * (a * a * c * c + b * b);
                var x = (numerator1 + numerator2) / denominator;
                var y = c * x + d;

                // 交点の座標そのものがオフセット量となります。
                offset = new Vector(x, y);
            }

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
        /// 比率
        /// </summary>
        private const double _ratio = 20000;
    }
}

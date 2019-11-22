namespace YKEyes.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// MainView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            this.MainContainer.MouseLeftButtonDown += (_, __) => this.DragMove();
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            // 縦横比を保持しながらリサイズする
            var change = Math.Max(e.HorizontalChange, e.VerticalChange);
            var width = this.ActualWidth + change;
                {
                         if (width < this.MinWidth) width = this.MinWidth;
                    else if (width > this.MaxWidth) width = this.MaxWidth;
                }
            var height = width * this.ActualHeight / this.ActualWidth;

            this.Width = width;
            this.Height = height;
        }
    }
}

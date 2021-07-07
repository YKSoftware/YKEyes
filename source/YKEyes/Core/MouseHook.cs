namespace YKEyes
{
    using System;

    public class MouseHook : IDisposable
    {
        static MouseHook()
        {
            Current.Hook();
        }

        public static YKToolkit.Controls.MouseHook Current { get; } = new YKToolkit.Controls.MouseHook();

        public void Dispose()
        {
            Current.UnHook();
        }
    }
}

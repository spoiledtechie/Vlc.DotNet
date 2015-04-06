using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Vlc.DotNet.Core.Interops;

namespace Vlc.DotNet.Wpf.Form
{
    /// <summary>
    /// this control is pulled from the Forms and can be attached onto smaller windows
    /// But also has airspace issues.
    /// http://blogs.msdn.com/b/dwayneneed/archive/2013/02/26/mitigating-airspace-issues-in-wpf-applications.aspx
    /// </summary>
    public class VlcControl : HwndHost 
    {
        public Forms.VlcControl MediaPlayer { get; private set; }

        public VlcControl()
        {
            MediaPlayer = new Forms.VlcControl();
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            Win32Interops.SetParent(MediaPlayer.Handle, hwndParent.Handle);
            return new HandleRef(this, MediaPlayer.Handle);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            if(MediaPlayer == null)
                return;
            Win32Interops.SetParent(IntPtr.Zero, hwnd.Handle);
            MediaPlayer.Dispose();
            MediaPlayer = null;
        }
    }
}
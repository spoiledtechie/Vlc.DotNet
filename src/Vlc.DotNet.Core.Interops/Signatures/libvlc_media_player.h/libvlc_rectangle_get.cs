using System;
using System.Runtime.InteropServices;

namespace Vlc.DotNet.Core.Interops.Signatures
{
    /// <summary>
    /// Gets the current rectangle.
    /// </summary>
    [LibVlcFunction("libvlc_rectangle_t")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate Rectangle GetRectangle(IntPtr instance);
}

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NickX.Dozentenplanung.ClientApplication.UI.CustomForms
{
    public static class FormExtension
    {
        [DllImport("user32.dll")]
        static internal extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        public static void EnableBlur(this Form @this)
        {
            var accent = new AccentPolicy();
            accent.AccentState = AccentStates.ACCENT_ENABLE_BLURBEHIND;
            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);
            var data = new WindowCompositionAttributeData()
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };
            SetWindowCompositionAttribute(@this.Handle, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }

        internal enum AccentStates
        {
            ACCENT_ENABLE_BLURBEHIND = 3
        }

        internal struct AccentPolicy
        {
            public AccentStates AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute
        { 
            WCA_ACCENT_POLICY = 19
        }
    }

    
}

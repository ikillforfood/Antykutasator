using System.Runtime.InteropServices;

namespace Antykutasator.Helpers
{
    public static class NativeMethodsHelper
    {
        [DllImport("user32")]
        public static extern void LockWorkStation();

    }
}

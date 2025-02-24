using System.Diagnostics;

namespace AITLib.Helpers
{
    public class AitDebugHelper
    {
        private static bool debugging;

        public static bool RunningInDebugMode()
        {
            WellAreWe();
            return debugging;
        }

        [Conditional("DEBUG")]
        private static void WellAreWe()
        {
            debugging = true;
        }
    }
}

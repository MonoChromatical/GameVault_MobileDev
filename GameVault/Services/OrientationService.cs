#if ANDROID
using Android.Content.PM;
using Microsoft.Maui.ApplicationModel;
#endif

namespace GameVault.Services
{
    // FLOW:
    // SettingsPage asks OrientationService to apply the portrait lock setting.
    // OrientationService talks to the Android activity.
    // Android then locks the screen to portrait or lets the phone rotate normally.
    public class OrientationService
    {
        public void ApplyPortraitLockSetting(bool lockPortrait)
        {
#if ANDROID
            if (Platform.CurrentActivity == null)
            {
                return;
            }

            if (lockPortrait == true)
            {
                Platform.CurrentActivity.RequestedOrientation = ScreenOrientation.Portrait;
            }
            else
            {
                Platform.CurrentActivity.RequestedOrientation = ScreenOrientation.Unspecified;
            }
#endif
        }
    }
}

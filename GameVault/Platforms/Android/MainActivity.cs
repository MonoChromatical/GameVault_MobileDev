using Android.App;
using Android.Content.PM;
using Android.OS;
using GameVault.Services;

namespace GameVault
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // FLOW:
            // Android starts MainActivity when the app launches.
            // MainActivity reads the saved portrait lock setting.
            // Android applies the saved orientation before the user opens Settings.
            // If the lock is off, orientation follows the phone/emulator sensor.
            if (UserSettings.LockRotation == true)
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            }
            else
            {
                RequestedOrientation = ScreenOrientation.Unspecified;
            }
        }
    }
}

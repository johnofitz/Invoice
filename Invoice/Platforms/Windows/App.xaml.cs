using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Graphics;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Invoice.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            ConfigurePickerHandler();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            var currentWindow = Application?.Windows[0]?.Handler?.PlatformView;
            var height = Application?.Windows[0].Height;
            var width = Application?.Windows[0].Width;


            IntPtr _windowHandle = WindowNative.GetWindowHandle(currentWindow);


            var windowId = Win32Interop.GetWindowIdFromWindow(_windowHandle);

            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(1200, 900));

            if (width != null && height != null)
            {
                var newLeft = (int)((width - width * 0.90) / 2);
                var newTop = (int)((height + height * 0.30) / 2);

                //// Move the window to the calculated position
                appWindow.Move(new PointInt32(newTop, newLeft));
            }
        }


        private static void ConfigurePickerHandler()
        {
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(IPicker.Title), (handler, view) =>
            {
                if (handler.PlatformView is not null && view is Picker pick && !string.IsNullOrWhiteSpace(pick.Title))
                {
                    handler.PlatformView.HeaderTemplate = new Microsoft.UI.Xaml.DataTemplate();
                    handler.PlatformView.PlaceholderText = pick.Title;
                    pick.Title = null;
                }
            });
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }

}

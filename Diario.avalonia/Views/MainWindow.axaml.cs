using Avalonia.Controls;
using DesktopNotifications.FreeDesktop;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using INotificationManager = DesktopNotifications.INotificationManager;
using Notification = DesktopNotifications.Notification;
namespace Diario.avalonia.Views;

public partial class MainWindow : Window
{
    public static ResourceDictionary d;
    private static INotificationManager notification= CreateManager();
    private static Notification not;
    private static INotificationManager CreateManager()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return new FreeDesktopNotificationManager();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return new DesktopNotifications.Windows.WindowsNotificationManager(null);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return new DesktopNotifications.Apple.AppleNotificationManager();

        throw new PlatformNotSupportedException();
    }
    public MainWindow()
    {
        InitializeComponent();
        d = this.FindResource(CultureInfo.CurrentCulture.TwoLetterISOLanguageName) as ResourceDictionary;
        if (d == null)
            d = this.FindResource("it") as ResourceDictionary;
        MainView.Traduci();
    }

    public static void MakeNotification(String title, String body)
    {
        not = new Notification
        {
            Title = title,
            Body = body
        };
        notification.ShowNotification(not);
    }
}

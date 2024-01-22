using Cs2CaseCalculator.Logic;
using Cs2CaseCalculator.ViewModels;
using Serilog;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Cs2CaseCalculator.Views
{
    public partial class MainWindow : Window
    {
        #region Remove from Alt+Tab Menu
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        #endregion
        public MainWindow()
        {
            this.InitializeComponent();
            ((MainWindowViewModel)this.DataContext).Instance = this;
            this.Title = typeof(MainWindow).Assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;

            Task.Factory.StartNew(() =>
            {
                if (RuntimeStorage.Cases == null || RuntimeStorage.Cases.Count == 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ((MainWindowViewModel)this.DataContext).UpdateCache();
                    });
                }

                this.Dispatcher.Invoke(() =>
                {
                    ((MainWindowViewModel)this.DataContext).Cases = new(RuntimeStorage.Cases);
                });
            });
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Verbose("Window closing");
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (RuntimeStorage.Configuration.RuntimeConfiguration.WindowLocation != default)
            {
                this.Left = RuntimeStorage.Configuration.RuntimeConfiguration.WindowLocation.X;
                this.Top = RuntimeStorage.Configuration.RuntimeConfiguration.WindowLocation.Y;
            }

            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            _ = SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TOOLWINDOW);

            Log.Verbose("Window loaded");
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();

                RuntimeStorage.Configuration.RuntimeConfiguration.WindowLocation = new((int)this.Left, (int)this.Top);
                RuntimeStorage.Configuration.Save();

                Log.Verbose($"Window relocated to coords:\nX: {this.Left}\nY: {this.Top}");

                Log.Verbose($"Window relocated to coords:\nX: {this.Left}\nY: {this.Top}");
            }
        }
    }
}

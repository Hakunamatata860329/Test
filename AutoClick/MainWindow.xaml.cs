using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AutoClick
{
    public partial class MainWindow : Window
    {
        private bool autoClicking = false;
        private POINT clickPoint;

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        public MainWindow()
        {
            InitializeComponent();
            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.T)
            {
                GetCursorPos(out clickPoint);
                MessageBox.Show($"Recorded new position: X={clickPoint.X}, Y={clickPoint.Y}", "Record Position");
            }
        }

        private void AutoClickButtonClick(object sender, RoutedEventArgs e)
        {
            autoClicking = true;

            Thread autoClickThread = new Thread(AutoClick);
            autoClickThread.Start();
        }

        private void AutoClick()
        {
            while (autoClicking)
            {
                if (clickPoint.X != 0 && clickPoint.Y != 0)
                {
                    SetCursorPos(clickPoint.X, clickPoint.Y);

                    // First click
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, clickPoint.X, clickPoint.Y, 0, 0);
                    // Small delay (you may need to adjust this value)
                    Thread.Sleep(50); // Sleep for 100 milliseconds
                    // Second click
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, clickPoint.X, clickPoint.Y, 0, 0);

                    // 添加一條訊息到 UI TextBox
                    string message = $"Click completed at X={clickPoint.X}, Y={clickPoint.Y}.\r\n";
                    Dispatcher.Invoke(() => AppendMessageToTextBox(message));

                    Thread.Sleep(5000);
                }
            }
        }

        private void AppendMessageToTextBox(string message)
        {
            // 在 UI TextBox 中添加一條訊息
            logTextBox.AppendText(message);
        }

        private void StopClickButtonClick(object sender, RoutedEventArgs e)
        {
            autoClicking = false;
            logTextBox.Clear();
        }

        [DllImport("user32.dll")]
        private static extern void SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
    }
}
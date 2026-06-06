using System;
using System.Windows;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;

namespace JustAlarm
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _clockTimer;
        private DispatcherTimer _alarmTimer;
        private int _alarmHour = -1;
        private int _alarmMin = -1;
        private bool _alarmSet = false;
        private TaskbarIcon _trayIcon;
        private bool _realClose = false;

        private const string CurrentVersion = "1.3";

        public MainWindow()
        {
            InitializeComponent();
            InitTrayIcon();
            _ = CheckUpdateAsync();

            _clockTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();

            _alarmTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _alarmTimer.Tick += AlarmTimer_Tick;
            _alarmTimer.Start();
        }

        private async System.Threading.Tasks.Task CheckUpdateAsync()
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                string latest = await client.GetStringAsync("https://justalarm.dothome.co.kr/version.txt");
                latest = latest.Trim();

                if (latest != CurrentVersion)
                {
                    MessageBox.Show(
                        $"새 버전 {latest}이 출시됐습니다!\njustalarm.dothome.co.kr 에서 다운로드하세요.",
                        "⬆️ 업데이트 알림",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch { /* 인터넷 없으면 무시 */ }
        }

        private void InitTrayIcon()
        {
            _trayIcon = new TaskbarIcon();
            _trayIcon.IconSource = new System.Windows.Media.Imaging.BitmapImage(
                new Uri("pack://application:,,,/icon.ico"));
            _trayIcon.ToolTipText = "Just Alarm";

            _trayIcon.TrayMouseDoubleClick += (s, e) =>
            {
                Show();
                WindowState = WindowState.Normal;
                Activate();
            };

            var menu = new System.Windows.Controls.ContextMenu();

            var openItem = new System.Windows.Controls.MenuItem { Header = "열기" };
            openItem.Click += (s, e) =>
            {
                Show();
                WindowState = WindowState.Normal;
                Activate();
            };

            var exitItem = new System.Windows.Controls.MenuItem { Header = "종료" };
            exitItem.Click += (s, e) =>
            {
                _realClose = true;
                _trayIcon.Dispose();
                Application.Current.Shutdown();
            };

            menu.Items.Add(openItem);
            menu.Items.Add(exitItem);
            _trayIcon.ContextMenu = menu;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_realClose)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            int h = now.Hour % 12;
            if (h == 0) h = 12;
            string ampm = now.Hour < 12 ? "AM" : "PM";
            ClockText.Text = $"현재 시각: {ampm} {h:D2}:{now.Minute:D2}:{now.Second:D2}";
        }

        private void AlarmTimer_Tick(object sender, EventArgs e)
        {
            if (!_alarmSet) return;
            var now = DateTime.Now;
            if (now.Hour == _alarmHour && now.Minute == _alarmMin && now.Second == 0)
            {
                _alarmSet = false;
                int h = _alarmHour % 12;
                if (h == 0) h = 12;
                string ampm = _alarmHour < 12 ? "AM" : "PM";
                System.Media.SystemSounds.Exclamation.Play();
                Show();
                WindowState = WindowState.Normal;
                Activate();
                MessageBox.Show($"알람! {ampm} {h}:{_alarmMin:D2}", "⏰ Just Alarm",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                StatusText.Text = "알람이 울렸습니다.";
            }
        }

        private void SetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(HourBox.Text, out int hour) || hour < 1 || hour > 12)
            {
                MessageBox.Show("1~12 사이의 시간을 입력하세요.", "입력 오류");
                return;
            }
            if (!int.TryParse(MinBox.Text, out int min) || min < 0 || min > 59)
            {
                MessageBox.Show("0~59 사이의 분을 입력하세요.", "입력 오류");
                return;
            }

            bool isPm = PmBtn.IsChecked == true;
            int hour24 = hour;
            if (!isPm && hour == 12) hour24 = 0;
            else if (isPm && hour != 12) hour24 = hour + 12;

            _alarmHour = hour24;
            _alarmMin = min;
            _alarmSet = true;

            string ampm = isPm ? "PM" : "AM";
            StatusText.Text = $"✔  알람 설정: {ampm} {hour}:{min:D2}";
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _alarmSet = false;
            _alarmHour = -1;
            _alarmMin = -1;
            StatusText.Text = "알람이 취소되었습니다.";
        }
    }
}
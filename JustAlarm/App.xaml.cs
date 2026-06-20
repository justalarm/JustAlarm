using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Windows;

namespace JustAlarm
{
    public partial class App : Application
    {
        private static Mutex _mutex;
        private const string PipeName = "JustAlarm_Pipe";

        protected override void OnStartup(StartupEventArgs e)
        {
            _mutex = new Mutex(true, "JustAlarm_SingleInstance", out bool createdNew);
            if (!createdNew)
            {
                try
                {
                    using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
                    client.Connect(1000);
                    using var writer = new StreamWriter(client);
                    writer.Write("show");
                }
                catch { }
                Shutdown();
                return;
            }

            new Thread(PipeServer) { IsBackground = true }.Start();
            base.OnStartup(e);
        }

        private void PipeServer()
        {
            while (true)
            {
                try
                {
                    using var server = new NamedPipeServerStream(PipeName, PipeDirection.In);
                    server.WaitForConnection();
                    using var reader = new StreamReader(server);
                    if (reader.ReadToEnd() == "show")
                    {
                        Dispatcher.Invoke(() =>
                        {
                            MainWindow?.Show();
                            if (MainWindow != null) MainWindow.WindowState = WindowState.Normal;
                            MainWindow?.Activate();
                        });
                    }
                }
                catch { }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
            base.OnExit(e);
        }
    }
}

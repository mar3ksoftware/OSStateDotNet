using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using OSStateDotNet.Enums;

namespace OSStateDotNet.Services.Implementations
{
    internal sealed class LinuxSleepStateServiceImpl : SystemSleepStateService
    {
        private const string SuspendedMessage = "suspending system";
        private const string ResumedMessage = "system resumed";
        private const string GrepCommand = "journalctl | grep 'systemd-sleep'";
        private bool _isProcessing;

        public LinuxSleepStateServiceImpl()
        {
            _isProcessing = false;
            SuspendCheckTimer = new Timer();
            SuspendCheckTimer.Enabled = false;
            SuspendCheckTimer.Interval = 2500;
            SuspendCheckTimer.Elapsed += SuspendCheckTimerOnElapsed;
            SuspendCheckTimer.Start();
        }

        private Timer SuspendCheckTimer { get; }

        private void SuspendCheckTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            InvokeGrepProcessing();
        }

        private void InvokeGrepProcessing()
        {
            if (_isProcessing) return;
            _ = Task
                .Run(ProcessGrep)
                .ConfigureAwait(false);
        }

        private void ProcessGrep()
        {
            _isProcessing = true;
            SendJournalCtlCommand();
            _isProcessing = false;
        }

        private void SendJournalCtlCommand()
        {
            var result = new List<string>();
            using var proc = new Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = "-c \" " + GrepCommand + " \"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();

            while (!proc.HasExited && !proc.StandardOutput.EndOfStream)
            {
                var line = proc.StandardOutput.ReadLine();
                result.Add(line);
            }

            proc.WaitForExit();
            ProcessStdOut(result);
            result.Clear();
        }

        private void ProcessStdOut(IReadOnlyList<string> result)
        {
            var resultSize = result.Count - 1;
            for (var i = resultSize; i > 0; i--)
            {
                var msg = result[i].ToLower();
                if (msg.Contains(SuspendedMessage))
                {
                    if (CurrentState != SystemSleepState.Suspended)
                        OnSystemSleepStateChanged(SystemSleepState.Suspended);
                    break;
                }

                if (!msg.Contains(ResumedMessage)) continue;
                if (CurrentState != SystemSleepState.Resumed) OnSystemSleepStateChanged(SystemSleepState.Resumed);
                break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            SuspendCheckTimer.Stop();
            SuspendCheckTimer.Elapsed -= SuspendCheckTimerOnElapsed;
            SuspendCheckTimer.Dispose();
        }
    }
}
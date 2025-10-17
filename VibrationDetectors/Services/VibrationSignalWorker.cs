using System;
using System.Threading;
using System.Threading.Tasks;
using VibrationDetectors.Models;

namespace VibrationDetectors.Services
{
    public class VibrationSignalWorker : IDisposable
    {
        private readonly double _mean;
        private readonly double _sigma;
        private readonly TimeSpan _interval;
        private readonly Random _random;
        private CancellationTokenSource _cts;
        private Task _workerTask;

        /// <summary>
        /// The latest simulated vibration signal value.
        /// </summary>
        public double SignalValue { get; private set; }

        public bool IsRunning => _workerTask != null && !_workerTask.IsCompleted;

        public VibrationSignalWorker(double mean = 5.0, double sigma = 1.95, TimeSpan? interval = null)
        {
            _mean = mean;
            _sigma = sigma;
            _interval = interval ?? TimeSpan.FromSeconds(1);
            _random = new Random();
        }

        /// <summary>
        /// Starts the background worker if it's not already running.
        /// </summary>
        public void Start()
        {
            if (IsRunning)
                return;

            _cts = new CancellationTokenSource();
            _workerTask = Task.Run(() => RunAsync(_cts.Token));
        }

        /// <summary>
        /// Stops the background worker.
        /// </summary>
        public void Stop()
        {
            if (_cts == null)
                return;

            _cts.Cancel();
            _workerTask?.Wait();
            _cts.Dispose();
            _cts = null;
            _workerTask = null;
        }

        private async Task RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                SignalValue = GenerateSignal();

                VibrationDetector.VibrationLevel = (int)Math.Round(SignalValue);
                

                await Task.Delay(_interval, token);
            }
        }

        private double GenerateSignal()
        {
            // Generate normally distributed number using Box-Muller transform
            double u1 = 1.0 - _random.NextDouble();
            double u2 = 1.0 - _random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2);
            double value = _mean + _sigma * randStdNormal;

            // Clamp between 0 and 10
            return Math.Max(0, Math.Min(10, value));
        }

        public void Dispose()
        {
            Stop();
        }
    }
}

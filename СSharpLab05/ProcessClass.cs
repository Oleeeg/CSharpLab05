using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CSharpLab05
{
    public class ProcessClass
    {
        private PerformanceCounter CounterCpu { get; }
        private PerformanceCounter RAMCounter { get; }
        private readonly long _total = PerformanceInfo.GetWholeMemory() * 10000;
        private readonly int _processorCount = Environment.ProcessorCount;
        private Process Process;
        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public double Cpu { get; set; }
        public double RAM { get; set; }
        public int ThreadNumber { get; set; }
        public string Path { get; set; }
        public DateTime Time { get; set; }

        internal ProcessClass(Process process)
        {
            Process = process;
            Name = process.ProcessName;
            Id = process.Id;
            ThreadNumber = process.Threads.Count;
            SetPathName(process);
            SetProcessTime(process);
            CounterCpu = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true);
            IsActive = process.Responding;
            RAMCounter = new PerformanceCounter("Process", "Working Set", process.ProcessName, true);
            UpdateFields();
        }
        public void UpdateFields()
        {
            try
            {
                Cpu = Math.Round(CounterCpu.NextValue() / _processorCount, 2);
            }
            catch (InvalidOperationException) { }
            try {
                RAM = Math.Round(RAMCounter.NextValue() / _total,2);
            }
            catch (InvalidOperationException) { }
            ThreadNumber = Process.Threads.Count;
        }
        private void SetProcessTime(Process process)
        {
            try
            {
                Time = process.StartTime;
            }
            catch (InvalidOperationException)
            {
            }
            catch (Win32Exception)
            {

            }
        }
        private void SetPathName(Process process)
        {
            try
            {
                Path = process.MainModule.FileName;
            }
            catch (InvalidOperationException )
            {
            }
            catch (Win32Exception)
            {
                Path = "Access is forbidden.";
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ProcessClass other && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
using System;
using AddUp.CommonLogging;

namespace Net6TestApp
{
    internal class Program
    {
        private static readonly ILog log = LogManager.GetLogger<Program>();

        static void Main(string[] args)
        {
            Console.WriteLine("BEGIN");
            log.Trace("Trace Message");
            log.Debug("Debug Message");
            log.Info("Info Message");
            log.Warn("Warn Message");
            log.Error("Error Message");
            log.Fatal("Fatal Message");
            Console.WriteLine("END");
        }
    }
}

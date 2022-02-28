using System;
using System.Diagnostics.CodeAnalysis;

namespace AddUp.CommonLogging.Simple
{
    [SuppressMessage("Critical Code Smell", "S1186:Methods should not be empty", Justification = "Nop")]
    public sealed class NoOpNestedVariablesContext : INestedVariablesContext
    {
        private sealed class NoOpDisposable : IDisposable
        {
            public void Dispose() { }
        }

        public bool HasItems { get; private set; }

        public IDisposable Push(string text) => new NoOpDisposable();
        public string Pop() => null;
        public void Clear() { }
    }
}
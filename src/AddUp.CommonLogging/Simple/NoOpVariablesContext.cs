using System.Diagnostics.CodeAnalysis;

namespace AddUp.CommonLogging.Simple
{
    [SuppressMessage("Critical Code Smell", "S1186:Methods should not be empty", Justification = "Nop")]
    public sealed class NoOpVariablesContext : IVariablesContext
    {
        public void Set(string key, object value) { }
        public object Get(string key) => null;
        public bool Contains(string key) => false;
        public void Remove(string key) { }
        public void Clear() { }
    }
}
using System;

namespace WPF.Models.Interfaces
{
    public interface IDisposableExtended : IDisposable
    {
        bool IsDisposed { get; set; }
    }
}

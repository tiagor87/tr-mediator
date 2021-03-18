using TRMediator.Core.Abstractions;
using TRMediator.Core.Exceptions;

namespace TRMediator.Core.Extensions
{
    internal static class HandlerExtensions
    {
        internal static void EnsureNotNull<THandler>(this THandler handler) where THandler : IHandler
        {
            if (ReferenceEquals(handler, null))
            {
                throw new HandlerNotFoundException<THandler>();
            }
        }
    }
}
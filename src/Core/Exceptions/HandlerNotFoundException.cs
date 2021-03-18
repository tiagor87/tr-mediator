using System;
using TRMediator.Core.Abstractions;

namespace TRMediator.Core.Exceptions
{
    public class HandlerNotFoundException<THandler> : Exception where THandler : IHandler
    {
        private const string MESSAGE = "Handler \"{0}\" was not found.";
        
        public HandlerNotFoundException() : base(string.Format(MESSAGE, typeof(THandler).Name)) 
        {
        }
    }
}
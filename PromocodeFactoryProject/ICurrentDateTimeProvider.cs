using System;

namespace PromocodeFactoryProject
{
    public interface ICurrentDateTimeProvider
    {
        DateTime CurrentDateTime { get; }
    }

    public class CurrentDateTimeProvider : ICurrentDateTimeProvider
    {
        public DateTime CurrentDateTime { get => DateTime.Now ;  }
    }
}
using System;
using InexRef.EventSourcing.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Domain
{
    public class AggregateRootFactory : IAggregateRootFactory
    {
        private readonly IServiceProvider _componentContext;

        public AggregateRootFactory(IServiceProvider componentContext)
        {
            _componentContext = componentContext;
        }

        public TAggregateRoot Create<TAggregateRoot>()
        {
            return _componentContext.GetRequiredService<TAggregateRoot>();
        }
    }
}
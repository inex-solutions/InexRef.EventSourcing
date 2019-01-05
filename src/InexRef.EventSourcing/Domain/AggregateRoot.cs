using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.OperationContext;

namespace InexRef.EventSourcing.Domain
{
    public abstract class AggregateRoot<TId> : IAggregateRoot<TId>, IAggregateRootInternal<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        private bool _isDisposed;

        private readonly List<IEvent<TId>> _uncommittedEvents = new List<IEvent<TId>>();

        protected AggregateRoot(
            IOperationContext operationContext)
        {
            OperationContext = operationContext;
            MessageHandlerInvoker = new MessageHandlerInvoker();
        }

        protected IOperationContext OperationContext { get; }

        private MessageHandlerInvoker MessageHandlerInvoker { get; }

        public TId Id { get; protected set; }

        public int Version { get; protected set; }

        public abstract string Name { get; }

        protected void RegisterEventHandler<TEvent>(Func<TEvent, Task> eventHandler)
            where TEvent : IEvent<TId>
        {
            MessageHandlerInvoker.RegisterMessageHandler(eventHandler);
        }

        private async Task HandleEvent(IEvent<TId> @event)
        {
            ThrowIfDisposed();
            await MessageHandlerInvoker.Invoke(@event);
        }

        protected async Task Apply(IEvent<TId> @event)
        {
            ThrowIfDisposed();
            await HandleEvent(@event);

            if (@event.Version > Version)
            {
                Version = @event.Version;
            }

            if (!OperationContext.IsLoading)
            {
                _uncommittedEvents.Add(@event);
            }
        }

        async Task IAggregateRootInternal<TId>.Load(TId id, IEnumerable<IEvent<TId>> eventHistory)
        {
            ThrowIfDisposed();
            Id = id;
            foreach (var @event in eventHistory)
            {
                await Apply(@event);
            }
        }

        IEnumerable<IEvent<TId>> IAggregateRootInternal<TId>.GetUncommittedEvents()
        {
            ThrowIfDisposed();
            return _uncommittedEvents;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isDisposed = true;
                _uncommittedEvents.Clear();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name, "Aggregate has been disposed and cannot be used further (note: aggregate is automatically disposed on saving)");
            }
        }
    }
}
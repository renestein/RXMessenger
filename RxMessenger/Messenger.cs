using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RxMessenger
{
  public class Messenger : IDisposable
  {
    private readonly Subject<Object> _events;
    private bool _isDisposed;

    public Messenger()
    {
      _events = new Subject<object>();
      _isDisposed = false;
    }

    public void Dispose()
    {
      _events.Dispose();
      _isDisposed = true;
    }

    public void PublishEvent<TEvent>(TEvent newEvent)
    {
      throwIfDisposed();
      _events.OnNext(newEvent);
    }

    public IObservable<TEvent> GetEventStream<TEvent>()
    {
      throwIfDisposed();
      return _events.OfType<TEvent>();
    }

    private void throwIfDisposed()
    {
      if (_isDisposed)
      {
        throw new ObjectDisposedException(GetType().FullName);
      }
    }
  }
}
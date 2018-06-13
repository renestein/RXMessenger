using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace RxMessenger
{
  public class Messenger : IDisposable
  {
    private readonly Subject<Object> _subject;
    private bool _isDisposed;
    private readonly IObservable<object> _events;

    public Messenger()
    {
      _subject = new Subject<object>();
      _events = _subject.ObserveOn(CurrentThreadScheduler.Instance);

      _isDisposed = false;
    }

    public void Dispose()
    {
      _subject.Dispose();
      _isDisposed = true;
    }

    public void PublishEvent<TEvent>(TEvent newEvent)
    {
      throwIfDisposed();
      _subject.OnNext(newEvent);
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
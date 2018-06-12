using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace RxMessenger
{
  public class Messenger : IDisposable
  {
    private readonly Subject<Object> _events;
    private bool _isDisposed;
    private TaskFactory _taskForPublishFactory;

    public Messenger()
    {
      _events = new Subject<object>();
      _isDisposed = false;
      _taskForPublishFactory = new TaskFactory(new ConcurrentExclusiveSchedulerPair().ExclusiveScheduler);
    }

    public void Dispose()
    {
      _events.Dispose();
      _isDisposed = true;
    }

    public void PublishEvent<TEvent>(TEvent newEvent)
    {
      throwIfDisposed();
      _taskForPublishFactory.StartNew(() => _events.OnNext(newEvent),
        TaskCreationOptions.None);
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
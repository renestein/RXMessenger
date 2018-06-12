using System;
using System.Reactive.Linq;
using RxMessenger;
using TicketApplication.Events;

namespace TicketApplication
{
  public abstract class AppEventParticipant
  {
    private IDisposable _quitEvents;

    protected AppEventParticipant(Messenger messenger)
    {
      Messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
    }

    protected Messenger Messenger
    {
      get;
    }

    public virtual void Start()
    {
      _quitEvents = Messenger.GetEventStream<QuitEvent>()
                     .FirstAsync().Subscribe(_ =>
        {
          
          ReleaseSubscriptions();
        });
    }

    public virtual void Stop()
    {
      _quitEvents.Dispose();
    }


    protected abstract void ReleaseSubscriptions();
  }
}
using System;
using RxMessenger;

namespace TicketApplication.Infrastructure
{
  public class Logger : AppEventParticipant
  {
    private IDisposable _allEventsSubscription;

    public Logger(Messenger messenger) : base(messenger)
    {
    }

    public override void Start()
    {
      _allEventsSubscription = Messenger.GetEventStream<Object>().Subscribe();
    }

    protected override void ReleaseSubscriptions()
    {
      _allEventsSubscription.Dispose();
    }
  }
}
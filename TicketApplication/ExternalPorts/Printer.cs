using System;
using RxMessenger;
using TicketApplication.Events;

namespace TicketApplication.ExternalPorts
{
  public class Printer : AppEventParticipant
  {
    private IDisposable _eventSubscription;

    public Printer(Messenger messenger) : base(messenger)
    {
    }

    public override void Start()
    {
      base.Start();
      _eventSubscription = Messenger.GetEventStream<PrintTicketRequestEvent>()
        .Subscribe(e => Messenger.PublishEvent(new PrintTicketResponseEvent()));
    }

    protected override void ReleaseSubscriptions()
    {
      _eventSubscription.Dispose();
    }
  }
}
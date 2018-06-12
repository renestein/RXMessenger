using System;
using System.Reactive.Linq;
using RxMessenger;
using TicketApplication.Events;

namespace TicketApplication.InternalProcessors
{
  public class BuyTicketProcessCoordinator : AppEventParticipant
  {
    private IDisposable _timer;

    public BuyTicketProcessCoordinator(Messenger messenger) : base(messenger)
    {
    }

    public override void Start()
    {
      base.Start();
      Messenger.GetEventStream<Object>()
        .Subscribe(e =>
        {
          switch (e)
          {
            case PrintTicketResponseEvent _:
            {
              publishRestartEvent();
              break;
            }
            case PaymentReceivedEvent pre:
            {
              if (pre.ReceivedAmount == pre.ExpectedAmount)
              {
                disposeTimer();
                Messenger.PublishEvent(new PrintTicketRequestEvent());
              }

              break;
            }
            case TicketSelectedEvent _:
            {
              const int TIMEOUT_IN_SECONDS = 10;
              _timer = Observable.Timer(TimeSpan.FromSeconds(TIMEOUT_IN_SECONDS))
                .Subscribe(_ => Messenger.PublishEvent(new CanceledEvent()));
              break;
            }
            case CanceledEvent _:
            {
              publishRestartEvent();
              break;
            }
          }
        });
      publishRestartEvent();
    }

    protected override void ReleaseSubscriptions()
    {
      disposeTimer();
    }

    private void publishRestartEvent()
    {
      disposeTimer();
      Messenger.PublishEvent(new RestartEvent());
    }

    private void disposeTimer()
    {
      _timer?.Dispose();
    }
  }
}
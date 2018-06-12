using System;
using RxMessenger;
using TicketApplication.Events;

namespace TicketApplication.InternalProcessors
{
  public class PaymentProcessor : AppEventParticipant
  {
    public const int TICKET_PRICE = 20;
    private IDisposable _paymentRequestStream;
    private int _expectedAmount;
    private int _receivedAmount;
    private IDisposable _restartRequestStream;

    public PaymentProcessor(Messenger messenger) : base(messenger)
    {
      clearAmounts();
    }

    public void AddCoin()
    {
      _receivedAmount += 1;
      publishPaymentReceived();
    }
    public override void Start()
    {
      _restartRequestStream = Messenger.GetEventStream<RestartEvent>()
        .Subscribe(_ => clearAmounts());

      _paymentRequestStream = Messenger.GetEventStream<PaymentRequestEvent>()
        .Subscribe(e =>
        {
          _receivedAmount = 0;
          _expectedAmount = e.Price;
          publishPaymentReceived();
        });
    }

    private void clearAmounts()
    {
      _receivedAmount = _expectedAmount = 0;
    }

    protected override void ReleaseSubscriptions()
    {
      _restartRequestStream.Dispose();
      _paymentRequestStream.Dispose();
    }

    private void publishPaymentReceived()
    {
      Messenger.PublishEvent(new PaymentReceivedEvent(_expectedAmount, _receivedAmount));
    }
  }
}
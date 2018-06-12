using System;
using RxMessenger;
using TicketApplication.Events;
using TicketApplication.ExternalPorts;
using TicketApplication.InternalProcessors;
using TicketApplication.Ui;

namespace TicketApplication
{
  public class AppBootstrapper
  {
    private BuyTicketProcessCoordinator _buyTicketProcessor;
    private Display _display;
    private Messenger _messenger;
    private PaymentProcessor _paymentProcessor;
    private Printer _printer;
    private IDisposable _displayDriver;
    private IDisposable _paymentProcessorDriver;

    public void Stop()
    {
      _paymentProcessorDriver.Dispose();
      _displayDriver.Dispose();
      _buyTicketProcessor.Stop();
      _display.Stop();
      _printer.Stop();
      _paymentProcessor.Stop();
      _messenger.Dispose();
    }

    public void Start()
    {
      _messenger = new Messenger();
      _paymentProcessor = new PaymentProcessor(_messenger);
      _paymentProcessor.Start();
      _printer = new Printer(_messenger);
      _printer.Start();
      _display = new Display(_messenger);
      _display.Start();
      _buyTicketProcessor = new BuyTicketProcessCoordinator(_messenger);
       _displayDriver  = _messenger.GetEventStream<RestartEvent>().Subscribe(_ =>
      {
        _display.SelectTicketButtonPressed();
        _display.PaymentButtonPressed();
      });

      var fullPrice = true;
      _paymentProcessorDriver = _messenger.GetEventStream<PaymentReceivedEvent>()
        .Subscribe(e =>
      {
        if (e.ReceivedAmount != 0)
        {
          return;
        }

        var toAddCoins = e.ExpectedAmount;
        ;
        if (!fullPrice)
        {
          toAddCoins = toAddCoins - 1;
        }

        fullPrice = !fullPrice;
        for (var i = 0; i < toAddCoins; i++)
        {
          _paymentProcessor.AddCoin();
        }
      });
      _buyTicketProcessor.Start();
    }
  }
}
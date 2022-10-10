using Livet.Messaging;
using Prism.Mvvm;
using PrismTrialCalculator.Models;
using Reactive.Bindings;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PrismTrialCalculator.ViewModels
{
    public class MainWindowViewModel : BindableBase, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public InteractionMessenger Messenger { get; } = new();

        public ReactiveProperty<string> Title { get; set; } = new("Prism Application");

        public ReactiveProperty<int> Operand1 { get; set; }
        public ReactiveProperty<int> Operand2 { get; set; }
        public ReactivePropertySlim<EOperator> Operator { get; set; } = new();
        public ReactivePropertySlim<int> Result { get; set; } = new();

        public MainWindowViewModel()
        {
            Operand1 = new ReactiveProperty<int>()
                .SetValidateNotifyError(value => value > 100 ? "100以下の数値を入力してください．" : null);
            Operand2 = new ReactiveProperty<int>()
                .SetValidateNotifyError(value => value < 0 ? "0以上の数値を入力してください．" : null);

            CaluculateCommand = Observable.CombineLatest(Operand1.ObserveHasErrors, Operand2.ObserveHasErrors)
               .Select(x => x.All(x => !x))
               .ToReactiveCommand()
               .WithSubscribe(() =>
               {
                   var calculator = new Calculator();
                   try
                   {
                       Result.Value = calculator.Calculate(Operator.Value, Operand1.Value, Operand2.Value);
                   }
                   catch (DivideByZeroException ex)
                   {
                       Messenger.Raise(new InformationMessage(ex.Message, "Errror", "Information"));
                   }
               });
        }

        public ReactiveCommand CaluculateCommand { get; set; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}

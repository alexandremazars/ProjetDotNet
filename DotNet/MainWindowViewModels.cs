using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;

namespace DotNet
{
    internal class MainWindowViewModels : BindableBase
    {
        #region public fields
        public ObservableCollection<IOption> AvailableOptions { get; private set; }
        public string strike { get; set; }
        public DateTime maturity { get; set; }
        public decimal price { get; set; }
        //public ObservableCollection<>

        #endregion

        #region Private fields
        private bool tickerStarted;
        #endregion 

        #region Public Constructors
        public MainWindowViewModels()
        {
            StartCommand = new DelegateCommand(StartTicker, CanStartTicker);
            IOption call = new VanillaCall("Vanilla", new Share("VanillaShare", "1"), new DateTime(2019, 1, 6), 10);
            List<IOption> myOptionsList = new List<IOption>() { call };
            AvailableOptions = new ObservableCollection<IOption>(myOptionsList);
            Console.Write(myOptionsList[0].Name);
        }
        #endregion

        public DelegateCommand StartCommand { get; private set; }

        public bool TickerStarted
        {
            get { return tickerStarted; }
            set
            {
                SetProperty(ref tickerStarted, value);
                StartCommand.RaiseCanExecuteChanged();

            }

        }
        private bool CanStartTicker()
        {
            /*while(strike == null || maturity == null )
            {
                TickerStarted = false;
            }*/
            return TickerStarted;
        }
        private void StartTicker()
        {
            TickerStarted = true;
        }
        Main test = new Main();
    }
}

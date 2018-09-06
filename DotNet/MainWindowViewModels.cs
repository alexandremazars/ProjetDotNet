using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using DotNet.Models;
using PricingLibrary.Utilities.MarketDataFeed;

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


<<<<<<< HEAD
        SimulationModel simulation = new SimulationModel(new VanillaCall("Vanilla", new Share("SOCIETE GENERALE SA", "GLE FP    "), new DateTime(2019, 6, 6), 100),
            new HistoricalDataFeedProvider(), new DateTime(2015, 1, 1), 1);
=======
        SimulationModel simulation = new SimulationModel(new VanillaCall("Vanilla", new Share("VanillaShare", "1"), new DateTime(2019, 6, 6), 8),
            new SimulatedDataFeedProvider(), DateTime.Now, 1);
>>>>>>> a84e30e564cc06c67f6759578a21b26ac5ccd6fd

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
        //Main test = new Main();

    }
}

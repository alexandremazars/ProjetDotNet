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
        public String strike { get; set; }
        public DateTime maturity { get; set; }
        public double price { get; set; }
        public DateTime debutTest { get; set; }
        //public ObservableCollection<>
        public decimal payOffValue { get; set; } 
        public double hedgeValue { get; set; }

        #endregion

        #region Private fields
        private bool tickerStarted;
        #endregion 

        #region Public Constructors
        public MainWindowViewModels()
        {
            StartCommand = new DelegateCommand(StartTicker, CanStartTicker);
            SimulationModel simulation = new SimulationModel(new VanillaCall("Vanilla Call", new Share("VanillaShare", "1"), new DateTime(2019, 6, 6), 8),
            new SimulatedDataFeedProvider(), DateTime.Now, 1);
            IOption call = simulation.Option;
            List<IOption> myOptionsList = new List<IOption>() { call };
            AvailableOptions = new ObservableCollection<IOption>(myOptionsList);
            strike = Convert.ToString(simulation.Strike);
            maturity = simulation.Option.Maturity;
            debutTest = simulation.DateDebut;
            price = simulation.GetRebalancement()[0].prixOption();
            payOffValue = simulation.GetPayoff().Last();
            hedgeValue = simulation.GetCouverture().Last();
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
        //Main test = new Main();

    }
}

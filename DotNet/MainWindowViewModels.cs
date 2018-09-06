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
        #region Private fields
        private Graph selectedClasses;
        #endregion

        #region public fields
        public ObservableCollection<IOption> AvailableOptions { get; private set; }
        public string strike { get; set; }
        public DateTime maturity { get; set; }
        public double price { get; set; }
        public DateTime debutTest { get; set; }
        //public ObservableCollection<>
        public decimal payOffValue { get; set; } 
        public double hedgeValue { get; set; }
        public ObservableCollection<Graph> AvailableClasses { get; private set; }
 


        public SimulationModel simulation;
        #endregion

        #region Private fields
        private bool tickerStarted;
        #endregion 

        #region Public Constructors
        public MainWindowViewModels()
        {
            StartCommand = new DelegateCommand(StartTicker, CanStartTicker);
            simulation = new SimulationModel(new VanillaCall("Vanilla Call", new Share("VanillaShare", "1"), new DateTime(2019, 6, 6), 8),
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
            Graph cl1 = new Graph();
            List<Graph> myList = new List<Graph>() { cl1};
            AvailableClasses = new ObservableCollection<Graph>(myList);
            Graph graph = new Graph();
            graph.GraphSimulation = simulation; 

        }
        #endregion

        public SimulationModel Simulation
        {
            get { return simulation; }
        }
        public Graph SelectedClasses
        {
            get { return selectedClasses; }
            set { SetProperty(ref selectedClasses, value); }
        }

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

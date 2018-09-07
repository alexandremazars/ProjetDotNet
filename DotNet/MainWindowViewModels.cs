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
using System.Windows;
using DotNet.Visualization;
using DotNet.ViewModel;

namespace DotNet
{
    internal class MainWindowViewModels : BindableBase
    {
        #region Private fields
        private Graph selectedClasses;
        private UniverseViewModel universeVM;
        #endregion

        #region public fields
        public ObservableCollection<IOption> AvailableOptions { get; private set; }
        public string strike { get; set; }
        public DateTime maturity { get; set; }
        public double price { get; set; }
        public DateTime debutTest { get; set; }
        public int nbDaysEstim { get; set; }
        public decimal payOffValue { get; set; } 
        public double hedgeValue { get; set; }
        public ObservableCollection<Graph> AvailableClasses { get; private set; }
        public static Graph graphTest { get; set; }

        #endregion

        #region Private fields
        private bool tickerStarted;
        #endregion 

        #region Public Constructors
        public MainWindowViewModels()
        {
            StartCommand = new DelegateCommand(StartTicker, CanStartTicker);
            universeVM = new UniverseViewModel();
            IOption call = Simulation.Option;
            List<IOption> myOptionsList = new List<IOption>() { call };
            AvailableOptions = new ObservableCollection<IOption>(myOptionsList);
            strike = Convert.ToString(Simulation.Strike);
            maturity = Simulation.Option.Maturity;
            debutTest = Simulation.DateDebut;
            nbDaysEstim = Simulation.PlageEstimation;
            price = Simulation.GetRebalancement()[0].prixOption();
            payOffValue = Simulation.GetPayoff().Last();
            hedgeValue = Simulation.GetCouverture().Last();

            /*Graph graph = new Graph();
            graph.setSimulation(simulation);
            List<Graph> myList = new List<Graph>() {graph};
            AvailableClasses = new ObservableCollection<Graph>(myList);*/

            graphTest = GraphTest;
            Window win = new GraphVisualization();
            win.Show();
        }
        #endregion

        public SimulationModel Simulation
        {
            get { return universeVM.Simulation; }
        }

        public Graph GraphTest
        {
            get { return universeVM.GraphVM.Graph; }
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

    }
}

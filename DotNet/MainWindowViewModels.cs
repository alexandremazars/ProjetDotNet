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

        public int nbDaysEstim { get; set; }
        public decimal payOffValue { get; set; } 
        public double hedgeValue { get; set; }
        public static Graph graphTest { get; set; }
        public Window win;

        #endregion

        #region Private fields
        private bool tickerStarted;
        #endregion 

        #region Public Constructors
        public MainWindowViewModels()
        {
            StartCommand = new DelegateCommand(StartTicker, CanStartTicker);
            Console.WriteLine(StartCommand);
            universeVM = new UniverseViewModel();
            IOption call = Simulation.Option;
            List<IOption> myOptionsList = new List<IOption>() { call };
            AvailableOptions = new ObservableCollection<IOption>(myOptionsList);
            strike = Convert.ToString(Simulation.Strike);
            maturity = Simulation.Option.Maturity;


            nbDaysEstim = Simulation.PlageEstimation;
            price = Simulation.GetRebalancement()[0].prixOption();
            payOffValue = Simulation.PayOffaMaturite;
            hedgeValue = Simulation.HedgeMaturity;

            graphTest = GraphTest;
            win = new GraphVisualization();
            win.Show();
        }
        #endregion

        public UniverseViewModel UniverseVM { get { return universeVM; } }

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
        public Window Win
        {
            get { return win; }
            set
            {
                win = value;
                RaisePropertyChanged(nameof(win));
            }
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
            
            return !TickerStarted;
        }
        private void StartTicker()
        {
            universeVM.Simulation = new SimulationModel(new VanillaCall("Vanilla Call", new Share("VanillaShare", "1"), new DateTime(2019, 6, 6), 8),
            new SimulatedDataFeedProvider(), UniverseVM.Initializer.DebutTest, 2);
            win.Close();
            universeVM.UnderlyingUniverse = new Universe(universeVM.Simulation, universeVM.GraphVM.Graph);
            
            // = GraphTest;
            graphTest = universeVM.UnderlyingUniverse.Graph;
            win = new GraphVisualization();
            win.Show();
            TickerStarted = false;

        }

    }
}

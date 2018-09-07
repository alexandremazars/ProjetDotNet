using DotNet.Models;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace DotNet.ViewModel
{
    internal class UniverseViewModel : BindableBase
    {
        #region Private Fields

       // private UniverseFacade facade;
        private GraphViewModel graphVM;
        private SimulationModel simulation;
        private InitializerViewModel initializer;
        private Universe underlyingUniverse;

        #endregion Private Fields

        #region Public Constructors

        public UniverseViewModel()
        {
            initializer = new InitializerViewModel();
            simulation = new SimulationModel(new VanillaCall("Vanilla Call", new Share("VanillaShare", "1"), new DateTime(2019, 6, 6), 8),
            new SimulatedDataFeedProvider(), initializer.debutTest, 2);
            graphVM = new GraphViewModel();
            underlyingUniverse = new Universe(simulation, graphVM.Graph);
            /* facade = new UniverseFacade(underlyingUniverse); */
        }

        #endregion Public Constructors

        #region Public Properties
        public InitializerViewModel Initializer
        {
            get { return initializer; }
            set { initializer = value; }
        }
        /* public UniverseFacade Facade
         {
             get { return facade; }
         }*/

        public SimulationModel Simulation
        {
            get { return simulation; }
            set {
                simulation = value;
                RaisePropertyChanged(nameof(Simulation));
            }
        }

        public Universe UnderlyingUniverse
        {
            get { return underlyingUniverse; }
            set
            {
                underlyingUniverse = value;
                RaisePropertyChanged(nameof(UnderlyingUniverse));
            }
        }

        public GraphViewModel GraphVM
        {
            get { return graphVM; }
        }
        
        #endregion Public Properties

        #region Public Methods

        public void ResetUniverse()
        {
            //Facade.InitializeObservableField();
        }

        #endregion Public Methods
    }
}
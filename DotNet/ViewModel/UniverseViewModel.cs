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

        #endregion Private Fields

        #region Public Constructors

        public UniverseViewModel()
        {
            simulation = new SimulationModel(new VanillaCall("Vanilla Call", new Share("VanillaShare", "1"), new DateTime(2019, 6, 6), 8),
            new SimulatedDataFeedProvider(), DateTime.Now, 2);
            graphVM = new GraphViewModel();
            var underlyingUniverse = new Universe(simulation, graphVM.Graph);
            /* facade = new UniverseFacade(underlyingUniverse); */
        }

        #endregion Public Constructors

        #region Public Properties

        /* public UniverseFacade Facade
         {
             get { return facade; }
         }*/

        public SimulationModel Simulation
        {
            get { return simulation; }
            //set ? 
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
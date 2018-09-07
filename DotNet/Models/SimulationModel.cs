using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Models
{
    class SimulationModel
    {
        private IOption option;
        private IDataFeedProvider dataFeedProvider;

        private DateTime dateDebut;

        private int plageEstimation;

        public SimulationModel(IOption option, IDataFeedProvider dataFeedProvider, DateTime dateDebut, int plageEstimation)
        {
            this.option = option ?? throw new ArgumentNullException("Option should not be null");
            this.dataFeedProvider = dataFeedProvider ?? throw new ArgumentNullException("dataFeed should not be null");
            if (dateDebut == null) { throw new ArgumentNullException("Beginning date should not be null"); }
            this.dateDebut = dateDebut;
            if (plageEstimation < 2) { throw new ArgumentOutOfRangeException("Estimation duration should be upper than 2 days"); }
            this.plageEstimation = plageEstimation;
            ComparaisonOptionCouverture();
        }

        public IOption Option
        {
            get { return option; }
            set { option = value; }
        }

        public IDataFeedProvider DataFeedProvider
        {
            get { return dataFeedProvider; }
            set { dataFeedProvider = value; }
        }

        public double Strike
        {
            get { return option.Strike; }
        }

        public DateTime DateDebut
        {
            get { return dateDebut; }
            set { dateDebut = value; }
        }

        public DateTime DateMaturite
        {
            get { return option.Maturity; }
        }

        public int PlageEstimation
        {
            get { return plageEstimation; }
            set { plageEstimation = value; }
        }

        public RebalancementModel Jour0(DataFeed feedJour0, int periodeRebalancement)
        {
            RebalancementModel couverture = new RebalancementModel(option, dateDebut, (double) feedJour0.PriceList["1"], dataFeedProvider.NumberOfDaysPerYear, periodeRebalancement);

            couverture.NbActifSsJacents = couverture.NbActifSsJacents;
            couverture.ValeurPortefeuille = couverture.prixOption();
            couverture.Liquidite = couverture.Liquidite;

            return couverture;
        }

        public List<decimal> GetPayoff()
        {
            List<decimal> payoffs = new List<decimal>();
            int periodeRebalancement = 1;
            var priceList = dataFeedProvider.GetDataFeed(option, dateDebut);

            for (var i = 1; i < priceList.Count; i+=periodeRebalancement)
            {
                var element = priceList[i];
                decimal spotPrice = element.PriceList["1"];
                payoffs.Add(spotPrice);
            }
            return payoffs;
        }

        public List<RebalancementModel> GetRebalancement()
        {
            List<RebalancementModel> couvertures = new List<RebalancementModel>();
            var priceList = dataFeedProvider.GetDataFeed(option, dateDebut);
            int periodeRebalancement = 1;
            RebalancementModel couverture = Jour0(priceList[0], periodeRebalancement);
            double optionInitiale = couverture.prixOption();

            couvertures.Add(couverture);

            for (var i = 1; i < priceList.Count; i += periodeRebalancement)
            {
                var element = priceList[i];
                decimal spotPrice = element.PriceList["1"];
                RebalancementModel NewCouverture = new RebalancementModel(option, element.Date, (double)spotPrice, 
                    dataFeedProvider.NumberOfDaysPerYear, periodeRebalancement, couverture);
                couverture = NewCouverture;

                couvertures.Add(NewCouverture);
            }

            return couvertures;
        }

        public List<double> GetCouverture()
        {
            List<double> rebalancements = new List<double>();
            foreach (RebalancementModel rebalancement in GetRebalancement()) rebalancements.Add(rebalancement.ValeurPortefeuille);
            return rebalancements;
        }

        public List<double> ComparaisonOptionCouverture()
        {
            var rebalancements = GetRebalancement();
            List<double> comparaisons = new List<double>();

            double optionInitiale = rebalancements[0].prixOption();

            foreach (RebalancementModel rebalancement in rebalancements)
                //comparaisons.Add(Math.Abs(rebalancement.prixOption() - rebalancement.ValeurPortefeuille) / optionInitiale);
                Console.WriteLine(Math.Abs(rebalancement.prixOption() - rebalancement.ValeurPortefeuille) / optionInitiale);

            return comparaisons;
        }
    }
}

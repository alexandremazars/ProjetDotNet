using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Models
{
    class SimulationModel
    {
        #region private field
        private IOption option;
        private IDataFeedProvider dataFeedProvider;

        private DateTime dateDebut;

        private int plageEstimation;
        #endregion

        public SimulationModel(IOption option, IDataFeedProvider dataFeedProvider, DateTime dateDebut, int plageEstimation)
        {
            this.option = option ?? throw new ArgumentNullException("Option should not be null");
            this.dataFeedProvider = dataFeedProvider ?? throw new ArgumentNullException("dataFeed should not be null");
            if (dateDebut == null) { throw new ArgumentNullException("Beginning date should not be null"); }
            this.dateDebut = new DateTime(2012, 9,10);
            if (plageEstimation < 2) { throw new ArgumentOutOfRangeException("Estimation duration should be upper than 2 days"); }
            this.plageEstimation = plageEstimation;
            Estimateur.dispMatrix(Estimateur.getCovMatrix(dataFeedProvider.GetDataFeed(option, this.dateDebut), plageEstimation, this.dateDebut.AddDays(14)));
            if (option.GetType().ToString().EndsWith("VanillaCall"))
                Console.WriteLine("Volatilité: " + Estimateur.Volatilite(dataFeedProvider.GetDataFeed(option, this.dateDebut), plageEstimation, this.dateDebut.AddDays(14), new double[] { 1 }, dataFeedProvider));
            else if (option.GetType().ToString().EndsWith("BasketOption"))
                Console.WriteLine("Volatilité: " + Estimateur.Volatilite(dataFeedProvider.GetDataFeed(option, this.dateDebut), plageEstimation, this.dateDebut.AddDays(14), ((BasketOption) option).Weights, dataFeedProvider));
            Estimateur.dispMatrix(Estimateur.getCorrMatrix(dataFeedProvider.GetDataFeed(option, this.dateDebut), plageEstimation, this.dateDebut.AddDays(14)));
            Console.WriteLine("Correlation moyenne: " + Estimateur.Correlation(dataFeedProvider.GetDataFeed(option, this.dateDebut), plageEstimation, this.dateDebut.AddDays(14)));
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

        public decimal PayOffaMaturite
        {
            get { return this.GetPayOff().Last(); }
        }

        public double HedgeMaturity
        {
            get { return this.GetCouverture().Last(); }
        }
         public double PriceDebut
        {
            get { return this.GetRebalancement()[0].prixOption(); }
        }
        public RebalancementModel Jour0(DataFeed feedJour0, int periodeRebalancement)
        {
            RebalancementModel couverture = new RebalancementModel(option, dateDebut, (double) feedJour0.PriceList["1"], dataFeedProvider.NumberOfDaysPerYear, periodeRebalancement);

            couverture.NbActifSsJacents = couverture.NbActifSsJacents;
            couverture.ValeurPortefeuille = couverture.prixOption();
            couverture.Liquidite = couverture.Liquidite;

            return couverture;
        }

        public List<decimal> GetPayOff()
        {
            List<decimal> payoffs = new List<decimal>();
            int periodeRebalancement = 1;
            var priceList = dataFeedProvider.GetDataFeed(option, dateDebut);

            for (var i = 1; i < priceList.Count; i+=periodeRebalancement)
            {
                var element = priceList[i];
                decimal spotPrice = element.PriceList["1"];
                payoffs.Add(Math.Max(spotPrice-Convert.ToDecimal(Strike), 0));
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
            {

            }
            //comparaisons.Add(Math.Abs(rebalancement.prixOption() - rebalancement.ValeurPortefeuille) / optionInitiale);
            //Console.WriteLine(Math.Abs(rebalancement.prixOption() - rebalancement.ValeurPortefeuille) / optionInitiale);

            return comparaisons;
        }

      
    }
}

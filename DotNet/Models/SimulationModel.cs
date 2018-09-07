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
        private IOption option;
        private IDataFeedProvider dataFeedProvider;

        private DateTime dateDebut;

        private int plageEstimation;

        [DllImport("wre-modeling-c-4.1.dll", EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]

        public static extern int WREmodelingCov(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
            ref int info
        );

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

        public decimal PayOffaMaturite
        {
            get { return this.GetPayoff().Last(); }
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
            {

            }
                //comparaisons.Add(Math.Abs(rebalancement.prixOption() - rebalancement.ValeurPortefeuille) / optionInitiale);
                //Console.WriteLine(Math.Abs(rebalancement.prixOption() - rebalancement.ValeurPortefeuille) / optionInitiale);

            return comparaisons;
        }


        public List<DataFeed> CutDataFeed(int joursDEstimation, DateTime dateActuelle)
        {
            DateTime dateDebutEstimation = dateActuelle.AddDays(-joursDEstimation);
            List<DataFeed> cutData = new List<DataFeed>();
            foreach (var element in dataFeedProvider.GetDataFeed(option, dateDebutEstimation))
            {
                if (element.Date <= dateActuelle)
                {
                    cutData.Add(element);
                }
            }
            return cutData;

        }

        public decimal[,] GetLogReturns(List<DataFeed> dataFeedList)
        {
            CovMatrix matriceCov = new CovMatrix(dataFeedList);
            decimal[,] LogReturnsMatrix;
            LogReturnsMatrix = matriceCov.LogReturns();
            return LogReturnsMatrix;

        }

        public static double[,] computeCovarianceMatrix(double[,] returns)
        {
            int dataSize = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            double[,] covMatrix = new double[nbAssets, nbAssets];
            int info = 0;
            int res;
            res = WREmodelingCov(ref dataSize, ref nbAssets, returns, covMatrix, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREmodelingCov encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREmodelingCov encountred a problem. See info parameter for more details");
            }
            return covMatrix;
        }

      
    }
}

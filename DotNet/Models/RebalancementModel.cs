using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Models
{
    class RebalancementModel
    {
        IOption option;
        DateTime date;
        private double spotPrice;

        RebalancementModel ancienJour;

        private double valeurPortefeuille;
        private double nbActifSsJacents;
        private double liquidite;

        public int nbJourParAn;
        public int periodeRebalancement;

        public RebalancementModel(IOption option, DateTime date, double spotPrice, int nbJourParAn, int periodeRebalancement)
        {
            this.option = option;
            this.date = date;
            this.spotPrice = spotPrice;
            this.nbJourParAn = nbJourParAn;
            this.periodeRebalancement = periodeRebalancement;
            nbActifSsJacents = NbActifSsJacents;
            valeurPortefeuille = ValeurPortefeuille;
            liquidite = Liquidite;
        }

        public RebalancementModel(IOption option, DateTime date, double spotPrice, int nbJourParAn, int periodeRebalancement, RebalancementModel ancienJour)
        {
            this.option = option;
            this.date = date;
            this.spotPrice = spotPrice;
            this.nbJourParAn = nbJourParAn;
            this.periodeRebalancement = periodeRebalancement;
            this.ancienJour = ancienJour;
            nbActifSsJacents = NbActifSsJacents;
            valeurPortefeuille = ValeurPortefeuille;
            liquidite = Liquidite;
        }
        public DateTime Date
        {
            get { return date; }
        }
        public PricingResults pricingResult()
        {
            Pricer pricer = new Pricer();
            return pricer.PriceCall((VanillaCall) option, date, nbJourParAn, spotPrice, 0.4);
        }

        public double prixOption()
        {
            return pricingResult().Price;
        }

        public double SpotPrice
        {
            get { return spotPrice; }
            set { spotPrice = value; }
        }

        public double NbActifSsJacents
        {
            get { return pricingResult().Deltas[0]; }
            set { nbActifSsJacents = value; }
        }

        public double Liquidite
        {
            get { return valeurPortefeuille - nbActifSsJacents * spotPrice; }
            set { liquidite = value; }
        }

        public double ValeurPortefeuille
        {
            get {
                if (ancienJour != null)
                    return ancienJour.nbActifSsJacents * spotPrice + ancienJour.liquidite * RiskFreeRateProvider.
                        GetRiskFreeRateAccruedValue(DayCount.ConvertToDouble(periodeRebalancement, nbJourParAn));
                else return valeurPortefeuille;
            }
            set { valeurPortefeuille = value; }
        }
    }
}

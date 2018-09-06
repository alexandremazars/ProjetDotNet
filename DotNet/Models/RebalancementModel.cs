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
        RebalancementModel ancienRebalancement;

        private double valeurPortefeuille;
        private double nbActifSsJacents;
        private double liquidite;

        public int nbJourParAn;

        public RebalancementModel(IOption option, DateTime date, double spotPrice, int nbJourParAn)
        {
            this.option = option;
            this.date = date;
            this.spotPrice = spotPrice;
            this.nbJourParAn = nbJourParAn;
            nbActifSsJacents = NbActifSsJacents;
            valeurPortefeuille = ValeurPortefeuille;
            liquidite = Liquidite;
        }

        public RebalancementModel(IOption option, DateTime date, double spotPrice, int nbJourParAn, RebalancementModel ancienRebalancement, RebalancementModel ancienJour)
        {
            this.option = option;
            this.date = date;
            this.spotPrice = spotPrice;
            this.nbJourParAn = nbJourParAn;
            this.ancienRebalancement = ancienRebalancement;
            this.ancienJour = ancienJour;
            nbActifSsJacents = NbActifSsJacents;
            valeurPortefeuille = ValeurPortefeuille;
            liquidite = Liquidite;
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
            get {
                if (date.DayOfWeek.ToString() == "Monday" || ancienRebalancement == null) return pricingResult().Deltas[0];
                else return ancienRebalancement.nbActifSsJacents;
            }
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
                {
                    return ancienJour.nbActifSsJacents * spotPrice + ancienJour.liquidite * RiskFreeRateProvider.
                        GetRiskFreeRateAccruedValue(DayCount.ConvertToDouble(
                            DayCount.CountBusinessDays(ancienRebalancement.date, date), nbJourParAn));
                }
                else return valeurPortefeuille;
            }
            set { valeurPortefeuille = value; }
        }
    }
}

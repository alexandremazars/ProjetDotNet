using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
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
        VanillaCall vanillaCall;
        DateTime date;
        double spotPrice;

        RebalancementModel ancienJour;

        public double valeurPortefeuille;
        public double nbActifSsJacents;
        public double liquidite;

        //PricingResults pricingResults;

        public RebalancementModel(VanillaCall vanillaCall, DateTime date, double spotPrice)
        {
            this.vanillaCall = vanillaCall;
            this.date = date;
            this.spotPrice = spotPrice;
        }

        public RebalancementModel(VanillaCall vanillaCall, DateTime date, double spotPrice, RebalancementModel ancienJour)
        {
            this.vanillaCall = vanillaCall;
            this.date = date;
            this.spotPrice = spotPrice;
            this.ancienJour = ancienJour;
        }

        public PricingResults pricingResult()
        {
            Pricer pricer = new Pricer();
            return pricer.PriceCall(vanillaCall, date, 365, spotPrice, 0.4);
        }

        /*public PricingResults PricingResult
        {
            set
            {
                //SetProperty(ref pricingResults, value);
            }
            get
            {
                Pricer pricer = new Pricer();
                return pricer.PriceCall(vanillaCall, date, 365, spotPrice, 0.4);
            }
        }*/

        public double prixOption()
        {
            return pricingResult().Price;
        }

        public double Delta()
        {
            nbActifSsJacents = pricingResult().Deltas[0];
            return nbActifSsJacents;
        }

        public double Liquidite()
        {
            liquidite = valeurPortefeuille - nbActifSsJacents * spotPrice;
            return liquidite;
        }

        public double ValeurPortefeuille()
        {
            return ancienJour.Delta() * spotPrice + ancienJour.Liquidite() * Math.Exp(RiskFreeRateProvider.GetRiskFreeRateAccruedValue(1));
        }

        public void CompareOptionCouverture()
        {
            Console.WriteLine("Prix de l'option: " + prixOption());
            Console.WriteLine("Valeur du portefeuille de couverture: " + Liquidite());
        }
    }
}

using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Models
{
    internal class Portfolio
    {
        #region Public fieds
        public Dictionary<string, decimal> priceAsset;
        public decimal portfolioValue;
        public Dictionary<string, double> composition;
        public decimal liquidity;
        #endregion

        #region Public constructors
        public Portfolio(List<decimal> initialPriceOption, Dictionary<string, double> composition, Dictionary<string, decimal> priceAsset)
        {
            this.priceAsset = priceAsset;
            this.composition = composition;
            this.liquidity = 0;
            this.portfolioValue = 0;
            int i = 1;
            foreach (string id in composition.Keys)
            {
                liquidity += Convert.ToDecimal(composition[id]) * priceAsset[id];
                portfolioValue += initialPriceOption[i];
                i += 1;
            }
        }
        #endregion
        #region public methods
        public void UpdatePortfolioValue(DataFeed priceAsset_t, int nbJourParAn, int periodeRebalancement)
        {
            this.portfolioValue = 0;
            foreach(string id in priceAsset.Keys)
            {
                this.portfolioValue += Convert.ToDecimal(composition[id]) * priceAsset_t.PriceList[id];
            }
            this.portfolioValue += this.liquidity * Convert.ToDecimal(RiskFreeRateProvider.GetRiskFreeRateAccruedValue(DayCount.ConvertToDouble(periodeRebalancement, nbJourParAn)));
        }

        public void UpdateCompo(Dictionary<string, double> compo)
        {
            this.composition = compo;
        }
        #endregion
    }
}

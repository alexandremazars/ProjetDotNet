using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Models
{
    class HistoricalDataFeedProvider : IDataFeedProvider
    {
        public string Name
        {
            get { return "Historical"; }
            set { Name = value; }
        }

        public int NumberOfDaysPerYear
        {
            get { return 252; }
            set { NumberOfDaysPerYear = value; }
        }

        public List<DataFeed> GetDataFeed(IOption option, DateTime from)
        {
            return BDModel.getDBDataFeed(option, from);
        }

        public DateTime GetMinDate()
        {
            return BDModel.getDBMinDate();
        }
    }
}

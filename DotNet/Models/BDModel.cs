using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Models
{
    static class BDModel
    {
        static string connectionString = "Data Source=ingefin.ensimag.fr;Initial Catalog=DotNetDB;User ID=etudiant;Password=edn!2015";

        public static List<DataFeed> getDBDataFeed(IOption option, DateTime from)
        {
            List<DataFeed> feeds = new List<DataFeed>();

            using (SqlConnection connection1 = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;

                cmd.CommandText = "SELECT DISTINCT * FROM HistoricalShareValues WHERE date >= @param1 ORDER BY date, id";
                cmd.Parameters.Add(new SqlParameter("@param1", from));
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection1;

                connection1.Open();

                reader = cmd.ExecuteReader();

                string id; DateTime lastDate = from; DateTime date; decimal value;
                Dictionary<string, decimal> priceList = new Dictionary<string, decimal>(); 

                while (reader.Read())
                {
                    id = !reader.IsDBNull(0) ? reader.GetString(0) : "";
                    date = !reader.IsDBNull(1) ? reader.GetDateTime(1) : new DateTime(0);
                    value = !reader.IsDBNull(2) ? reader.GetDecimal(2) : 0;

                    if (date != lastDate)
                    {
                        DataFeed feed = new DataFeed(lastDate, priceList);
                        feeds.Add(feed);
                        priceList = new Dictionary<string, decimal>();
                        lastDate = date;
                    }

                    if (option.UnderlyingShareIds.Contains(id)) priceList.Add(id, value);
                }

                connection1.Close();

                return feeds;
            }
        }

        public static DateTime getDBMinDate()
        {
            using (SqlConnection connection1 = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;

                cmd.CommandText = "SELECT MIN(date) FROM HistoricalShareValues";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection1;

                connection1.Open();

                reader = cmd.ExecuteReader();

                if (reader.Read()) return (!reader.IsDBNull(0) ? reader.GetDateTime(0) : new DateTime(0));

                return new DateTime(0);
            }
        }
    }
}

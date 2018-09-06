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

                cmd.CommandText = "SELECT DISTINCT date FROM HistoricalShareValues WHERE date > @param1 ORDER BY date";
                cmd.Parameters.Add(new SqlParameter("@param1", from));
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection1;

                connection1.Open();

                List<DateTime> dates = new List<DateTime>();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DateTime date = !reader.IsDBNull(0) ? reader.GetDateTime(0) : new DateTime(0);

                    dates.Add(date);
                }

                reader.Close();

                foreach(DateTime date in dates)
                {
                    SqlCommand cmd2 = new SqlCommand();
                    SqlDataReader reader2;

                    cmd2.CommandText = "SELECT DISTINCT value FROM HistoricalShareValues WHERE date=@param1 AND id=@param2";
                    cmd2.Parameters.Add(new SqlParameter("@param1", date));
                    cmd2.Parameters.Add(new SqlParameter("@param2", option.UnderlyingShareIds[0]));
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = connection1;

                    Dictionary<string, decimal> priceList = new Dictionary<string, decimal>();

                    reader2 = cmd2.ExecuteReader();

                    while (reader2.Read())
                    {
                        decimal value = !reader2.IsDBNull(0) ? reader2.GetDecimal(0) : 0;
                        priceList.Add(option.UnderlyingShareIds[0], value);
                    }

                    feeds.Add(new DataFeed(date, priceList));

                    reader2.Close();
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

                cmd.CommandText = "SELECT DISTINCT date FROM HistoricalShareValues ORDER BY date";
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

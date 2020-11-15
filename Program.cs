using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Newtonsoft.Json;

namespace StockAnalyzing
{
    class Program
    {   //reading from .csv file
        public static List<StockRowDetails> DataFromCSV()
        {
            List<StockRowDetails> result;
            using (TextReader fileReader = File.OpenText("Task2.csv"))
            {
                var csvReader = new CsvReader(fileReader, CultureInfo.CurrentCulture);
                csvReader.Configuration.HasHeaderRecord = false;
                csvReader.Read();
                result = csvReader.GetRecords<StockRowDetails>().ToList();
            }
            return result;
        }
        // function for separating rows that belongs only to one stock ID. 
        public static List<StockRowDetails> stockForId(int id, List<StockRowDetails> allDetails)
        {
            List<StockRowDetails> result = new List<StockRowDetails>();
            for (int i = 0; i < allDetails.Count(); i++)
            {
                if (id == allDetails[i].ID)
                {
                    result.Add(allDetails[i]);
                }
            }
            return result;
        }
        public static List<string> maxProfit(List<StockRowDetails> stockDetailsById)
        {
            double profit = 0;
            double max = 0;
            string lowestPrice = null;
            string highestPrice = null;
            string buyingTime = null;
            string sellingTime = null;
            List<string> result = new List<string>();
            for (int i = 0; i < stockDetailsById.Count() - 1; i++)
            {
                for (int j = i + 1; j < stockDetailsById.Count(); j++)
                {
                    profit = stockDetailsById[j].Price - stockDetailsById[i].Price;
                    if (max < profit)
                    {
                        max = profit;
                        lowestPrice = stockDetailsById[i].Price.ToString();
                        highestPrice = stockDetailsById[j].Price.ToString();
                        buyingTime = stockDetailsById[i].Time.ToString();
                        sellingTime = stockDetailsById[j].Time.ToString();
                    }
                }
            }
            result.Add($"Maximum profit for stock ID {stockDetailsById[0].ID}: {max.ToString()}");
            result.Add("Minimum buy price: " + lowestPrice);
            result.Add("Max buy price: " + highestPrice);
            result.Add("Best time for buying a stock: " + buyingTime);
            result.Add("Best time for selling a stock: " + sellingTime);
            return result;

        }
        static void Main(string[] args)
        {
            List<StockRowDetails> dataFromCSV = DataFromCSV();

            List<int> stockId = new List<int>();

            foreach (StockRowDetails details in dataFromCSV)
            {
                stockId.Add(details.ID);
            }

            // list with only distinct stock IDs
            List<int> distinctStockId = stockId.Distinct().ToList();
            
            //sorting by time
            var orderedData = dataFromCSV.OrderBy(r => r.Time).ToList();
            
            List<StockRowDetails> chosenRowsForId = new List<StockRowDetails>();
            List<string> maximumProfit = new List<string>();
            foreach (var id in distinctStockId)
            {
                // "chosenRowsForId" list is also ordered by time
                
                chosenRowsForId = stockForId(id, orderedData);
                maximumProfit = maxProfit(chosenRowsForId);
                foreach (var item in maximumProfit)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("----------------------------------------------");
            }
        }
    }
}

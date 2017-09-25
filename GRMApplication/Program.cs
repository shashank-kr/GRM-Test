using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DataMapper;
using Repository;

namespace GRMApplication
{
    class Program
    {
		//Input data file location
        private const string musicContractDataFilePath = @"data\MusicContracts.txt";
        private const string distributionPartnerDataFilePath = @"data\DistributionPartners.txt";
        private const bool skipHeader = true;

        static void Main(string[] args)
        {
            Console.WriteLine("GRM Developer Test." + Environment.NewLine + "Author: Shashank Kumar");
            
			//load data files into data mappers
            DistributionPartnerDataMapper distributionPartnerDataMapper = new DistributionPartnerDataMapper();
            MusicContractDataMapper musicContractDataMapper = new MusicContractDataMapper();

            List<string> streamErrors = new List<string>();
			Console.WriteLine("Loading data... ");

			try
            {
                //Load input data file into memory.
                using (FileStream fs = File.OpenRead(musicContractDataFilePath))
                {
                    //skip the headers in the first line 
                    musicContractDataMapper.LoadData(fs, skipHeader,out streamErrors);
                    if (streamErrors.Any())
                    {
                        Console.WriteLine("Data problems reading file '{0}':", musicContractDataFilePath);
                        streamErrors.ForEach(Console.WriteLine);
                    }
                }
                
                streamErrors.Clear();
                using (FileStream fs = File.OpenRead(distributionPartnerDataFilePath))
                {
                    distributionPartnerDataMapper.LoadData(fs, skipHeader,out streamErrors);
                    if (streamErrors.Any())
                    {
                        Console.WriteLine("Data problems reading file '{0}':", distributionPartnerDataFilePath);
                        streamErrors.ForEach(Console.WriteLine);
                    }
                }
                Console.WriteLine("Done.");

                var repo = new GRMRepository(distributionPartnerDataMapper, musicContractDataMapper);
                //listen for query.
                Console.WriteLine("Please enter your query, Enter Q to quit:");
                var query = Console.ReadLine();
                while (query != "Q")
                {
                    //parse query, first space indicates separator
                    int pos = query.IndexOf(' ');
                    if (pos > 0)
                    {
                        string distributor = query.Substring(0, pos).Trim();
                        string dateStr = query.Substring(pos).Trim();
                        //Console.WriteLine("Query: Partner = {0}, Date = {1}", distributor, ParseDate(dateStr).ToString());
                        var results = repo.Query(distributor, ParseDate(dateStr));
                        results.ForEach(Console.WriteLine);
                        Console.WriteLine("Query completed. Enter another query, or type Q to exit.");
                        query = Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Query. Please try again.");
                        query = Console.ReadLine();
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(-1);
            }
            
            Environment.Exit(0);
        }

        private static DateTime ParseDate(string dateStr)
        {
            DateTime date;

            if (string.IsNullOrEmpty(dateStr))
            {
                throw new FormatException("Date format was invalid.");;
            }

            var replaced = dateStr.Substring(0, 4)
                                     .Replace("nd", "")
                                     .Replace("th", "")
                                     .Replace("rd", "")
                                     .Replace("st", "")
                                     + dateStr.Substring(4);
           
			bool parsed =
				DateTime.TryParseExact(replaced, "d MMM yyyy",
					CultureInfo.InvariantCulture, DateTimeStyles.None,
					out date);

			if (!parsed)
            {
				parsed = DateTime.TryParseExact(replaced, "d MMMM yyyy",
					CultureInfo.InvariantCulture, DateTimeStyles.None,
					out date);


				if (!parsed)
                    throw new FormatException("Date format was invalid.");
            }

            return date;
        }
    }
}

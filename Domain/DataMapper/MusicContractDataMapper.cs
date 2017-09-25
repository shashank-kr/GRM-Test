using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.DataMapper
{
    public class MusicContractDataMapper : DataMapper<MusicContract>
    {
        private List<MusicContract> _data = new List<MusicContract>(); 

        public override void LoadData(Stream sourceStream, bool skipHeader, out List<string> loadErrors)
        {
            loadErrors = new List<string>();
            using (TextReader tr = new StreamReader(sourceStream))
            {
                string[] rowData;
                if (skipHeader)
                    tr.ReadLine();

                while ((rowData = this.SplitSourceData(tr.ReadLine())) != null)
                {
                    try
                    {
                        _data.Add(new MusicContract()
                        {
                            //Artist|Title|Usages|StartDate|EndDate
                            Artist = rowData[0],
                            Title = rowData[1],
                            Usages = rowData[2].Split(',').Select(s => s.Trim()).ToArray(),
                            //trim any whitespace from each element,
                            StartDate = ParseDate(rowData[3]),
                            EndDate = ParseDate(rowData[4]) //?? DateTime.MaxValue 
                        });
                    }
                    catch (InvalidDataException ide)
                    {
                        loadErrors.Add( string.Join("|",rowData));
                    }
                }
                
                tr.Close();
            } 
            

        }

        public override List<MusicContract> GetData()
        {
            return _data;
        }



        private DateTime? ParseDate(string dateStr)
        {
            DateTime date;

            if (string.IsNullOrEmpty(dateStr))
            {
                return null;
            }

            var replaced = dateStr.Substring(0, 4)
                                     .Replace("nd", "")
                                     .Replace("th", "")
                                     .Replace("rd", "")
                                     .Replace("st", "")
                                     + dateStr.Substring(4);
            bool parsed =
                DateTime.TryParseExact(replaced, "d MMM yyyy",
                    CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal,
                    out date);

            if (!parsed )
            {
                parsed = DateTime.TryParseExact(replaced, "d MMMM yyyy",
                    CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal,
                    out date);

                if(!parsed)
                    throw new FormatException("Date format was invalid.");
            }
            
            return date;
        }

    }
}

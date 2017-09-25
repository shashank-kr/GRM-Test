using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.DataMapper
{
    public class DistributionPartnerDataMapper : DataMapper<DistributionPartner>
    {

        private readonly List<DistributionPartner> _data = new List<DistributionPartner>(); 

        public override void LoadData(Stream sourceStream, bool skipHeader, out List<string> loadErrors)
        {
            loadErrors = new List<string>();
            using (TextReader tr = new StreamReader(sourceStream))
            {
                string[] rowData = new string[] {};
                try
                {
                    if (skipHeader)
                    {
                        tr.ReadLine();
                    }

                    while ((rowData = this.SplitSourceData(tr.ReadLine())) != null)
                    {
                        _data.Add(new DistributionPartner()
                        {
                            //Partner|Usage
                            Partner = rowData[0],
                            Usages = rowData[1].Split(',').Select(s => s.Trim()).ToArray()
                            //trim any whitespace from each element
                        });
                    }
                }
                catch (InvalidDataException ide)
                {
                    loadErrors.Add( ide.Message );
                }

                    tr.Close();

            }
        }


        public override List<DistributionPartner> GetData()
        {
            return _data;
        }

    }
}

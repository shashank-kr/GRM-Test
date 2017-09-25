using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class DistributionPartner
    {
        public string Partner { get; set; }
        public string[] Usages { get; set; }

        public override string ToString()
        {
            return Partner + "|" + string.Join(",", Usages);
        }
    }
}

/*
Partner|Usage

ITunes|digital download

YouTube|streaming
*/

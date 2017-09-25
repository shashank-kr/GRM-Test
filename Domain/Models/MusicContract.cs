using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class MusicContract
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string[] Usages { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override string ToString()
        {
            return Artist + "|" + Title + "|" + string.Join(",", Usages) + "|" + StartDate.Value.ToString("d MMM yyyy") + "|" + ((EndDate.HasValue) ? EndDate.Value.ToString("d MMM yyyy") : "");
        }
    }
}

/*

Artist|Title|Usages|StartDate|EndDate

Tinie Tempah|Frisky (Live from SoHo)|digital download, streaming|1st Feb 2012|

Tinie Tempah|Miami 2 Ibiza|digital download|1st Feb 2012|

Tinie Tempah|Till I'm Gone|digital download|1st Aug 2012|

Monkey Claw|Black Mountain|digital download|1st Feb 2012|

Monkey Claw|Iron Horse|digital download, streaming|1st June 2012|

Monkey Claw|Motor Mouth|digital download, streaming|1st Mar 2011|

Monkey Claw|Christmas Special|streaming|25st Dec 2012|31st Dec 2012
*/

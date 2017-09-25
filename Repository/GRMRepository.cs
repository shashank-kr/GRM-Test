using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DataMapper;
using Domain.Models;

namespace Repository
{
    public class GRMRepository
    {
        private List<MusicContract> _musicContracts;
        private List<DistributionPartner> _distributionPartners;

        public GRMRepository(DistributionPartnerDataMapper distributionPartnerDataMapper, MusicContractDataMapper musicContractDataMapper)
        {
            _musicContracts = musicContractDataMapper.GetData() ?? null;
            _distributionPartners = distributionPartnerDataMapper.GetData() ?? null;
        }

        //query
        public List<MusicContract> Query(string distributor, DateTime theDate)
        {
            var query = (from d in _distributionPartners
                         from c in _musicContracts
                         where String.Equals(d.Partner, distributor,StringComparison.InvariantCultureIgnoreCase)
                         where c.Usages.Intersect(d.Usages, StringComparer.InvariantCultureIgnoreCase).Any()
                         where c.StartDate <= theDate
                         where (!c.EndDate.HasValue || c.EndDate >= theDate)
                         select c);

            return query.ToList();
        }

    }
}

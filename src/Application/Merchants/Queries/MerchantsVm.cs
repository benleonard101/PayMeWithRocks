using System.Collections.Generic;

namespace PayMeWithRocks.Application.Merchants.Queries
{
    public class MerchantsVm
    {
        public IList<MerchantDto> Merchants { get; set; }
    }
}
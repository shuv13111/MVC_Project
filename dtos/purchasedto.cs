using ShoeTrackr.Models;

namespace ShoeTrackr.dtos
{
    public class purchasedto
    {
        public List<PurchaseModel> purchasesList { get; set; }

        public PurchaseModel purchase { get; set; }
    }
}

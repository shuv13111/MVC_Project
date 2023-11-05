using Microsoft.AspNetCore.Routing.Constraints;

namespace ShoeTrackr.Models
{
    public class SalesModel
    {
        public int Id { get; set; }

        public string company { get; set; }

        public DateTime date { get; set; }

        public int netPrice { get; set; }

        public double netQuantity { get; set; }

        public virtual List<SaleItemModel>? SaleItemModels { get; set; }
    }
}

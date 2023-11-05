using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ShoeTrackr.Models
{
    public class ItemModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int price { get; set; }

        public int quantity { get; set; }

        public double rating { get; set; }

        public virtual List<SaleItemModel>? SaleItemModels { get; set; }

        public virtual List<PurchaseModel> purchaseModels { get; set; }

    }

    public class ItemModelConfiguration : IEntityTypeConfiguration<ItemModel>
    {
        public void Configure(EntityTypeBuilder<ItemModel> builder)
        {
            builder.HasKey(x => new { x.Id });

            
        }

    }
}

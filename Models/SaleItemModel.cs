using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ShoeTrackr.Models
{
    public class SaleItemModel
    {
        public int SalesItemId { get; set; }

        public int sale_id { get; set; }

        public int item_id { get; set; }

        public double sale_price { get; set; }

        public int sale_quantity { get; set; }

        public SalesModel sales { get; set; }

        public ItemModel items { get; set; }
    }

    public class SaleItemModelConfiguration : IEntityTypeConfiguration<SaleItemModel>
    {
        public void Configure(EntityTypeBuilder<SaleItemModel> builder)
        {
            builder.HasKey(x => new { x.SalesItemId });

            builder.HasOne(x => x.sales).WithMany(x => x.SaleItemModels).HasForeignKey(x => x.sale_id).OnDelete(DeleteBehavior.ClientSetNull); ;

            builder.HasOne(x => x.items).WithMany(x => x.SaleItemModels).HasForeignKey(x => x.item_id).OnDelete(DeleteBehavior.ClientSetNull); ;
        }

    }
}

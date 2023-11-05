using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ShoeTrackr.Models
{
    public class PurchaseModel
    {
        public int Id { get; set; }

        public int item_id { get; set; }
        public string Supplier { get; set; }

        public int quantity { get; set; }

        public DateTime purchaseDate { get; set; }

     
        public virtual ItemModel Item { get; set; }

    }

    public class PurchaseModelConfiguration : IEntityTypeConfiguration<PurchaseModel>
    {
        public void Configure(EntityTypeBuilder<PurchaseModel> builder)
        {
            builder.HasKey(x => new { x.Id });

            builder.HasOne(x => x.Item).WithMany(x => x.purchaseModels).HasForeignKey(x => x.item_id).OnDelete(DeleteBehavior.ClientSetNull);
        }

    }
}

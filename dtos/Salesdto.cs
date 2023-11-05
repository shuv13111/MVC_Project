using ShoeTrackr.Models;
using System;
using System.Resources;

namespace ShoeTrackr.dtos
{
    public class Salesdto
    {
        public List<SalesViewModel> saleslist { get; set; }

        public SalesViewModel SalesModel { get; set; }

    }

    public class SalesViewModel
    { 
        public int Id { get; set; }
        public string salesName { get; set; }

        public string itemsIdList { get; set; }

        public string quantityList { get; set; }

        public int quantity { get; set; }

        public int saleId { get; set; }

        public int price { get; set; }

        public DateTime date { get; set; }
        public List<SaleItemModel> ?saleitems { get; set; }
    
    }
}

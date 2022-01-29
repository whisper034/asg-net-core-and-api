using System;

namespace Jenshin.Impack.API.Model.Request
{
    public class DetailPurchaseItemDTO
    {
        public Guid ItemID { get; set; }
        public int? ItemPrice { get; set; }
        public bool? GenesisCrystalOnly { get; set; }
    }
}

using System;

namespace Jenshin.Impack.API.Model.Request
{
    public class DetailPurchaseUserDTO
    {
        public Guid UserID { get; set; }
        public int UserPrimogemAmount { get; set; }
        public int UserGenesisCrystalAmount { get; set; }
    }
}

using Jenshin.Impack.API.Model;
using Binus.WS.Pattern.Entities;
using Binus.WS.Pattern.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Jenshin.Impack.API.Output;

namespace Jenshin.Impack.API.Helper
{
    public class ItemHelper
    {
        public static int AddNewItem(MsShopItem data)
        {
            try
            {
                EntityHelper.Add(new MsShopItem()
                {
                    ItemName = data.ItemName,
                    ItemDescription = data.ItemDescription,
                    ItemPrice = data.ItemPrice,
                    GenesisCrystalOnly = data.GenesisCrystalOnly,
                    Stsrc = "A", // "A" apabila ada barangnya, "D" apabila tidak ada barangnya
                    CreatedDt = DateTime.Now,
                    CreatedBy = "2440067175",
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 1;
        }


        public static int UpdateItem(MsShopItem data)
        {
            var returnValue = new MsShopItem();
            var msShopItem = EntityHelper.Get<MsShopItem>().ToList();

            try
            {
                returnValue = (
                    from msi in msShopItem.Where(dataRow => dataRow.ItemID == data.ItemID)
                    select new MsShopItem()
                    {
                        ItemID = msi.ItemID,
                        ItemName = msi.ItemName,
                        ItemDescription = msi.ItemDescription,
                        ItemPrice = msi.ItemPrice,
                        GenesisCrystalOnly = msi.GenesisCrystalOnly,
                        Stsrc = msi.Stsrc,
                        UpdatedDt = msi.UpdatedDt,
                        UpdatedBy = msi.UpdatedBy,
                        CreatedDt = msi.CreatedDt,
                        CreatedBy = msi.CreatedBy,
                    }).FirstOrDefault();

                if (data.ItemID.Equals(new Guid())) // kalau tidak memasukkan ID, nanti system akan return ID dalam bentuk angka 0, maka pakai cara ini
                {
                    throw new Exception("Item ID must be provided!");
                }

                EntityHelper.Update(new MsShopItem()
                {
                    ItemID = data.ItemID,
                    ItemName = data.ItemName ?? returnValue.ItemName,
                    ItemDescription = data.ItemDescription ?? returnValue.ItemDescription,
                    ItemPrice = data.ItemPrice ?? returnValue.ItemPrice,
                    GenesisCrystalOnly = data.GenesisCrystalOnly ?? returnValue.GenesisCrystalOnly,
                    Stsrc = data.Stsrc ?? returnValue.Stsrc,
                    UpdatedDt = DateTime.Now,
                    UpdatedBy = "2440067175",
                    CreatedDt = data.CreatedDt ?? returnValue.CreatedDt,
                    CreatedBy = data.CreatedBy ?? returnValue.CreatedBy,

                    // value1 ?? value2 --> apabila value yang kiri null maka masukkan yang kanan
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return 1;
        }
    }
}
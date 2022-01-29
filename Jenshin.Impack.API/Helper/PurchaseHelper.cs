using Jenshin.Impack.API.Model;
using Binus.WS.Pattern.Entities;
using Binus.WS.Pattern.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Jenshin.Impack.API.Output;
using Jenshin.Impack.API.Model.Request;

namespace Jenshin.Impack.API.Helper
{
    public class PurchaseHelper
    {
        public static string PurchaseItem(PurchaseRequestDTO data)
        {
            string message = "";

            var detailPurchaseUser = new DetailPurchaseUserDTO();
            var detailPurchaseItem = new DetailPurchaseItemDTO();
            var msUser = EntityHelper.Get<MsUser>().ToList();
            var msUserBalance = EntityHelper.Get<MsUserBalance>().ToList();
            var msShopItem = EntityHelper.Get<MsShopItem>().ToList();
            var trUserPurchase = EntityHelper.Get<TrUserPurchase>().ToList();

            try
            {
                detailPurchaseUser = (
                    from mu in msUser.Where(dataRow => dataRow.UserID == data.UserID)
                    join mub in msUserBalance on mu.UserID equals mub.UserID
                    select new DetailPurchaseUserDTO()
                    {
                        UserID = mu.UserID,
                        UserPrimogemAmount = mub.UserPrimogemAmount,
                        UserGenesisCrystalAmount = mub.UserGenesisCrystalAmount,
                    }).FirstOrDefault();

                detailPurchaseItem = (
                    from msi in msShopItem.Where(dataRow => dataRow.ItemID == data.ItemID)
                    select new DetailPurchaseItemDTO()
                    {
                        ItemID = msi.ItemID,
                        ItemPrice = msi.ItemPrice,
                        GenesisCrystalOnly = msi.GenesisCrystalOnly,
                    }).FirstOrDefault();

                var totalItemPrice = detailPurchaseItem.ItemPrice * data.Amount;

                if ((bool)detailPurchaseItem.GenesisCrystalOnly) // kalo itemnya cuma bisa dibeli pake genesis crystal
                {
                    if(totalItemPrice > detailPurchaseUser.UserGenesisCrystalAmount) 
                    {
                        message = "Insufficient Funds";
                    }
                    else
                    {
                        message = "Purchase Successful!";

                        EntityHelper.Update(new MsUserBalance
                        {
                            UserID = detailPurchaseUser.UserID,
                            UserPrimogemAmount = detailPurchaseUser.UserPrimogemAmount,
                            UserGenesisCrystalAmount = (int)(detailPurchaseUser.UserGenesisCrystalAmount - totalItemPrice),
                        });

                        EntityHelper.Add(new TrUserPurchase()
                        {
                            UserID = data.UserID,
                            ItemID = data.ItemID,
                            PurchaseAmount = data.Amount,
                        });
                    }
                }
                else // kalau itemnya bisa dibeli pake primogem dan/atau genesis crystal
                {
                    if (totalItemPrice > detailPurchaseUser.UserPrimogemAmount) // kalau total primogemnya tidak cukup
                    {
                        // tambahkan total primogem yang dipunyai dengan total genesis crystal yang dipunyai:
                        var userTotalBalance = detailPurchaseUser.UserPrimogemAmount + detailPurchaseUser.UserGenesisCrystalAmount;

                        if (totalItemPrice > userTotalBalance) // kalau masih tidak cukup juga
                        {
                            message = "Insufficient Funds";
                        }
                        else
                        {
                            message = "Purchase Successful!";

                            EntityHelper.Update(new MsUserBalance
                            {
                                UserID = detailPurchaseUser.UserID,
                                UserPrimogemAmount = 0,
                                UserGenesisCrystalAmount = (int)(userTotalBalance - totalItemPrice),
                            });

                            EntityHelper.Add(new TrUserPurchase()
                            {
                                UserID = data.UserID,
                                ItemID = data.ItemID,
                                PurchaseAmount = data.Amount,
                            });
                        }
                    }
                    else
                    {
                        message = "Purchase Successful!";

                        EntityHelper.Update(new MsUserBalance
                        {
                            UserID = detailPurchaseUser.UserID,
                            UserPrimogemAmount = (int)(detailPurchaseUser.UserPrimogemAmount - totalItemPrice),
                            UserGenesisCrystalAmount = detailPurchaseUser.UserGenesisCrystalAmount,
                        });

                        EntityHelper.Add(new TrUserPurchase()
                        {
                            UserID = data.UserID,
                            ItemID = data.ItemID,
                            PurchaseAmount = data.Amount,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return message;
        }
    }
}
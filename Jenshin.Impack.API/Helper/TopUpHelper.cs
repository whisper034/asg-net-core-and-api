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
    public class TopUpHelper
    {
        public static string TopUp(TopUpRequestDTO data)
        {
            string message = "";
            var returnValue = new SpecificUser();
            var msUser = EntityHelper.Get<MsUser>().ToList();
            var msUserBalance = EntityHelper.Get<MsUserBalance>().ToList();

            try
            {
                returnValue = (
                    from mu in msUser.Where(dataRow => dataRow.UserEmail == data.Email)
                    join mub in msUserBalance on mu.UserID equals mub.UserID
                    select new SpecificUser
                    {
                        ID = mu.UserID,
                        Name = mu.UserName,
                        Primogem = mub.UserPrimogemAmount,
                        GenesisCrystal = mub.UserGenesisCrystalAmount,
                    }).FirstOrDefault();

                if(returnValue == null)
                {
                    throw new Exception("User Not Found!");
                }

                EntityHelper.Update(new MsUserBalance
                {
                    UserID = returnValue.ID,
                    UserPrimogemAmount = returnValue.Primogem,
                    UserGenesisCrystalAmount = returnValue.GenesisCrystal + data.Amount,
                });

                message = data.Amount + " Genesis Crystals has been topped up to " + data.Email;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return message;
        }
    }
}
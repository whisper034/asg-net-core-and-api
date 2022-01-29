using Jenshin.Impack.API.Model;
using Binus.WS.Pattern.Entities;
using Binus.WS.Pattern.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Jenshin.Impack.API.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Jenshin.Impack.API.Helper
{
    public class UserHelper
    {
        public static List<User> GetAllUser()
        {
            var returnValue = new List<User>();
            var Users = EntityHelper.Get<MsUser>().ToList();

            try
            {
                returnValue = Users.Select(
                    x => new User
                    {
                        Name = x.UserName,
                        AdventureRank = x.UserAdventureRank,
                        Email = x.UserEmail,
                        Signature = x.UserSignature,
                    }
                ).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return returnValue;
        }


        public static List<SpecificUser> GetSpecificUser(string Email, string Username)
        {
            var returnValue = new List<SpecificUser>();
            var msUser = EntityHelper.Get<MsUser>().ToList();
            var msUserBalance = EntityHelper.Get<MsUserBalance>().ToList();

            try
            {
                // Referensi:
                // http://www.codingfusion.com/Post/How-to-Join-tables-and-return-result-into-view-usi
                // https://www.tutorialsteacher.com/linq/linq-joining-operator-join
                // join tabelnya mirip di sql, tetapi FROM dan SELECT-nya itu dibalik aja

                returnValue = (
                    from mu in msUser.Where(dataRow => dataRow.UserEmail == Email || dataRow.UserName == Username)
                    join mub in msUserBalance on mu.UserID equals mub.UserID
                    select new SpecificUser
                    {
                        ID = mu.UserID,
                        Name = mu.UserName,
                        Primogem = mub.UserPrimogemAmount,
                        GenesisCrystal = mub.UserGenesisCrystalAmount,
                    }).ToList();

                if (!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(Username)) // kalau kedua parameternya terisi
                {
                    throw new Exception("Please input either UserName OR UserEmail.");
                }
                else if (String.IsNullOrEmpty(Email) && String.IsNullOrEmpty(Username)) // kalau tidak ada parameter yang terisi
                {
                    throw new Exception("Parameter must be filled!");
                }
                else if (returnValue.Capacity.Equals(0)) // kalau akunnya tidak ditemukan
                {
                    throw new Exception("Account not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return returnValue;
        }
    }
}
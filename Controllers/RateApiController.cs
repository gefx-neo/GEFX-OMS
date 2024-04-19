using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using Newtonsoft.Json;

namespace GreatEastForex.Controllers
{
    public class RateApiController : Controller
    {
        // GET: RateApi
        public void getRate()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    DateTime now = DateTime.Now;
                    string strNow = now.ToString("yyyy-MM-ddThh:mm:ssZ");
                    var buffer = Encoding.ASCII.GetBytes("greateastforex45086039:j26mueau2eas4a0pm031e3svln");
                    var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(buffer));
                    client.DefaultRequestHeaders.Authorization = authHeader;
                    var task = client.GetAsync("https://xecdapi.xe.com/v1/convert_from.json/?from=SGD&to=MYR,USD,EUR,GBP,AUD,JPY,CNY,IDR&amount=1");
                    if (task.Result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine("wrong credentials");
                    }
                    else
                    {
                        task.Result.EnsureSuccessStatusCode();
                        string file;
                        var response = task.Result.Content.ReadAsStreamAsync().Result;
                        using (var sr = new StreamReader(response))
                        {
                            file = sr.ReadToEnd();
                        }
                        ConvertFromResponse result = JsonConvert.DeserializeObject<ConvertFromResponse>(file);
                        using (var context = new DataAccess.GreatEastForex())
                        {
                            var getProduct = context.Products.Where(e => e.IsDeleted == "N").ToList();
                            var getRemittanceProduct = context.RemittanceProducts.Where(e => e.IsDeleted == "N").ToList();
                            var FromSGDtoRateList = result.to.ToList();
                            foreach (var product in getProduct)
                            {
                                var getProductCode = "";
                                if(product.CurrencyCode.Length >= 3)
                                 getProductCode = product.CurrencyCode.Substring(0, 3);
                                else
                                    getProductCode = product.CurrencyCode.Substring(0, 2);
                                var checkCurrency = FromSGDtoRateList.Where(e => e.quotecurrency == getProductCode).FirstOrDefault();
                                if (checkCurrency != null)
                                {
                                    var rate = (1 / checkCurrency.mid);
                                    rate = rate * product.Unit;
                                    product.AutomatedBuyRate = Convert.ToDecimal(rate);
                                    product.AutomatedSellRate = Convert.ToDecimal(rate);

                                    if (product.MaxAmount == null)
                                        product.MaxAmount = 10000;
                                    if (product.BuyRateAdjustment == null)
                                        product.BuyRateAdjustment = Convert.ToDecimal(0.02);
                                    if (product.SellRateAdjustment == null)
                                        product.SellRateAdjustment = Convert.ToDecimal(0.02);
                                }
                            }
                            foreach (var product in getRemittanceProduct)
                            {
                                var getProductCode = "";
                                if (product.CurrencyCode.Length >= 3)
                                    getProductCode = product.CurrencyCode.Substring(0, 3);
                                else
                                    getProductCode = product.CurrencyCode.Substring(0, 2);
                                var checkCurrency = FromSGDtoRateList.Where(e => e.quotecurrency == getProductCode).FirstOrDefault();
                                if (checkCurrency != null)
                                {
                                    var rate = (1 / checkCurrency.mid);
                                    product.AutomatedPayRate = Convert.ToDecimal(rate);
                                    product.AutomatedGetRate = Convert.ToDecimal(rate);
                                    //var getMaxAmount = product.MaxAmount;
                                    var getBuyRateAdjustment = product.BuyRateAdjustment;
                                    var getSellRateAdjustment = product.SellRateAdjustment;
                                    //if(getMaxAmount == null)
                                    //{
                                    //    product.MaxAmount = 100000;
                                    //}
                                    if (getBuyRateAdjustment == null)
                                    {
                                        product.BuyRateAdjustment = 0.00001M;
                                    }
                                    if (getSellRateAdjustment == null)
                                    {
                                        product.SellRateAdjustment = 0.00001M;
                                    }
                                }
                            }
                            context.SaveChanges();
                        }
                        Console.WriteLine(file);
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }

    }
}
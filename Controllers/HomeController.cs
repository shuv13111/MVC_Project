using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeTrackr.Data;
using ShoeTrackr.Models;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http;
using Newtonsoft.Json;
using static ShoeTrackr.Controllers.HomeController;
using ShoeTrackr.dtos;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ShoeTrackr.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        static string items_url = "https://shoes-collections.p.rapidapi.com/shoes/";
        static string api_key = "5a7f563dd0msh635408e9a35c696p12b244jsn633d591c0ec3";
        static string host = "shoes-collections.p.rapidapi.com";
        public DataContext dbContext;
        List<String> valid_errors;
        static Dictionary<int,int> hm;
        HttpClient httpClient;
        public HomeController(DataContext context, ILogger<HomeController> logger)
        {
            dbContext = context;
            _logger = logger;
            hm = new Dictionary<int, int>();
        }
        //public HomeController(ILogger<HomeController> logger)
        //{
           
        //}

        public async Task<IActionResult?> Index()
        {
            try
            {
                if (dbContext != null)
                {
                    if (dbContext.items.Count() == 0)
                    {

                        httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", api_key);
                        httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", host);
                        httpClient.DefaultRequestHeaders.Accept.Add(
                            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        //  string NATIONAL_PARK_API_PATH = BASE_URL + "/parks?limit=20";
                        string API_PATH = items_url;
                        string itemsData = "";

                        ItemModel items = null;

                        //httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);
                        httpClient.BaseAddress = new Uri(API_PATH);

                        //HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                        //                                        .GetAwaiter().GetResult();
                        HttpResponseMessage response = httpClient.GetAsync(API_PATH)
                                                                .GetAwaiter().GetResult();



                        if (response.IsSuccessStatusCode)
                        {
                            itemsData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        }
                        List<Item> itmlist;
                        if (!itemsData.Equals(""))
                        {
                            // JsonConvert is part of the NewtonSoft.Json Nuget package
                            //await dbContext.SaveChangesAsync();
                            itmlist = JsonConvert.DeserializeObject<List<Item>>(itemsData);
                            if (itmlist != null)
                            {
                                foreach (Item i in itmlist)
                                {
                                    //dbContext.items.Add(new ItemModel { Id = , Name = i.});
                                    dbContext.items.Add(new ItemModel
                                    {
                                        Name = i.name,
                                        Description = i.description,
                                        price = (int)i.price,
                                        rating = (double)i.rating.rate,
                                        quantity = 0,
                                    });
                                }
                                await dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error msg" + e);
            }
                    return View("~/Views/Home/Home.cshtml");
        }

        public IActionResult Dashboard()
        {
            List<ItemModel> model = dbContext.items.ToList();
            itemdto dto = new itemdto();
            dto.ItemsList = model;
            return View("~/Views/Home/Table.cshtml", dto);
        }

        [HttpGet]
        public IActionResult GetData()
        
        
        {
            // Your action logic here
            var rdata = dbContext.items.ToList();
            int cnt1 = 0, cnt2 = 0, cnt3=0,cnt4=0,cnt5=0;
            int total = 0;
            foreach (var item in rdata)
            { 
                if (item.rating>0.00 && item.rating<=1.00)
                    cnt1++;
                if (item.rating > 1.00 && item.rating <= 2.00)
                    cnt2++;
                if (item.rating > 2.00 && item.rating <= 3.00)
                    cnt3++;
                if (item.rating > 3.00 && item.rating <= 4.00)
                    cnt4++;
                if (item.rating > 4.00 && item.rating <= 5.00)
                    cnt5++;
                total++;
            }

            cnt1 = (cnt1*100)/ total;
            cnt2 = (cnt2 * 100) / total;
            cnt3 = (cnt3 * 100) / total;
            cnt4 = (cnt4 * 100) / total;
            cnt5 = (cnt5 * 100) / total;

            return Json(new {cnt1,cnt2,cnt3,cnt4,cnt5 });
        }


        public IActionResult GetPurchase()
        {
            List<PurchaseModel> model = dbContext.purchases.ToList();
            purchasedto dto = new purchasedto();
            dto.purchasesList = model;
            if (TempData["purchasesmsg"] != null)
            {
                ViewBag.msg = TempData["purchasesmsg"].ToString();
            }

            return View("~/Views/Home/purchase.cshtml",dto);
        }

        
        public IActionResult GetItems()
        {
            List<ItemModel> model = dbContext.items.ToList();
          //  List<ItemModel> ls = dbContext.items.ToList<ItemModel>();
            for (int i = 0; i < model.Count; i++)
            {
               
                hm[model[i].Id]=model[i].quantity;
            }
            itemdto dto = new itemdto();
            dto.ItemsList = model;
            
            return View("~/Views/Home/Table.cshtml", dto);
        }

        public Dictionary<int,int> GetListRescue()
        {
            List<ItemModel> model = dbContext.items.ToList();
            //  List<ItemModel> ls = dbContext.items.ToList<ItemModel>();
            for (int i = 0; i < model.Count; i++)
            {
               // hs.Add(model[i].Id);
                hm[model[i].Id]= model[i].quantity;
            }
            return hm;
        }
       
        public IActionResult GetSales()
        {
            List<SalesModel> model = dbContext.sales.ToList();
            List<SalesViewModel> vm = new List<SalesViewModel>();

            foreach (SalesModel sm in model)
            {

                vm.Add(new SalesViewModel()
                {
                    Id = sm.Id,
                    salesName  = sm.company,
                    quantity = (int)sm.netQuantity,
                    date = sm.date,
                    price = sm.netPrice
                    
                });
            
            }

           
            Salesdto dto = new Salesdto();
            dto.saleslist = vm;
            if (TempData["msg"] != null)
            {
                ViewBag.msg = TempData["msg"].ToString();
            }
            if (TempData["crate-err"] != null)
            {
                ViewBag.msg = TempData["crate-err"].ToString();

            }
            return View("~/Views/Home/sales.cshtml", dto);
        }
        [HttpGet]
        public IActionResult GetSalesDetailsById(int id)
        {
            List<SaleItemModel> model = dbContext.saleItemModels.Where(x => x.sale_id == id).ToList();
            List<ItemModel> dbmodel = dbContext.items.ToList();
            List<ItemModel> im = new List<ItemModel>();
            foreach (SaleItemModel sm in model)
            {
                foreach (ItemModel imod in dbmodel)
                {
                    if (sm.item_id == imod.Id)
                        im.Add(new ItemModel()
                        {

                            Name = imod.Name,
                            quantity = sm.sale_quantity,
                            price = imod.price

                        });
                
                
                }
            
            }
            return View("~/Views/Home/SalesDetail.cshtml", im);
        }
        public IActionResult AboutUs()
        {
            return View("~/Views/Home/AboutUs.cshtml");
        }

        [HttpPost]
        public IActionResult CreatePurchase(purchasedto dto)
        {
            try
            {
                


                       // List<PurchaseModel> purchases = dbContext.purchases.ToList();
                        if (true)
                        {
                            ItemModel itemtoUpdate = dbContext.items.Where(item => item.Id == dto.purchase.item_id).ToList()[0];
                            itemtoUpdate.quantity = itemtoUpdate.quantity + dto.purchase.quantity;
                            hm = GetListRescue();

                            PurchaseModel model = new PurchaseModel()
                            {

                                Supplier = dto.purchase.Supplier,
                                quantity = dto.purchase.quantity,
                                item_id= dto.purchase.item_id,
                                purchaseDate = dto.purchase.purchaseDate

                            };
                            hm[dto.purchase.item_id] = hm[dto.purchase.item_id] +dto.purchase.quantity;
                            dbContext.purchases.Add(model);

                            dbContext.SaveChanges();

                            return RedirectToAction("GetPurchase","Home");
                        }
                        else
                        {
                    return RedirectToAction("GetPurchase", "Home");
                }
                  

            
            }
               
            
            catch (Exception  /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
               // ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");

            }
           

            return View("~/Views/Home/Home.cshtml");
        }


        [HttpPost]
        public IActionResult UpdatePurchase(purchasedto dto)
        {
            try
            {
                if (true)
                {
                    if (true)
                    {


                        // List<PurchaseModel> purchases = dbContext.purchases.ToList();
                        if (true)
                        {
                            
                            PurchaseModel purchaseModel;
                            StringBuilder sb = new StringBuilder();
                            List<PurchaseModel> ls = dbContext.purchases.Where(x => x.Id == dto.purchase.Id).ToList();
                            if (ls.Count==0)
                            {
                                valid_errors = new List<string>();
                                sb.Append("Enter a valid Purchase Id to update");
                                TempData["purchasesmsg"] = sb.ToString();
                                return RedirectToAction("GetPurchase", "Home");
                            }
                            purchaseModel = ls[0];
                            ItemModel itemtoUpdate = dbContext.items.Where(item => item.Id == purchaseModel.item_id).ToList()[0];

                            itemtoUpdate.quantity = itemtoUpdate.quantity - purchaseModel.quantity +dto.purchase.quantity;
                            purchaseModel.purchaseDate = dto.purchase.purchaseDate;
                            purchaseModel.Supplier = dto.purchase.Supplier;
                            purchaseModel.quantity = dto.purchase.quantity;
                            dbContext.purchases.Update(purchaseModel);

                            dbContext.SaveChanges();

                            return RedirectToAction("GetPurchase", "Home");
                        }
                        else
                        {

                        }
                    }

                }


            }


            catch (Exception  /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                // ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");

            }


            return View("~/Views/Home/Home.cshtml");
        }


        public IActionResult DeletePurchase(int id)
        {
            try
            {
                if (true)
                {
                    if (true)
                    {


                        // List<PurchaseModel> purchases = dbContext.purchases.ToList();
                        if (true)
                        {
                            PurchaseModel purchaseModel = dbContext.purchases.Where(x => x.Id == id).ToList()[0];
                            ItemModel itemtoUpdate = dbContext.items.Where(item => item.Id == purchaseModel.item_id).ToList()[0];
                            itemtoUpdate.quantity = itemtoUpdate.quantity - purchaseModel.quantity;
                            dbContext.purchases.Remove(purchaseModel);
                            dbContext.SaveChanges();
                            return RedirectToAction("GetPurchase", "Home");
                        }
                        else
                        {

                        }
                    }

                }


            }


            catch (Exception  /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                // ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");

            }


            return View("~/Views/Home/Home.cshtml");
        }
        public IActionResult DeleteSales(int id)
        {
            try
            {
                if (true)
                {
                    if (true)
                    {


                        // List<PurchaseModel> purchases = dbContext.purchases.ToList();
                        if (true)
                        {
                            SalesModel purchaseModel = dbContext.sales.Where(x => x.Id == id).ToList()[0];
                            foreach (SaleItemModel item in purchaseModel.SaleItemModels)
                            { 
                             ItemModel itemModel = dbContext.items.Where(x=>x.Id == item.item_id).ToList()[0];
                             itemModel.quantity = itemModel.quantity-itemModel.quantity;
                             dbContext.items.Update(itemModel);
                            }
                            dbContext.sales.Remove(purchaseModel);
                            dbContext.SaveChanges();
                            return RedirectToAction("GetSales", "Home");
                        }
                        else
                        {

                        }
                    }

                }


            }


            catch (Exception  /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                // ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");

            }


            return View("~/Views/Home/Home.cshtml");
        }

        [HttpPost]
        public IActionResult CreateSale(Salesdto dto)
        {
            try
            {
                if (true)
                {
                    if (true)
                    {


                        // List<PurchaseModel> purchases = dbContext.purchases.ToList();
                        if (true)
                        {
                           
                            string[] idList = dto.SalesModel.itemsIdList.Split(',');
                            string[] quanList = dto.SalesModel.quantityList.Split(',');
                            string[] priceList = dto.SalesModel.quantityList.Split(',');
                            int zsum=0, vsum = 0;


                            hm = GetListRescue();
                            valid_errors = new List<string>();
                            StringBuilder sb = new StringBuilder();
                            //List<ItemModel> itmslist = dbContext.items.ToList<ItemModel>();
                            for(int i = 0; i < idList.Length; i++)
                            {
                                if (idList.Length != quanList.Length || idList.Length != priceList.Length || quanList.Length != priceList.Length)
                                {
                                    sb.Append("the number of inputs in the items must match the number of inputs in quantities");
                                    break;
                                }
                                if (!hm.ContainsKey(Convert.ToInt32(idList[i])))
                                {
                                    sb.Append("Item with Id " + idList[i] +"not found");
                                    break;
                                }
                                else
                                {
                                    if (hm[Convert.ToInt32(idList[i])] < Convert.ToInt32(quanList[i]))
                                    {
                                        sb.Append("select Item with Id " + idList[i] + "quantity available :"+ hm[Convert.ToInt32(idList[i])] );
                                        break;
                                    }
                                }
                            }
                            if (sb.Length > 0)
                            {
                                TempData["msg"] = sb.ToString();
                                return RedirectToAction("GetSales", "Home");
                            
                        }
                            List<SaleItemModel> saleItemModelList = new List<SaleItemModel>();
                            for (int i = 0; i < idList.Length; i++)
                            {
                                int v = Convert.ToInt32(idList[i]);
                                ItemModel itemtoUpdate_1 = dbContext.items.Where(item => item.Id == v).ToList()[0];
                                int z = Convert.ToInt32(quanList[i]);
                                vsum = z * itemtoUpdate_1.price;
                                zsum = z + zsum;
                               
                            }

                            SalesModel salesModel = new SalesModel()
                            {
                                company = dto.SalesModel.salesName,
                                netQuantity = zsum,
                                netPrice = vsum,
                                date = dto.SalesModel.date,

                            };

                            dbContext.sales.Add(salesModel);

                            dbContext.SaveChanges();

                            SalesModel sales = dbContext.sales.OrderBy(x => x.Id).LastOrDefault();

                            for (int i = 0; i < idList.Length; i++)
                            {
                                int v = Convert.ToInt32(idList[i]);
                                int z = Convert.ToInt32(quanList[i]);
                                ItemModel itemtoUpdate1 = dbContext.items.Where(item => item.Id == v).ToList()[0];
                                itemtoUpdate1.quantity = itemtoUpdate1.quantity - z;
                                
                                SaleItemModel saleItemModel = new SaleItemModel()
                                {

                                    sale_quantity = z,
                                    item_id = v,
                                    sale_id = sales!=null ? sales.Id : 1,
                                    sale_price = z *itemtoUpdate1.price,
                                };
                                saleItemModelList.Add(saleItemModel);
                            }
                            foreach(SaleItemModel modi in saleItemModelList)
                            dbContext.saleItemModels.Add(modi);

                            dbContext.SaveChanges();



                           

                            return RedirectToAction("GetSales", "Home");
                        }
                        else
                        {

                        }
                    }

                }


            }


            catch (Exception  /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                // ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");

            }


            return View("~/Views/Home/Home.cshtml");
        }

        [HttpPost]

        public IActionResult UpdateSaleItem(Salesdto dto)
        {
            StringBuilder sb = new StringBuilder();
            SaleItemModel sim = new SaleItemModel() { sale_id = dto.SalesModel.saleId, sale_price = dto.SalesModel.quantity, sale_quantity = Convert.ToInt32(dto.SalesModel.quantityList), item_id = Convert.ToInt32(dto.SalesModel.itemsIdList) };
            List<SaleItemModel> ls = dbContext.saleItemModels.Where(k => k.sale_id == sim.sale_id && k.item_id == sim.item_id).ToList<SaleItemModel>();

            if (Convert.ToInt32(dto.SalesModel.quantityList)<0)
            {
                sb.Append("Quantity cannot be negative");
                TempData["crate-err"] = sb.ToString();
                return RedirectToAction("GetSales", "Home");

            }

            if(dto.SalesModel.quantity<=0)
            {
                sb.Append("Price cannot be negative or zero");
                TempData["crate-err"] = sb.ToString();
                  return RedirectToAction("GetSales", "Home");

            }
            
            if (ls.Count==0)
            {
                sb.Append("There is no record with the given Sale ID and Item ID");
                TempData["crate-err"] = sb.ToString();
                return RedirectToAction("GetSales", "Home");

            }
           
            sim = ls[0];
            ItemModel itemtoUpdate = dbContext.items.Where(item => item.Id == sim.SalesItemId).ToList()[0];

            if (itemtoUpdate.quantity + sim.sale_quantity > Convert.ToInt32(dto.SalesModel.quantityList))
            {
                itemtoUpdate.quantity = itemtoUpdate.quantity + sim.sale_quantity - dto.SalesModel.quantity;
                sim.sale_price = dto.SalesModel.quantity;
                sim.sale_quantity = Convert.ToInt32(dto.SalesModel.quantityList);
                dbContext.saleItemModels.Update(sim);
                dbContext.items.Update(itemtoUpdate);
                dbContext.SaveChanges();

                return RedirectToAction("GetSales", "Home");
            }
            else
            {
                sb.Append("Please Choose a lower quantity");
            }
            return View("~/Views/Home/sales.cshtml");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class Item
        {

            public int id { get; set; }
            public string name { get; set; }
            public double price { get; set; }
            public string image { get; set; }
            public string description { get; set; }
            public int quantity { get; set; }

            public rating rating { get; set; }

        }

        public class rating
        {
            public double rate { get; set; }

            public int count { get; set; }
        }
    }
}
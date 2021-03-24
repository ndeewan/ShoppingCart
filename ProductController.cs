using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWLibraryShopping;
using ShoppingApp.Models;

namespace ShoppingApp.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            ProductDAL productDAL = new ProductDAL();
            List<Products> prodlist = productDAL.GetProducts();

            List<ProductModel> modelList = new List<ProductModel>();
            foreach (var item in prodlist)
            {
                ProductModel model = new ProductModel();
                model.ProductID = item.ProductID;
                model.ProductName = item.ProductName;
                model.UnitPrice = item.UnitPrice;
                modelList.Add(model);
            }

            return View(modelList);
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int Qty)
        {

            TempData["Qty"] = Qty;
            double amount = Convert.ToInt32(TempData["Qty"])* Convert.ToDouble(TempData["UnitPrice"]);
            ViewBag.Amount = amount;
            ProductModel prodData = new ProductModel();
            ProductDAL productDAL = new ProductDAL();
            Products prod = new Products();
            prod = productDAL.GetProduct(id);

            prodData.ProductID = prod.ProductID;
            prodData.ProductName = prod.ProductName;
            prodData.UnitPrice = prod.UnitPrice;
            prodData.Quantity = Convert.ToInt32(TempData["Qty"]);
            
            prod.ProductID = id;
            prod.ProductName = prodData.ProductName;
            prod.UnitPrice = prodData.UnitPrice;
            prod.Quantity = prodData.Quantity;
            productDAL.AddProduct(prod);
            return View(prodData);
            
        }


        //Adding product selected by the user to the Cart
       
        public ActionResult AddToCart(int id)
        {
            ProductModel prodData = new ProductModel();
            ProductDAL productDAL = new ProductDAL();
            Products prod = new Products();
            prod = productDAL.GetProduct(id);

            prodData.ProductID = prod.ProductID;
            prodData.ProductName = prod.ProductName;
            prodData.UnitPrice = prod.UnitPrice;
            TempData["UnitPrice"] = prodData.UnitPrice;
            //prodData.Quantity = ViewBag.Qty;
            //prod.ProductID = pid;
            //prod.ProductName = prodData.ProductName;
            //prod.UnitPrice = prodData.UnitPrice;
            //prod.Quantity = Qty;
            //productDAL.AddProduct(prod);
            Session["pdata"] = prodData;
            // return RedirectToAction("ShowCart");
            //prolist =(List<ProductModel>) Session["pdata"];
            return View(prodData);
            
        }



        //Showing ProductCart contents to the user

        List<ProductModel> prolist = new List<ProductModel>();

        public ActionResult ShowCart()
        {
            ProductDAL productDAL = new ProductDAL();
            List<Products> prodlist = productDAL.GetProductCart();

            List<ProductModel> modelList = new List<ProductModel>();

            double amount=0;

            foreach (var item in prodlist)
            {
                ProductModel model = new ProductModel();
                model.ProductID = item.ProductID;
                model.ProductName = item.ProductName;
                model.UnitPrice = item.UnitPrice;
                model.Quantity = item.Quantity;
                modelList.Add(model);

                amount += model.UnitPrice * model.Quantity;
            }
            ViewBag.Amount = amount;
             return View(modelList);
            //return View(prolist);
            
        }

        public ActionResult RemoveFromCart(int id)
        {
            ProductModel prodData = new ProductModel();
            ProductDAL productDAL = new ProductDAL();
            Products prod = new Products();
            prod = productDAL.GetProduct(id);

            prodData.ProductID = prod.ProductID;
            prodData.ProductName = prod.ProductName;
            prodData.UnitPrice = prod.UnitPrice;
            TempData["UnitPrice"] = prodData.UnitPrice;
            Session["pdata"] = prodData;

            TempData["pid"] = id;

            return View(prodData);

        }

        [HttpPost]
        public ActionResult RemoveFromCart()
        {
            ProductDAL proDAL = new ProductDAL();
            int id = Convert.ToInt32(TempData["pid"]);
            proDAL.DeleteProduct(id);
            return RedirectToAction("ShowCart");
        }

    }
}
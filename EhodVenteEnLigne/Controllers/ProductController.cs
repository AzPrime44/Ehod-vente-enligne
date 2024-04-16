using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EhodBoutiqueEnLigne.Models.Services;
using EhodBoutiqueEnLigne.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;
using System;

namespace EhodBoutiqueEnLigne.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILanguageService _languageService;
        private readonly IStringLocalizer<ProductService> _localizer;

        public ProductController(IProductService productService, ILanguageService languageService, IStringLocalizer<ProductService> localizer)
        {
            _productService = productService;
            _languageService = languageService;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductViewModel> products = _productService.GetAllProductsViewModel();
            return View(products);
        }

        [Authorize]
        public IActionResult Admin()
        {
            return View(_productService.GetAllProductsViewModel().OrderByDescending(p => p.Id));
        }

        [Authorize]
        public ViewResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(ProductViewModel product)
        {
            List<string> modelErrors = _productService.CheckProductModelErrors(product);
            Dictionary<string, string> errorDictionary = new Dictionary<string, string>();

            foreach (string error in modelErrors)
            {
                ModelState.AddModelError(error, _localizer[error]);
                errorDictionary.Add(error,error);
                
            }

            if (ModelState.IsValid)
            {
                _productService.SaveProduct(product);
                return RedirectToAction("Admin");
            }
            else
            {
                ViewData["errors"] = errorDictionary;
                return View(product);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Admin");
        }
    }
}
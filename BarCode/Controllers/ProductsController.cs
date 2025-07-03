using BarCode.Models;
using BarCode.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BarCode.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductServices _productServices;

        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productServices.GetAllAsync();
                return View(products);
            }
            catch (Exception ex)
            {
                // Log the error (use a logger in real app)
                return View("Error", ex.Message);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var product = await _productServices.GetByIdAsync(id.Value);
                if (product == null) return NotFound();
                return View(product);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                await _productServices.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return View(product);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var product = await _productServices.GetByIdAsync(id.Value);
                if (product == null) return NotFound();
                return View(product);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(product);

            try
            {
                await _productServices.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Update failed: {ex.Message}");
                return View(product);
            }
        }

  
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productServices.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        public IActionResult BarcodeSearch()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> BarcodeSearch(IFormFile barcode)
        {
            try
            {
                if (barcode == null || barcode.Length == 0)
                    return Json(new { error = "No file uploaded." });

                var id = _productServices.DecodeBarcode(barcode);
                if (string.IsNullOrEmpty(id))
                    return Json(new { error = "Could not decode barcode." });

                var result = await _productServices.GetByIdAsync(Convert.ToInt32(id));
                if (result == null)
                    return Json(new { error = "Product not found." });

                return Json(new { result });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Oct2026.WebApi.Models;
using Sep2026.Database.Models;

namespace Oct2026.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private AppDbContext _db;
        public SaleController()
        {
            _db = new AppDbContext();
        }

        [HttpGet]
        public IActionResult GetSales()
        {
            var sales = _db.Sales
                .OrderBy(s => s.SaleId)
                .Select(s => new SaleResponseModel
                {
                    SaleId = s.SaleId,
                    WalletId = s.WalletId,
                    ProductCode = s.ProductCode,
                    WalletName = s.WalletName,
                    SoldQty = s.SoldQty,
                    TotalAmount = s.TotalAmount,
                    SaleDate = s.SaleDate
                })
                .ToList();

            return Ok(sales);
        }

        [HttpGet("{id}")]
        public IActionResult GetSale(int id)
        {
            var sale = _db.Sales
                .Where(s => s.SaleId == id)
                .Select(s => new SaleResponseModel
                {
                    SaleId = s.SaleId,
                    WalletId = s.WalletId,
                    ProductCode = s.ProductCode,
                    WalletName = s.WalletName,
                    SoldQty = s.SoldQty,
                    TotalAmount = s.TotalAmount,
                    SaleDate = s.SaleDate
                })
                .FirstOrDefault();

            if (sale == null)
            {
                return NotFound();
            }

            return Ok(sale);
        }

        [HttpPost]
        public IActionResult CreateSale(SaleCreateRequestModel requestModel)
        {
            if (requestModel.SoldQty <= 0)
            {
                return BadRequest("Sold quantity must be greater than zero.");
            }

            var wallet = _db.Wallets
                .FirstOrDefault(w => w.Id == requestModel.WalletId && !w.IsDeleted);

            if (wallet == null)
            {
                return NotFound($"Wallet with id {requestModel.WalletId} not found.");
            }

            if (wallet.StockQuantity <= 0 || wallet.StockQuantity < requestModel.SoldQty)
            {
                return BadRequest(
                    $"Out of stock or insufficient stock. Available stock: {wallet.StockQuantity}");
            }

            decimal totalAmount = wallet.Price * requestModel.SoldQty;

            wallet.StockQuantity -= requestModel.SoldQty;

            Sale sale = new Sale
            {
                WalletId = wallet.Id,
                ProductCode = wallet.ProductCode,
                WalletName = wallet.Name,
                SoldQty = requestModel.SoldQty,
                TotalAmount = totalAmount,
                SaleDate = DateTime.Now
            };

            _db.Sales.Add(sale);
            int result = _db.SaveChanges();

            if (result <= 0)
            {
                return Ok("Sale Failed");
            }

            return Ok(new
            {
                Message = "Sale Successful",
                Sale = new SaleResponseModel
                {
                    SaleId = sale.SaleId,
                    WalletId = sale.WalletId,
                    ProductCode = sale.ProductCode,
                    WalletName = sale.WalletName,
                    SoldQty = sale.SoldQty,
                    TotalAmount = sale.TotalAmount,
                    SaleDate = sale.SaleDate
                }
            });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Oct2026.WebApi.Models;
using Sep2026.Database.Models;
using Sep2026.WebApi.Models;

namespace Oct2026.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private AppDbContext _db;
        public WalletController()
        {
            _db = new AppDbContext();
        }

        [HttpGet]
        public IActionResult GetWallets()
        {
            var wallets = _db.Wallets.ToList();
            return Ok(wallets);
        }

        [HttpGet("Edit/{id}")]
        public IActionResult GetWallet(int id)
        {
            var wallet = _db.Wallets.FirstOrDefault(w => w.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }
            return Ok(wallet);
        }
        [HttpPost]
        public IActionResult CreateWallet(WalletCreateRequestModel requestModel)
        {
            int maxId = _db.Wallets.Any() ? _db.Wallets.Max(w => w.Id) : 0;
            int newId = maxId + 1;
            string productCode = $"WAL-{newId:D4}";

            _db.Wallets.Add(new Wallet
            {
                ProductCode = productCode,
                Brand = requestModel.Brand,
                Color = requestModel.Color,
                Gender = requestModel.Gender,
                Name = requestModel.Name,
                Price = requestModel.Price,
                StockQuantity = requestModel.StockQuantity
            });
            int result = _db.SaveChanges();
            string message = result > 0 ? "Saving Successful" : "Saving Failed";
            return Ok(message);
        }
        [HttpPut("{id}")]
        public IActionResult UpSertWallet(int id, WalletUpsertRequestModel requestModel)
        {
            var wallet = _db.Wallets.FirstOrDefault(w => w.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            wallet.Brand = requestModel.Brand.Trim();
            wallet.Color = requestModel.Color.Trim();
            wallet.Gender = requestModel.Gender;
            wallet.Name = requestModel.Name.Trim();
            wallet.Price = requestModel.Price;
            wallet.StockQuantity = requestModel.StockQuantity;

            int result = _db.SaveChanges();
            string message = result > 0 ? "Update Successful" : "Update Failed";
            return Ok(message);
        }
        [HttpPatch("{id}")]
        public IActionResult UpdateWallet(int id, WalletUpdateRequestModel requestModel)
        {
            var wallet = _db.Wallets.FirstOrDefault(w => w.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(requestModel.Brand))
            {
                wallet.Brand = requestModel.Brand.Trim();
            }

            if (!string.IsNullOrEmpty(requestModel.Color))
            {
                wallet.Color = requestModel.Color.Trim();
            }

            if (requestModel.Gender != null)
            {
                wallet.Gender = requestModel.Gender.Value;
            }

            if (!string.IsNullOrEmpty(requestModel.Name))
            {
                wallet.Name = requestModel.Name.Trim();
            }

            if (requestModel.Price != null)
            {
                wallet.Price = requestModel.Price.Value;
            }

            if (requestModel.StockQuantity != null)
            {
                wallet.StockQuantity = requestModel.StockQuantity.Value;
            }

            int result = _db.SaveChanges();
            string message = result > 0 ? "Update Successful" : "Update Failed";
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWallet(int id)
        {
            var wallet = _db.Wallets.FirstOrDefault(w => w.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            _db.Wallets.Remove(wallet);

            int result = _db.SaveChanges();
            string message = result > 0 ? "Delete Successful" : "Delete Failed";

            return Ok(message);
        }
    }
}

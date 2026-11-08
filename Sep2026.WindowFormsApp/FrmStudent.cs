using Sep2026.Database.Models;

namespace Oct2026.WindowForm
{
    public partial class FrmStudent : Form
    {
        public FrmStudent()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                AppDbContext dbContext = new AppDbContext();
                string genderText = txtGender.Text.Trim();

                byte gender = genderText.ToLower() switch
                {
                    "male" => 1,
                    "female" => 2,
                    _ => 3
                };
                int maxId = dbContext.Wallets.Any() ? dbContext.Wallets.Max(w => w.Id) : 0;
                int newId = maxId + 1;
                string productCode = $"WAL-{newId:D4}";

                dbContext.Wallets.Add(new Wallet
                {
                    ProductCode = productCode,
                    Name = txtName.Text.Trim(),
                    Brand = txtBrand.Text.Trim(),
                    Gender = gender,
                    Color = txtColor.Text.Trim(),
                    Price = Convert.ToDecimal(txtPrice.Text.Trim()),
                    StockQuantity = Convert.ToInt32(txtStockQty.Text.Trim())
                });
                int result = dbContext.SaveChanges();
                string message = result > 0 ? "Saving Successful" : "Saving Failed";
                MessageBoxIcon icon = result > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Error;
                MessageBox.Show(message, "Caption", MessageBoxButtons.OK, icon);
                ClearBtn();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearBtn();
        }

        private void ClearBtn()
        {
            txtName.Clear();
            txtBrand.Clear();
            txtGender.Clear();
            txtColor.Clear();
            txtPrice.Clear();
            txtStockQty.Clear();

            txtName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearBtn();
        }
    }
}

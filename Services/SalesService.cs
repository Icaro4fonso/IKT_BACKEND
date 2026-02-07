using ExcelDataReader;
using IKT_BACKEND.Domain.Repositories;
using IKT_BACKEND.Domain.Services;
using IKT_BACKEND.Dtos;
using IKT_BACKEND.Persistence.Models;
using IKT_BACKEND.Utils;
using IKT_BACKEND.Validators;
using System.Globalization;

namespace IKT_BACKEND.Services
{
    public class SalesService : ISalesService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly ISalesRespository salesRespository;

        private const string DATE_TIME_CELL = "datetime";
        private const string PAYMENT_CELL = "cash_type";
        private const string MONEY_CELL = "money";
        private const string COFFEE_NAME_CELL = "coffee_name";
        private const string CARD_CELL = "card";
        private const string MONTH_CELL = "Monthsort";

        public SalesService(IUnitOfWork unitOfWork, IProductRepository productRepository, ISalesRespository salesRespository)
        {
            this.unitOfWork = unitOfWork;
            this.productRepository = productRepository;
            this.salesRespository = salesRespository;    
        }   

        public BaseResponse<bool> GetSuccess()
        {
            return new OkResponse<bool>(true);
        }

        public async Task<BaseResponse<bool>> SaveRecords(IFormFile file) 
        {
            HashSet<string> productNames = new();
            List<ExcelSaleDto> salesDto = new();

            // Switch to brasil 
            var cultureBrasil = new CultureInfo("pt-BR");

            // Validation 
            var errors = new List<string>();
            var validator = new ExcelSaleValidator();

            // Open stream for file
            using Stream stream = file.OpenReadStream();
            using var reader = ExcelReaderFactory.CreateReader(stream);

            var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            });
              
            var table = dataSet.Tables[0];

            for (int i = 1; i < table.Rows.Count; i++)
            {

                // Current row
                var row = table.Rows[i];

                // Retrieve data
                DateTime dateTime = DateTime.Parse(row[DATE_TIME_CELL].ToString());

                decimal price = decimal.Parse(row[MONEY_CELL].ToString(),
                                              NumberStyles.Currency | NumberStyles.AllowDecimalPoint,
                                              cultureBrasil);

                string coffeeName = row[COFFEE_NAME_CELL].ToString();

                int saleMonth = int.Parse(row[MONTH_CELL].ToString());

                string cardNumber = row[CARD_CELL].ToString();

                string paymentRaw = row[PAYMENT_CELL].ToString();
                PaymentType paymentType = paymentRaw == "card" ? PaymentType.Card : PaymentType.Cash;

                ExcelSaleDto excelDto = new()
                {
                    DateTime = dateTime,
                    Price = price,
                    ProductName = coffeeName,
                    PaymentType = paymentType,
                    Month = saleMonth,
                    CardNumber = cardNumber
                };

                var validation = validator.Validate(excelDto);

                // Valid row and it will be handled on database
                if (validation.IsValid)
                {
                    salesDto.Add(excelDto);
                    productNames.Add(excelDto.ProductName);

                }// Invalid row
                else
                {
                    // ToDo: User needs to know the errors on excel
                    foreach (var error in validation.Errors)
                    {
                        errors.Add($"Line {i}: {error.ErrorMessage}");
                    }
                }
            }

            // Products in database
            Dictionary<string,long> databaseProducts = await productRepository.FindByRange(productNames);

            //  Find new products to add in database
            var newProducts = productNames
                .Where(n => !databaseProducts.ContainsKey(n))
                .Select(n => new Product { Name = n })
                .ToList();

            // Save on database new products
            if (newProducts.Count > 0)
            {
                await productRepository.AddByRange(newProducts);
                await unitOfWork.SaveChangesAsync();

                foreach (var p in newProducts)
                { 
                    databaseProducts[p.Name] = p.Id;
                }
            }

            var sales = salesDto.Select(dto =>
                new Sale()
                {
                    DateTime = DateTime.SpecifyKind(dto.DateTime, DateTimeKind.Utc),
                    PaymentType = dto.PaymentType,
                    Price = dto.Price,
                    ProductId = databaseProducts[dto.ProductName],
                }).ToList();

            await salesRespository.BulkInsertAsync(sales);

            return new OkResponse<bool>(true);
        }
    }
}

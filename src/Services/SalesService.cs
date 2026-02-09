using ExcelDataReader;
using IKT_BACKEND.Domain.Repositories;
using IKT_BACKEND.Domain.Services;
using IKT_BACKEND.Dtos;
using IKT_BACKEND.Persistence.Models;
using IKT_BACKEND.Utils;
using IKT_BACKEND.Validators;
using System.Data;
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

        public async Task<BaseResponse<List<string>>> SaveRecords(IFormFile file)
        {
            HashSet<string> productNames = new();
            List<ExcelSaleDto> salesDto = new();

            // Validation 
            var errors = new List<string>();
            var validator = new ExcelSaleValidator();
            try
            {
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

                if (dataSet.Tables.Count == 0)
                {
                    return new ErrorResponse<List<string>>("This file does not contains any record");
                }

                var table = dataSet.Tables[0];


                for (int i = 0; i < table.Rows.Count; i++)
                {

                    // Current row
                    var row = table.Rows[i];

                    // Validate Row
                    var rowValidation =  ValidateRowValues(row);

                    if (rowValidation.Success)
                    {
                        ExcelSaleDto excelDto = rowValidation.GetValue(); 

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
                                errors.Add($"Line {i + 1}: {error.ErrorMessage}");
                            } 
                        }
                    }
                    else
                    {
                        errors.Add($"Line {i + 1}: {rowValidation.GetErrorMessage()}");
                    }
                }

                // Products in database
                Dictionary<string, long> databaseProducts = await productRepository.FindByRange(productNames);

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

                    // Add new products ids
                    foreach (var p in newProducts)
                    {
                        databaseProducts[p.Name] = p.Id;
                    }
                }

                // Map to Sale model
                var sales = salesDto.Select(dto =>
                    new Sale()
                    {
                        DateTime = DateTime.SpecifyKind(dto.DateTime, DateTimeKind.Utc),
                        PaymentType = dto.PaymentType,
                        Price = dto.Price,
                        Month = dto.Month,
                        ProductId = databaseProducts[dto.ProductName],
                    }).ToList();

                await salesRespository.BulkInsertAsync(sales);

                return new OkResponse<List<string>>(errors);
            }
            catch (Exception ex)
            {
                return new ErrorResponse<List<string>>($"Error while saving saving your file: {ex.Message}. Please contact our support team. ");
            }
        }

        public async Task<BaseResponse<List<SaleResumeDto>>> MostProfitMonths()
        {
            var sales = await salesRespository.MostProfitMonthsAsync();

            return new OkResponse<List<SaleResumeDto>>(sales);
        }

        private BaseResponse<ExcelSaleDto> ValidateRowValues(DataRow row)
        {
            // DateTime
            if (!DateTime.TryParse(row[DATE_TIME_CELL]?.ToString(), out var dateTime))
            {
                return new ErrorResponse<ExcelSaleDto>("Invalid datetime");
            }

            // Switch to brasil money currency
            var cultureBrasil = new CultureInfo("pt-BR");
            // Price
            if (!decimal.TryParse(
                    row[MONEY_CELL]?.ToString(),
                    NumberStyles.Currency | NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out var price))
            {
                return new ErrorResponse<ExcelSaleDto>("Invalid price");
            }

            // Coffe Name
            string coffeeName = row[COFFEE_NAME_CELL]?.ToString();
            if (string.IsNullOrWhiteSpace(coffeeName))
            {
                return new ErrorResponse<ExcelSaleDto>("Invalid coffeeName");
            }

            // Month Sale
            if (!int.TryParse(row[MONTH_CELL]?.ToString(), out var saleMonth))
            {
                return new ErrorResponse<ExcelSaleDto>("Invalid month");
            }

            string cardNumber = row[CARD_CELL]?.ToString();
            string paymentRaw = row[PAYMENT_CELL]?.ToString();

            // Payment type
            if(Enum.TryParse(paymentRaw, out PaymentType paymentType))
            {
                return new ErrorResponse<ExcelSaleDto>("Invalid payment type");
            }

            ExcelSaleDto excelDto = new()
            {
                DateTime = dateTime,
                Price = price,
                ProductName = coffeeName,
                PaymentType = paymentType,
                Month = saleMonth,
                CardNumber = cardNumber
            };

            return new OkResponse<ExcelSaleDto>(excelDto);
        }
    }
}

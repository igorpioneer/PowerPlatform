using CsvHelper;
using CsvHelper.Configuration;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Repository
{
    public class CreateCSVFile
    {
        public static byte[] GetCSVFile(int CampaignId)
        {
            var context = new PowerPlatformTaskContext();
            var data = context.Customers
                .Join(context.Sales,
                      c => c.Id,
                      s => s.CustomerId,
                      (c, s) => new { Customer = c, Sales = s })
                .Join(context.Products,
                      co => co.Sales.ProductId,
                      p => p.Id,
                      (co, p) => new { co.Customer.Name, p.ProductName, p.Price, co.Sales.CreatedAt })
                .ToList();
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture));
            csvWriter.WriteRecords(data);
            streamWriter.Close();
            return memoryStream.ToArray();
        }
    }
}

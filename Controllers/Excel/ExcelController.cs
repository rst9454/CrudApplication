
using CrudApplication.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CrudApplication.Controllers.Excel
{
    public class ExcelController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationContext context;

        public ExcelController(IConfiguration configuration, ApplicationContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult GetCustomerList()
        {
            var data = context.Customers.ToList();
            return new JsonResult(data);
        }

        public IActionResult ImportExcelFile()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ImportExcelFile(IFormFile formFile)
        {

            try
            {
                var mainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadExcelFile");
                if (!Directory.Exists(mainPath))
                {
                    Directory.CreateDirectory(mainPath);
                }
                var filePath = Path.Combine(mainPath, formFile.FileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }
                var fileName = formFile.FileName;
                string extension = Path.GetExtension(fileName);
                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls":
                        conString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filePath + ";Extended Properties='Excel 8.0; HDR=YES'";
                        break;
                    case ".xlsx":
                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                        break;
                }
                DataTable dt = new DataTable();
                conString = String.Format(conString, filePath);
                using (OleDbConnection conExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = conExcel;
                            conExcel.Open();
                            DataTable dtExcelSchema = conExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            cmdExcel.CommandText = "SELECT * FROM [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            conExcel.Close();
                        }
                    }
                }

                conString = configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.DestinationTableName = "Customers";
                        sqlBulkCopy.ColumnMappings.Add("CustomerCode", "CustomerCode");
                        sqlBulkCopy.ColumnMappings.Add("FirstName", "FirstName");
                        sqlBulkCopy.ColumnMappings.Add("LastName", "LastName");
                        sqlBulkCopy.ColumnMappings.Add("Gender", "Gender");
                        sqlBulkCopy.ColumnMappings.Add("Country", "Country");
                        sqlBulkCopy.ColumnMappings.Add("Age", "Age");
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
                TempData["Success"] = "File Imported Successfully, Data Saved into Database.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View();
        }

        public IActionResult ExportExcel()
        {
            try
            {
                var data = context.Customers.ToList();
                if (data != null & data.Count > 0)
                {
                    //using (XLWorkbook wb = new XLWorkbook())
                    //{
                    //    wb.Worksheets.Add(ToConvertDataTable(data.ToList()));
                    //    using (MemoryStream strem = new MemoryStream())
                    //    {
                    //        wb.SaveAs(strem);
                    //        string fileName = $"Customer_{DateTime.Now.ToString("dd/MM/yyyy")}.xlsx";
                    //        return File(strem.ToArray(), "application/vnd.openxmlformats-officedocuments.spreadsheetml.sheet", fileName);
                    //    }
                    //}
                }
                TempData["Error"] = "Data not found!";
            }
            catch (Exception)
            {

            }

            return RedirectToAction("index");
        }

        public DataTable ToConvertDataTable<T>(List<T> items)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            PropertyInfo[] propInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in propInfo)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[propInfo.Length];
                for (int i = 0; i < propInfo.Length; i++)
                {
                    values[i] = propInfo[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }

    }
}

using CDR.Services.Spreadsheet.Model.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Spreadsheet.Service.Helper
{
    public class SheetHelper
    {
        public async Task<ExcelPackage> GetSheet(CloudBlobClient blobClient, string temporaryContainer, string FilePath)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(temporaryContainer);
            CloudBlockBlob Blob = container.GetBlockBlobReference(FilePath);
            var buvStream = new MemoryStream();
            await Blob.DownloadToStreamAsync(buvStream);
            ExcelPackage Package = new ExcelPackage(buvStream);
            return Package;
        }

        public ExcelPackage MergeSheets(ExcelPackage Buv, ExcelPackage Vb)
        {
            foreach (var sheet in Vb.Workbook.Worksheets)
            {
                string workSheetName = sheet.Name;
                //check name of worksheet, in case that worksheet with same name already exist exception will be thrown by EPPlus
                foreach (var masterSheet in Buv.Workbook.Worksheets)
                {
                    if (sheet.Name == masterSheet.Name)
                    {
                        workSheetName = string.Format("{0}_{1}", workSheetName, DateTime.Now.ToString("yyyyMMddhhssmmm"));
                    }
                }
                //add new sheet
                Buv.Workbook.Worksheets.Add(workSheetName, sheet);
            }
            Buv.Save();
            return Buv;
        }

        public ExcelPackage ClearCommentsAndStyle(ExcelPackage masterPackage)
        {
            using (masterPackage)
            {
                if (masterPackage.Workbook.Worksheets.Count > 0)
                {
                    var sheets = masterPackage.Workbook.Worksheets;
                    foreach (var sheet in sheets)
                    {
                        var workSheet = masterPackage.Workbook.Worksheets[sheet.Name];
                        var comments = workSheet.Comments.Cast<ExcelComment>().Where(c => c.Text.ToLower().StartsWith("V:")).ToList();
                        if (comments.Count > 0)
                        {
                            foreach (ExcelComment cell in comments)
                            {
                                workSheet.Comments.Remove(workSheet.Cells[cell.Address].Comment);
                                workSheet.Cells[cell.Address].Style.Fill.PatternType = ExcelFillStyle.None;
                            }
                        }
                    }
                }
            }

            return masterPackage;
        }

    }
}

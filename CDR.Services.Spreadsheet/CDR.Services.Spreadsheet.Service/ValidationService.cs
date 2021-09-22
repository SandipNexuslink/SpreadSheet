using CDR.Azure.Storage.Blob.Client;
using CDR.Services.Spreadsheet.Model;
using CDR.Services.Spreadsheet.Model.Interfaces;
using CDR.Services.Spreadsheet.Service.Helper;
using CDR.Services.Spreadsheet.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CDR.Services.Spreadsheet.Service
{
    public class ValidationService : IValidationService
    {
        private readonly IConfiguration _config;
        private readonly IAzureBlobStorageClient _blobClient;
        private string cellsWithErrors;
        private string _container;
        private SheetHelper _sheetHelper;
        private string ValidatorBookName { get { return _config["ExcelProperties:ValidatorBookName"]; } }
        //private static readonly CultureInfo culture = new CultureInfo("en-GB");


        public ValidationService(IConfiguration config, IAzureBlobStorageClient azureBlobStorageClient)
        {
            _config = config;
            _blobClient = azureBlobStorageClient;
            _container = _config["CDR:CDI:TemporaryFiles:Container"];
            _sheetHelper = new SheetHelper(_config, _blobClient);
        }

        public async Task<IValidationResponse> Validation(IValidationRequest req)
        {
            if (string.IsNullOrEmpty(req.BookUnderValidation))
            {
                return new ValidationResponse();
            }

            var response = new ValidationResponse
            {
                id = Guid.NewGuid().ToString(),
                Created = DateTimeOffset.Now,
                BookUnderValidation = req.BookUnderValidation,
                ValidatorBook = req.ValidatorBook
            };

            /*Setting up blob storage starts*/
            var buvFilePath = req.BookUnderValidation.Remove(0, _container.Length + 1);
            var vbFilePath = req.ValidatorBook.Remove(0, _container.Length + 1);
            /*Setting up blob storage end*/

            var buvPackage = await _sheetHelper.GetSheet(buvFilePath);
            var vdPackage = await _sheetHelper.GetSheet(vbFilePath);

            /* Merge sheet start */
            foreach (var sheet in vdPackage.Workbook.Worksheets)
            {
                string workSheetName = ValidatorBookName;
                buvPackage.Workbook.Worksheets.Add(workSheetName, sheet);
            }
            /* Merge sheet end */

            /* Clear comments and style start*/
            if (buvPackage.Workbook.Worksheets.Count > 0)
            {
                var sheets = buvPackage.Workbook.Worksheets;
                foreach (var sheet in sheets)
                {
                    var workSheet = buvPackage.Workbook.Worksheets[sheet.Name];
                    var comments = workSheet.Comments.Cast<ExcelComment>().Where(c => c.Text.ToLower().StartsWith(_config["ExcelProperties:CommentPrefix"])).ToList();
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
            buvPackage.Save();
            /* Clear comments and style end */

            /* Validate sheet start */
            var validatorSheet = buvPackage.Workbook.Worksheets[ValidatorBookName];
            var cells = validatorSheet.Cells[validatorSheet.Dimension.ToString()];
            var InValidCells = cells.Where(c => c.Value.ToString().ToLower() == "false").Select(c => c.Address).ToList();
            response.IsValid = !InValidCells.Any();
            cellsWithErrors = string.Join(",", InValidCells);
            cellsWithErrors = Regex.Replace(cellsWithErrors, "[^0-9 ,_]", "");
            /* Validate sheet end */

            /* Collecting validation errors start*/
            var errorCells = cellsWithErrors.Split(',');
            var lastCell = string.Empty;
            ValidationResult validationResult = new ValidationResult("");
            for (int i = 0; i < errorCells.Length; i++)
            {
                if (!req.Options.WriteErrorsToBook)
                {
                    var cellAddresses = new List<string>();
                    var cellAddress = validatorSheet.Cells["A" + errorCells[i]].Formula;
                    cellAddress = cellAddress.Substring(cellAddress.LastIndexOf('!') + 1);
                    cellAddresses.Add(cellAddress);
                    if (lastCell != string.Empty && lastCell == cellAddress)
                        validationResult.ErrorMessage += ", " + validatorSheet.Cells["C" + errorCells[i]].Value.ToString();
                    else
                    {
                        validationResult = new ValidationResult(validatorSheet.Cells["C" + errorCells[i]].Value.ToString(), cellAddresses);
                        response.Results.Add(validationResult);
                    }
                    lastCell = cellAddress;
                }
                else
                {
                    var sheetName = validatorSheet.Cells["A" + errorCells[i]].Formula;
                    var cellAddress = validatorSheet.Cells["A" + errorCells[i]].Formula;
                    int startIndex = sheetName.IndexOf(']');
                    int endIndex = sheetName.IndexOf('!');
                    sheetName = sheetName.Substring(startIndex + 1, endIndex - startIndex - 1);
                    sheetName = sheetName.Substring(0, sheetName.Length - 1);
                    cellAddress = cellAddress.Substring(cellAddress.LastIndexOf('!') + 1);
                    var sheetToUpdate = buvPackage.Workbook.Worksheets[sheetName];
                    sheetToUpdate.Cells[cellAddress].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheetToUpdate.Cells[cellAddress].Style.Fill.BackgroundColor.SetColor(Color.Red);
                    ExcelRange Rng = sheetToUpdate.Cells[cellAddress];
                    ExcelComment cmd = Rng.AddComment(validatorSheet.Cells["C" + errorCells[i]].Value.ToString(), _config["ExcelProperties:CommentAuther"]);
                    buvPackage.Save();
                }
            }

            /* Remove validator sheet start*/
            var validatorBook = buvPackage.Workbook.Worksheets[ValidatorBookName];
            if (validatorBook != null)
                buvPackage.Workbook.Worksheets.Delete(validatorBook);
            buvPackage.Save();
            /* Remove validator sheet end*/

            if (req.Options.WriteErrorsToBook)
            {
                await _sheetHelper.UpsertSheet(buvFilePath, buvPackage);
            }
            /* Collecting validation errors end*/


            response.Results.AsEnumerable();

            return response;
        }

    }
}

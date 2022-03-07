using Microsoft.Office.Interop.Excel;
using Sandelio_app_1.classes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Sandelio_app_1.controllers
{
    internal class ExcelController
    {
        public static void CreateFile(List<Pallet> pallets, string client)
        {
            Application excel;
            Workbook workBook;
            Worksheet palletListSheet;
            Worksheet drawingsWorkSheet;

            try
            {
                //Start Excel and get Application object.
                excel = new Application
                {
                    UserControl = false,
                    Visible = false
                };

                //Get a new workbook.
                workBook = excel.Workbooks.Add(Missing.Value);

                // Pallet List sheet formatting function
                // ** This has to be the first function, since it uses the default sheet
                palletListSheet = CreatePalletListSheet(pallets, client, workBook);

                // Drawings sheet formatting function
                drawingsWorkSheet = CreateDrawingsSheet(pallets, client, workBook);

                // Individual pallet sheet formatting function
                CreatePalletSheets(pallets, client, workBook);

                // Give the user control of Microsoft Excel's lifetime.
                excel.Visible = true;
                excel.UserControl = true;
            }
            catch (Exception theException)
            {
                string errorMessage;
                errorMessage = "Error: \n";
                errorMessage = string.Concat(errorMessage, theException.Message);
                errorMessage = string.Concat(errorMessage, "\n");
                errorMessage = string.Concat(errorMessage, theException.Source);

                _ = MessageBox.Show(errorMessage, "Error");
            }
        }

        private static void CreatePalletSheets(List<Pallet> pallets, string client, Workbook workBook)
        {
            for (int i = 1; i < pallets.Count + 1; i++)
            {
                Worksheet currentSheet = (Worksheet)workBook.Worksheets.Add(After: workBook.Sheets[workBook.Sheets.Count]);
                currentSheet.Name = $"Pallet {i}";

                Range range = currentSheet.Range[currentSheet.Cells[2, 1], currentSheet.Cells[4, 4]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;
                currentSheet.Cells[2, 1] = "Client:";
                currentSheet.Cells[2, 2] = client;
                currentSheet.Range[currentSheet.Cells[2, 2], currentSheet.Cells[2, 4]].Merge();
                currentSheet.Cells[3, 1] = "Order nr.:";
                currentSheet.Range[currentSheet.Cells[3, 1], currentSheet.Cells[4, 1]].Merge();
                currentSheet.Cells[3, 3] = pallets[i - 1].OrderNumber;

                currentSheet.Range[currentSheet.Cells[3, 2], currentSheet.Cells[4, 4]].Merge();
                currentSheet.Cells[8, 1] = "LDM:";
                // Čia blogai sudaugina, turi būt metrais
                currentSheet.Cells[8, 2] = $"{pallets[i - 1].GetLDM()}";
                currentSheet.Cells[8, 4] = "Height:";
                currentSheet.Cells[8, 5] = pallets[i - 1].GetTotalHeight();
                currentSheet.Cells[8, 7] = "Width:";
                currentSheet.Cells[8, 8] = pallets[i - 1].Width;
                currentSheet.Cells[8, 10] = "Length:";
                currentSheet.Cells[8, 11] = pallets[i - 1].Length;
                currentSheet.Cells[8, 13] = "Weight:";
                currentSheet.Cells[8, 14] = pallets[i - 1].GetPalletWeight() + "kg";
                range = currentSheet.Range[currentSheet.Cells[8, 1], currentSheet.Cells[8, 2]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                range = currentSheet.Range[currentSheet.Cells[8, 4], currentSheet.Cells[8, 5]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                range = currentSheet.Range[currentSheet.Cells[8, 7], currentSheet.Cells[8, 8]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                range = currentSheet.Range[currentSheet.Cells[8, 10], currentSheet.Cells[8, 11]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                range = currentSheet.Range[currentSheet.Cells[8, 13], currentSheet.Cells[8, 14]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                currentSheet.Cells[2, 7] = "Delivery address:";
                currentSheet.Range[currentSheet.Cells[2, 7], currentSheet.Cells[2, 8]].Merge();
                currentSheet.Cells[3, 7] = "Post code:";
                currentSheet.Range[currentSheet.Cells[3, 7], currentSheet.Cells[3, 8]].Merge();
                currentSheet.Cells[4, 7] = "City:";
                currentSheet.Range[currentSheet.Cells[4, 7], currentSheet.Cells[4, 8]].Merge();
                currentSheet.Cells[5, 7] = "Country:";
                currentSheet.Range[currentSheet.Cells[5, 7], currentSheet.Cells[5, 8]].Merge();
                currentSheet.Cells[6, 7] = "Loading date:";
                currentSheet.Range[currentSheet.Cells[6, 7], currentSheet.Cells[6, 8]].Merge();

                range = currentSheet.Range[currentSheet.Cells[2, 7], currentSheet.Cells[6, 10]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;
                currentSheet.Cells[2, 9] = pallets[i - 1].Adress;
                currentSheet.Range[currentSheet.Cells[2, 9], currentSheet.Cells[2, 10]].Merge();
                currentSheet.Cells[3, 9] = pallets[i - 1].PostCode;
                currentSheet.Range[currentSheet.Cells[3, 9], currentSheet.Cells[3, 10]].Merge();
                currentSheet.Cells[4, 9] = pallets[i - 1].City;
                currentSheet.Range[currentSheet.Cells[4, 9], currentSheet.Cells[4, 10]].Merge();
                currentSheet.Cells[5, 9] = pallets[i - 1].Country;
                currentSheet.Range[currentSheet.Cells[5, 9], currentSheet.Cells[5, 10]].Merge();
                currentSheet.Cells[6, 9] = "";
                currentSheet.Range[currentSheet.Cells[6, 9], currentSheet.Cells[6, 10]].Merge();

                currentSheet.Cells[2, 11] = "Pallet nr.:";
                currentSheet.Range[currentSheet.Cells[2, 12], currentSheet.Cells[2, 12]].NumberFormat = "@";
                currentSheet.Cells[2, 12] = $"{i}/{pallets.Count}";
                range = currentSheet.Range[currentSheet.Cells[2, 11], currentSheet.Cells[2, 12]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                // this is the side of the 'pallet' that displays the length
                currentSheet.Cells[10, 2] = $"{pallets[i - 1].Length}";
                currentSheet.Range[currentSheet.Cells[10, 2], currentSheet.Cells[11, 11]].Merge();
                range = currentSheet.Range[currentSheet.Cells[10, 2], currentSheet.Cells[11, 11]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                // this is the side of the 'pallet' that displays the width
                currentSheet.Cells[12, 1] = $"{pallets[i - 1].Width}";
                currentSheet.Range[currentSheet.Cells[12, 1], currentSheet.Cells[31, 1]].Merge();
                range = currentSheet.Range[currentSheet.Cells[12, 1], currentSheet.Cells[31, 1]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                string[] tempArray = pallets[i - 1].ToStringArray();

                for (int y = 12; y < 32; y++)
                {
                    // this creates the lines in the 'pallet' view
                    if (y - 12 < tempArray.Length)
                    {
                        currentSheet.Cells[y, 2] = tempArray[y - 12];
                    }
                    currentSheet.Range[currentSheet.Cells[y, 2], currentSheet.Cells[y, 11]].Merge();
                    range = currentSheet.Range[currentSheet.Cells[y, 2], currentSheet.Cells[y, 11]];
                    range.Borders.LineStyle = XlLineStyle.xlContinuous;
                    range.Borders.Weight = XlBorderWeight.xlThin;
                }

                currentSheet.Cells[12, 13] = "BAR CODE";
                currentSheet.Range[currentSheet.Cells[12, 13], currentSheet.Cells[24, 14]].Merge();
                range = currentSheet.Range[currentSheet.Cells[12, 13], currentSheet.Cells[24, 14]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                // ger cell range of all active cells
                Range rng = currentSheet.UsedRange;
                // set all active cells formatting to centered
                rng.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                rng.VerticalAlignment = XlVAlign.xlVAlignCenter;
                // set all active cells NumberFormat to @
                rng.NumberFormat = "@";
            }
        }

        private static Worksheet CreatePalletListSheet(List<Pallet> pallets, string client, Workbook workBook)
        {
            Worksheet sheet = (Worksheet)workBook.ActiveSheet;
            sheet.Name = "Pallet list";

            //Add table headers going cell by cell.
            sheet.Cells[1, 1] = "Client:";
            sheet.Cells[1, 2] = client;
            sheet.Range[sheet.Cells[1, 2], sheet.Cells[1, 4]].Merge();
            Range range = sheet.Range[sheet.Cells[1, 1], sheet.Cells[1, 4]];
            range.Borders.LineStyle = XlLineStyle.xlContinuous;
            range.Borders.Weight = XlBorderWeight.xlThin;

            sheet.Cells[3, 1] = "Total LDM:";
            float totalLdm = 0;
            for (int i = 0; i < pallets.Count; i++)
            {
                totalLdm += pallets[i].GetLDM();
            }
            // round to 2 decimals
            totalLdm = (float)Math.Round(totalLdm, 2);
            sheet.Cells[3, 3] = $"{totalLdm}";
            sheet.Range[sheet.Cells[3, 1], sheet.Cells[3, 2]].Merge();
            range = sheet.Range[sheet.Cells[3, 1], sheet.Cells[3, 3]];
            range.Borders.LineStyle = XlLineStyle.xlContinuous;
            range.Borders.Weight = XlBorderWeight.xlThin;

            sheet.Cells[3, 5] = "Total weight:";
            sheet.Range[sheet.Cells[3, 5], sheet.Cells[3, 6]].Merge();
            range = sheet.Range[sheet.Cells[3, 5], sheet.Cells[3, 8]];
            range.Borders.LineStyle = XlLineStyle.xlContinuous;
            range.Borders.Weight = XlBorderWeight.xlThin;

            // calculates the total weight of the pallets
            int totalShipmentWeight = 0;
            foreach (var pallet in pallets)
            {
                totalShipmentWeight += pallet.GetPalletWeight();
            }
            sheet.Cells[3, 7] = $"{totalShipmentWeight}kg";
            sheet.Range[sheet.Cells[3, 7], sheet.Cells[3, 8]].Merge();

            sheet.Cells[5, 1] = "Pallet nr.";
            sheet.Cells[5, 2] = "Order nr.";
            sheet.Range[sheet.Cells[5, 2], sheet.Cells[5, 4]].Merge();
            sheet.Cells[5, 5] = "Pallet width";
            sheet.Cells[5, 6] = "Pallet length";
            sheet.Cells[5, 7] = "Total width";
            sheet.Cells[5, 8] = "Total length";
            sheet.Cells[5, 9] = "Total height";
            sheet.Cells[5, 10] = "Pallet Weight";
            sheet.Cells[5, 11] = "Stronger pallet";
            sheet.Cells[5, 12] = "Comment";
            range = sheet.Range[sheet.Cells[5, 1], sheet.Cells[5, 12]];
            range.Borders.LineStyle = XlLineStyle.xlContinuous;
            range.Borders.Weight = XlBorderWeight.xlThin;
            for (int z = 0; z < pallets.Count; z++)
            {
                sheet.Cells[z + 6, 1] = z + 1;
                sheet.Cells[z + 6, 2] = pallets[z].OrderNumber;
                sheet.Range[sheet.Cells[z + 6, 2], sheet.Cells[z + 6, 4]].Merge();
                sheet.Cells[z + 6, 5] = pallets[z].Width;
                sheet.Cells[z + 6, 6] = pallets[z].Length;
                sheet.Cells[z + 6, 7] = pallets[z].Width;
                sheet.Cells[z + 6, 8] = pallets[z].Length + 50;
                sheet.Cells[z + 6, 9] = $"{pallets[z].GetTotalHeight()}";
                sheet.Cells[z + 6, 10] = $"{pallets[z].GetPalletWeight()}kg";
                sheet.Cells[z + 6, 11] = "No";
                sheet.Cells[z + 6, 12] = "";
                range = sheet.Range[sheet.Cells[z + 6, 1], sheet.Cells[z + 6, 12]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;
            }
            // ger cell range of all active cells
            Range rng = sheet.UsedRange;
            // set all active cells formatting to centered
            rng.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            rng.VerticalAlignment = XlVAlign.xlVAlignCenter;
            // set all active cells NumberFormat to @
            rng.NumberFormat = "@";
            sheet.Range[sheet.Cells[3, 3], sheet.Cells[3, 3]].NumberFormat = "0.00";
            sheet.Columns.AutoFit();

            sheet.Select();
            return sheet;
        }

        private static Worksheet CreateDrawingsSheet(List<Pallet> pallets, string client, Workbook workBook)
        {
            // Drawings worksheet logic
            Worksheet sheet = (Worksheet)workBook.Worksheets.Add(After: workBook.Sheets[workBook.Sheets.Count]);
            sheet.Name = "Drawings";

            // used for tracking which cell to write in
            int x = -8;
            int iter = 0;
            foreach (var pallet in pallets)
            {
                Range er = sheet.Range[sheet.Cells[1, 1], sheet.Cells[1, 1]];
                float widthOfPallet = 432;
                float maxWidth = widthOfPallet + widthOfPallet * iter;
                float currentWidth = 0 + widthOfPallet * iter;
                float currentHeight = 35;
                float trackHeight = 0;

                x += 9;
                sheet.Cells[1, x] = $"Client:";
                sheet.Cells[1, x + 1] = client;
                sheet.Cells[2, x] = $"Order nr.:";
                sheet.Cells[2, x + 1] = pallet.OrderNumber;
                sheet.Cells[1, x + 6] = $"Pallet nr.:";
                Range range = sheet.Range[sheet.Cells[1, x + 6], sheet.Cells[1, x + 7]];
                range.NumberFormat = "@";
                sheet.Cells[1, x + 7] = $"{pallet.PalletNumber}/{pallets.Count}";

                // Drawings should be a data structure containing their size
                classes.Picture[] pictures = pallet.GetPictures();
                // pictures to list
                List<classes.Picture> pictureList = new List<classes.Picture>();
                foreach (var picture in pictures)
                {
                    pictureList.Add(picture);
                }
                // sort pictureList by height
                pictureList.Sort((x1, x2) => x2.Height.CompareTo(x1.Height));

                for (int i = 0; i < pictureList.Count; i++)
                {
                    // implement drawings placement logic here:
                    if (trackHeight < pictureList[i].Height)
                    {
                        trackHeight = pictureList[i].Height;
                    }
                    _ = sheet.Shapes.AddPicture(pictureList[i].Path, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, currentWidth, currentHeight, pictureList[i].Width, pictureList[i].Height);
                    currentWidth += pictureList[i].Width;
                    int nextWidth = i + 1 > pictureList.Count - 1 ? 0 : pictureList[i + 1].Width;
                    if (currentWidth + nextWidth > maxWidth)
                    {
                        currentWidth = 0 + widthOfPallet * iter;
                        currentHeight += trackHeight;
                        trackHeight = 0;
                    }
                    // ----------------------------------------
                }
                iter++;
            }
            Range rng = sheet.UsedRange;
            rng.NumberFormat = "@";

            return sheet;
        }
    }
}
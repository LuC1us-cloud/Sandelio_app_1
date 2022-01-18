using Microsoft.Office.Interop.Excel;
using Sandelio_app_1.classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Sandelio_app_1.controllers
{
    internal class ExcelController
    {
        // Drawings page logic is still NAN
        public static void CreateFile(List<Pallet> pallets, string client, string[] drawingPaths)
        {
            Application excel;
            Workbook workBook;
            Worksheet palletListSheet;
            Worksheet drawingsWorkSheet;
                Debug.WriteLine(drawingPaths);
            
            try
            {
                //Formatting last

                //Start Excel and get Application object.
                excel = new Application
                {
                    Visible = true
                };

                //Get a new workbook.
                workBook = excel.Workbooks.Add(Missing.Value);
                palletListSheet = (Worksheet)workBook.ActiveSheet;
                palletListSheet.Name = "Pallet list";
                drawingsWorkSheet = (Worksheet)workBook.Worksheets.Add();
                drawingsWorkSheet.Name = "Drawings";
                drawingsWorkSheet.Shapes.AddPicture(drawingPaths[0], Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 0, 0, 0, 0);
                //Add table headers going cell by cell.
                palletListSheet.Cells[1, 1] = "Client:";
                palletListSheet.Cells[1, 2] = client;
                palletListSheet.Range[palletListSheet.Cells[1, 2], palletListSheet.Cells[1, 4]].Merge();
                Microsoft.Office.Interop.Excel.Range range = palletListSheet.Range[palletListSheet.Cells[1, 1], palletListSheet.Cells[1, 4]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                palletListSheet.Cells[3, 1] = "Total LDM:";
                palletListSheet.Cells[3, 3] = "1.5";
                palletListSheet.Range[palletListSheet.Cells[3, 1], palletListSheet.Cells[3, 2]].Merge();
                range = palletListSheet.Range[palletListSheet.Cells[3, 1], palletListSheet.Cells[3, 3]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                palletListSheet.Cells[3, 5] = "Total weight:";
                palletListSheet.Range[palletListSheet.Cells[3, 5], palletListSheet.Cells[3, 6]].Merge();
                range = palletListSheet.Range[palletListSheet.Cells[3, 5], palletListSheet.Cells[3, 8]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;

                // calculates the total weight of the pallets
                int totalWeight = 0;
                foreach (var item in pallets)
                {
                    totalWeight += item.GetPalletWeight();
                }
                palletListSheet.Cells[3, 7] = $"{totalWeight}kg";
                palletListSheet.Range[palletListSheet.Cells[3, 7], palletListSheet.Cells[3, 8]].Merge();

                palletListSheet.Cells[5, 1] = "Pallet nr.";
                palletListSheet.Cells[5, 2] = "Order nr.";
                palletListSheet.Range[palletListSheet.Cells[5, 2], palletListSheet.Cells[5, 4]].Merge();
                palletListSheet.Cells[5, 5] = "Pallet width";
                palletListSheet.Cells[5, 6] = "Pallet length";
                palletListSheet.Cells[5, 7] = "Total width";
                palletListSheet.Cells[5, 8] = "Total length";
                palletListSheet.Cells[5, 9] = "Total height";
                palletListSheet.Cells[5, 10] = "Pallet Weight";
                palletListSheet.Cells[5, 11] = "Stronger pallet";
                palletListSheet.Cells[5, 12] = "Comment";
                range = palletListSheet.Range[palletListSheet.Cells[5, 1], palletListSheet.Cells[5, 12]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Weight = XlBorderWeight.xlThin;
                for (int z = 0; z < pallets.Count; z++)
                {
                    palletListSheet.Cells[z + 6, 1] = z + 1;
                    palletListSheet.Cells[z + 6, 2] = pallets[z].OrderNumber;
                    palletListSheet.Range[palletListSheet.Cells[z + 6, 2], palletListSheet.Cells[z + 6, 4]].Merge();
                    palletListSheet.Cells[z + 6, 5] = pallets[z].Width;
                    palletListSheet.Cells[z + 6, 6] = pallets[z].Length;
                    palletListSheet.Cells[z + 6, 7] = pallets[z].Width;
                    palletListSheet.Cells[z + 6, 8] = pallets[z].Length + 50;
                    palletListSheet.Cells[z + 6, 9] = $"{pallets[z].GetTotalHeight()}";
                    palletListSheet.Cells[z + 6, 10] = $"{pallets[z].GetPalletWeight()}kg";
                    palletListSheet.Cells[z + 6, 11] = "No";
                    palletListSheet.Cells[z + 6, 12] = "";
                    range = palletListSheet.Range[palletListSheet.Cells[z + 6, 1], palletListSheet.Cells[z + 6, 12]];
                    range.Borders.LineStyle = XlLineStyle.xlContinuous;
                    range.Borders.Weight = XlBorderWeight.xlThin;
                }
                // ger cell range of all active cells
                Microsoft.Office.Interop.Excel.Range rng = palletListSheet.UsedRange;
                // set all active cells formatting to centered
                rng.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                rng.VerticalAlignment = XlVAlign.xlVAlignCenter;
                // set all active cells NumberFormat to @
                rng.NumberFormat = "@";
                palletListSheet.Columns.AutoFit();

                palletListSheet.Select();
                for (int i = 1; i < pallets.Count + 1; i++)
                {
                    Worksheet currentSheet = (Worksheet)workBook.Worksheets.Add();
                    currentSheet.Name = $"Pallet {i}";

                    range = currentSheet.Range[currentSheet.Cells[2, 1], currentSheet.Cells[4, 4]];
                    range.Borders.LineStyle = XlLineStyle.xlContinuous;
                    range.Borders.Weight = XlBorderWeight.xlThin;
                    currentSheet.Cells[2, 1] = "Client:";
                    currentSheet.Cells[2, 2] = client;
                    currentSheet.Range[currentSheet.Cells[2, 2], currentSheet.Cells[2, 4]].Merge();
                    currentSheet.Cells[3, 1] = "Order nr.:";
                    currentSheet.Range[currentSheet.Cells[3, 1], currentSheet.Cells[4, 1]].Merge();
                    currentSheet.Cells[3, 3] = pallets[i - 1].PalletNumber;

                    currentSheet.Range[currentSheet.Cells[3, 2], currentSheet.Cells[4, 4]].Merge();
                    currentSheet.Cells[8, 1] = "LDM:";
                    currentSheet.Cells[8, 2] = pallets[i - 1].Width * pallets[i - 1].Length / 1000 / 2.4;
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

                    // this is the side of the 'pallet' that displays the width
                    currentSheet.Cells[10, 2] = $"{pallets[i - 1].Width}";
                    currentSheet.Range[currentSheet.Cells[10, 2], currentSheet.Cells[11, 11]].Merge();
                    range = currentSheet.Range[currentSheet.Cells[10, 2], currentSheet.Cells[11, 11]];
                    range.Borders.LineStyle = XlLineStyle.xlContinuous;
                    range.Borders.Weight = XlBorderWeight.xlThin;

                    // this is the side of the 'pallet' that displays the length
                    currentSheet.Cells[12, 1] = $"{pallets[i - 1].Length}";
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
                            currentSheet.Cells[y, 2] = tempArray[y - 12].ToString();
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
                    rng = currentSheet.UsedRange;
                    // set all active cells formatting to centered
                    rng.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    rng.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    // set all active cells NumberFormat to @
                    rng.NumberFormat = "@";
                }
                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                excel.Visible = true;
                excel.UserControl = true;
            }
            catch (Exception theException)
            {
                string errorMessage;
                errorMessage = "Error: ";
                errorMessage = string.Concat(errorMessage, theException.Message);
                errorMessage = string.Concat(errorMessage, " Line: ");
                errorMessage = string.Concat(errorMessage, theException.Source);

                _ = MessageBox.Show(errorMessage, "Error");
            }
        }
    }
}
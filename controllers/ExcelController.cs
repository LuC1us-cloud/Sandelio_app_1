using Sandelio_app_1.classes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace Sandelio_app_1.controllers
{
    internal class ExcelController
    {
        // LDM still empty, height not calculated, border formatting issues, Drawings page logic is still NAN
        public static void CreateFile(List<Pallet> pallets, string client)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet palletListSheet;
            Microsoft.Office.Interop.Excel._Worksheet wsDrawings;

            try
            {
                //Formatting last

                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = true
                };

                //Get a new workbook.
                oWB = oXL.Workbooks.Add(Missing.Value);
                palletListSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
                palletListSheet.Name = "Pallet list";
                wsDrawings = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Worksheets.Add();
                wsDrawings.Name = "Drawings";
                //Add table headers going cell by cell.
                palletListSheet.Cells[1, 1] = "Client:";
                palletListSheet.Cells[1, 2] = client;
                palletListSheet.Range[palletListSheet.Cells[1, 2], palletListSheet.Cells[1, 4]].Merge();
                Microsoft.Office.Interop.Excel.Range range = palletListSheet.Range[palletListSheet.Cells[1, 1], palletListSheet.Cells[1, 4]];
                range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                palletListSheet.Cells[3, 1] = "Total LDM:";
                palletListSheet.Cells[3, 3] = "";
                palletListSheet.Range[palletListSheet.Cells[3, 1], palletListSheet.Cells[3, 3]].Merge();
                range = palletListSheet.Range[palletListSheet.Cells[3, 1], palletListSheet.Cells[3, 2]];
                range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                palletListSheet.Cells[3, 5] = "Total weight:";
                palletListSheet.Range[palletListSheet.Cells[3, 5], palletListSheet.Cells[3, 6]].Merge();
                range = palletListSheet.Range[palletListSheet.Cells[3, 5], palletListSheet.Cells[3, 8]];
                range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

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
                range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
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
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                }
                // ger cell range of all active cells
                Microsoft.Office.Interop.Excel.Range rng = palletListSheet.UsedRange;
                // set all active cells formatting to centered
                rng.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                rng.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                // set all active cells NumberFormat to @
                rng.NumberFormat = "@";
                palletListSheet.Columns.AutoFit();

                palletListSheet.Select();
                for (int i = 1; i < pallets.Count + 1; i++)
                {
                    Microsoft.Office.Interop.Excel.Worksheet ws1 = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Worksheets.Add();
                    ws1.Name = $"Pallet {i}";

                    range = ws1.Range[ws1.Cells[2, 1], ws1.Cells[4, 1]];
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    ws1.Cells[2, 1] = "Client:";
                    ws1.Cells[2, 2] = client;
                    ws1.Range[ws1.Cells[2, 2], ws1.Cells[2, 4]].Merge();
                    ws1.Cells[3, 1] = "Order nr.:";
                    ws1.Range[ws1.Cells[3, 1], ws1.Cells[4, 1]].Merge();
                    ws1.Cells[3, 3] = pallets[i - 1].PalletNumber;

                    ws1.Range[ws1.Cells[3, 2], ws1.Cells[4, 4]].Merge();
                    ws1.Cells[8, 1] = "LDM:";
                    ws1.Cells[8, 2] = "";
                    ws1.Cells[8, 4] = "Height:";
                    ws1.Cells[8, 5] = pallets[i - 1].Height;
                    ws1.Cells[8, 7] = "Width:";
                    ws1.Cells[8, 8] = pallets[i - 1].Width;
                    ws1.Cells[8, 10] = "Length:";
                    ws1.Cells[8, 11] = pallets[i - 1].Length;
                    ws1.Cells[8, 13] = "Weight:";
                    ws1.Cells[8, 14] = pallets[i - 1].GetPalletWeight() + "kg";
                    range = ws1.Range[ws1.Cells[8, 1], ws1.Cells[8, 2]];
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    range = ws1.Range[ws1.Cells[8, 4], ws1.Cells[8, 5]];
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    range = ws1.Range[ws1.Cells[8, 7], ws1.Cells[8, 8]];
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    range = ws1.Range[ws1.Cells[8, 10], ws1.Cells[8, 11]];
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    range = ws1.Range[ws1.Cells[8, 13], ws1.Cells[8, 14]];
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    ws1.Cells[2, 7] = "Delivery address:";
                    ws1.Range[ws1.Cells[2, 7], ws1.Cells[2, 8]].Merge();
                    ws1.Cells[3, 7] = "Post code:";
                    ws1.Range[ws1.Cells[3, 7], ws1.Cells[3, 8]].Merge();
                    ws1.Cells[4, 7] = "City:";
                    ws1.Range[ws1.Cells[4, 7], ws1.Cells[4, 8]].Merge();
                    ws1.Cells[5, 7] = "Country:";
                    ws1.Range[ws1.Cells[5, 7], ws1.Cells[5, 8]].Merge();
                    ws1.Cells[6, 7] = "Loading date:";
                    ws1.Range[ws1.Cells[6, 7], ws1.Cells[6, 8]].Merge();

                    range = ws1.Range[ws1.Cells[2, 7], ws1.Cells[6, 9]];
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    ws1.Cells[2, 9] = pallets[i - 1].Adress;
                    ws1.Range[ws1.Cells[2, 9], ws1.Cells[2, 10]].Merge();
                    ws1.Cells[3, 9] = pallets[i - 1].PostCode;
                    ws1.Range[ws1.Cells[3, 9], ws1.Cells[3, 10]].Merge();
                    ws1.Cells[4, 9] = pallets[i - 1].City;
                    ws1.Range[ws1.Cells[4, 9], ws1.Cells[4, 10]].Merge();
                    ws1.Cells[5, 9] = pallets[i - 1].Country;
                    ws1.Range[ws1.Cells[5, 9], ws1.Cells[5, 10]].Merge();
                    ws1.Cells[6, 9] = "";
                    ws1.Range[ws1.Cells[6, 9], ws1.Cells[6, 10]].Merge();

                    ws1.Cells[2, 11] = "Pallet nr.:";
                    ws1.Range[ws1.Cells[2, 12], ws1.Cells[2, 12]].NumberFormat = "@";
                    ws1.Cells[2, 12] = $"{i}/{pallets.Count}";
                    range = ws1.Range[ws1.Cells[2, 11], ws1.Cells[2, 12]];
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    // this is the side of the 'pallet' that displays the width
                    ws1.Cells[10, 2] = $"{pallets[i - 1].Width}";
                    ws1.Range[ws1.Cells[10, 2], ws1.Cells[11, 11]].Merge();
                    range = ws1.Range[ws1.Cells[10, 2], ws1.Cells[11, 11]];
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    // this is the side of the 'pallet' that displays the length
                    ws1.Cells[12, 1] = $"{pallets[i - 1].Length}";
                    ws1.Range[ws1.Cells[12, 1], ws1.Cells[31, 1]].Merge();
                    range = ws1.Range[ws1.Cells[12, 1], ws1.Cells[31, 1]];
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    string[] tempArray = pallets[i - 1].ToStringArray();
                    for (int y = 12; y < 32; y++)
                    {
                        // this creates the lines in the 'pallet' view
                        if (y - 12 < tempArray.Length)
                        {
                            ws1.Cells[y, 2] = tempArray[y - 12].ToString();
                        }
                        ws1.Range[ws1.Cells[y, 2], ws1.Cells[y, 11]].Merge();
                        range = ws1.Range[ws1.Cells[y, 2], ws1.Cells[y, 11]];
                        range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    }

                    ws1.Cells[12, 13] = "BAR CODE";
                    ws1.Range[ws1.Cells[12, 13], ws1.Cells[24, 14]].Merge();
                    range = ws1.Range[ws1.Cells[12, 13], ws1.Cells[24, 14]];
                    range.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    range.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;


                    // ger cell range of all active cells
                    rng = ws1.UsedRange;
                    // set all active cells formatting to centered
                    rng.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    rng.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    // set all active cells NumberFormat to @
                    rng.NumberFormat = "@";

                }
                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
        }
    }
}
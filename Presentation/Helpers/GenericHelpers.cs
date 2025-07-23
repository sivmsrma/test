
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Diagnostics;

namespace Terret_Billing.Presentation.Helpers
{
    public static class GenericHelpers
    {

        public static string GetStateName(string stateCode)
        {
            var stateName = string.Empty;

            switch (stateCode)
            {
                case "01":
                    stateName = "Jammu & Kashmir";
                    // Code to execute if variable == value1
                    break;

                case "02":
                    stateName = "Himachal Pradesh";
                    // Code to execute if variable == value2
                    break;

                case "03":
                    stateName = "Punjab";
                    // Code to execute if varaiable == value3
                    break;

                case "04":
                    stateName = "Chandigarh";
                    // Code to execute if variable ==4
                    break;

                case "05":
                    stateName = "Uttarakhand";
                    // Code to execute if variable ==5
                    break;

                case "06":
                    stateName = "Harayana";
                    // Code to execute if varaible ==6
                    break;

                case "07":
                    stateName = "Delhi";
                    // Code to execute if variable ==7
                    break;

                case "08":
                    stateName = "Rajasthan";
                    // Code to execute if variabale ==8
                    break;

                case "09":
                    stateName = "Uttar Pradesh";
                    // Code to execute if variable ==9
                    break;

                case "10":
                    stateName = "Bihar";
                    // Code to execute if variable ==10
                    break;

                case "11":
                    stateName = "Sikkim";
                    //Code to execute if variable ==11;
                    break;

                case "12":
                    stateName = "Arunchal Pradesh";
                    // Code to execute if variable ==12
                    break;

                case "13":
                    stateName = "Nagaland";
                    //Code to execute if variable ==13
                    break;

                case "14":
                    stateName = "Manipur";
                    // Code to execute if variable ==14
                    break;

                case "15":
                    stateName = "Mizoram";
                    // Code to execute if varibale ==15
                    break;

                case "16":
                    stateName = "Tripura";
                    // Code to execute if variable ==16
                    break;

                case "17":
                    stateName = "Meghalaya";
                    // Code to execute if variable ==17
                    break;

                case "18":
                    stateName = "Assam";
                    // Code to execuite if variable ==18
                    break;

                case "19":
                    stateName = "West bengal";
                    // Code to execute if variable ==19
                    break;

                case "20":
                    stateName = "Jharkhand";
                    // Code ton execute if variable ==20
                    break;

                case "21":
                    stateName = "Odisha";
                    // Code to execute if varaible ==21
                    break;

                case "22":
                    stateName = "Chhattisgarh";
                    // Code to execute if varibale ==22
                    break;

                case "23":
                    stateName = "Madhya Pradesh";
                    // Code to execute if varibale ==23
                    break;

                case "24":
                    stateName = "Gujarat";
                    // Code to execute if variable ==24
                    break;

                case "26":
                    stateName = "Dadra and Nagar Haveli and Daman and Diu";
                    // Code to execute if variable ==26
                    break;

                case "27":
                    stateName = "Maharashtra";
                    // Code to execute if variable ==27
                    break;

                case "28":
                    stateName = "Andhra Pradesh";
                    // Code to execute if variable ==28
                    break;

                case "29":
                    stateName = "Karnataka";
                    // Code to execute if variable ==29
                    break;

                case "30":
                    stateName = "Goa";
                    // Code to execute if variable ==30
                    break;

                case "31":
                    stateName = "Lakshadweep";
                    // Code to execute if variable ==31
                    break;

                case "32":
                    stateName = "Kerala";
                    // Code to execute if variable ==32
                    break;

                case "33":
                    stateName = "Tamil Nadu";
                    // Code to execute if variable ==33
                    break;

                case "34":
                    stateName = "Puducherry";
                    // Code to execute if variable ==34
                    break;

                case "35":
                    stateName = "Andaman and Nicobar Islands";
                    // Code to execute if variable ==35
                    break;

                case "36":
                    stateName = "Telangana";
                    // Code to execute if variable ==36
                    break;

                case "37":
                    stateName = "Andhra Pradesh ( New)";
                    // Code to execute if variable ==37
                    break;

                case "38":
                    stateName = "Ladakh";
                    // Code to execute if variable ==38
                    break;

                default:
                    break;
            }

            return stateName;
        }

        public static bool ExportToExcel<T>(IEnumerable<T> data, string filePath)
        {
            try
            {
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Data");
                var properties = typeof(T).GetProperties();

                for (int i = 0; i < properties.Length; i++)
                    worksheet.Cell(1, i + 1).Value = properties[i].Name;

                int row = 2;
                foreach (var item in data)
                {
                    for (int i = 0; i < properties.Length; i++)
                        worksheet.Cell(row, i + 1).Value = properties[i].GetValue(item)?.ToString() ?? "";
                    row++;
                }

                workbook.SaveAs(filePath);

                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}
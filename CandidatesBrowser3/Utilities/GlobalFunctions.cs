using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace CandidatesBrowser3.Utilities
{
    public static class GlobalFunctions
    {
        public static void CopyProperties<T>(T objectToCopy, T objectToPaste)
        {
            foreach(PropertyInfo propertyToGet in objectToCopy.GetType().GetProperties().OrderBy(e=>e.Name))
            {
                foreach(PropertyInfo propertyToSet in objectToPaste.GetType().GetProperties().OrderBy(e => e.Name))
                {
                    if (propertyToGet.Name.Equals(propertyToSet.Name))
                    {
                        propertyToSet.SetValue(objectToPaste, propertyToGet.GetValue(objectToCopy));
                    }

                }
            }

        }


        public static DataTable ToDataTableFromList<T>( List<T> List )
        {
            DataTable dt = new DataTable();

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor item in properties)
            {
                dt.Columns.Add(item.Name, Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType);
            }

            foreach (T item in List)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyDescriptor property in properties)
                {
                    row[property.Name] = property.GetValue(item);
                }
                dt.Rows.Add(row);
            }

            
            return dt;
        }

        public static DataTable ToDataTableFromList<T>(List<T> List, string[] columnsToInclude)
        {
            DataTable dt = new DataTable();

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            foreach (var column in columnsToInclude)
            {
                foreach (PropertyDescriptor item in properties)
                {
                    if(item.Name==column)
                        dt.Columns.Add(item.Name, Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType);
                }
            }
           

            foreach (T item in List)
            {
                DataRow row = dt.NewRow();
                foreach (var column in columnsToInclude)
                {
                    foreach (PropertyDescriptor property in properties)
                    {
                        if (property.Name == column)
                            row[property.Name] = property.GetValue(item);
                    }
                }
              
                dt.Rows.Add(row);
            }


            return dt;
        }


        public static void ExportToExcel(DataTable DT)
        {
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = false;

            xlexcel.DisplayAlerts = false;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);

            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);


            for (int i = 0; i < DT.Columns.Count; i++)
            {
                xlWorkSheet.Cells[1, i+1] = DT.Columns[i].ColumnName;
            }

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                // to do: format datetime values before printing
                for (int j = 0; j < DT.Columns.Count; j++)
                {
                    xlWorkSheet.Cells[(i +  2), (j +1)] = DT.Rows[i][j];
                }
            }

            Microsoft.Office.Interop.Excel.Range XlRange;
            XlRange = xlWorkSheet.UsedRange;
            int lastcol = XlRange.Columns.Count + 1;
                
            XlRange.Borders.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlack;
            XlRange.Font.Name = "Arial";
            XlRange.Cells.Font.Size = 10;

            for (int i = 1; i <= lastcol; i++)
            {
                
                {
                    Microsoft.Office.Interop.Excel.Range allColumns = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Columns[i];
                    allColumns.AutoFit();
                }

            }

            Microsoft.Office.Interop.Excel.Range HeaderRow = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Range[xlWorkSheet.Cells[1, 1], xlWorkSheet.Cells[1, lastcol]];
            HeaderRow.Cells.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightGray;
            HeaderRow.Cells.Font.FontStyle = "Arial";
            HeaderRow.Font.Name = "Arial";
            HeaderRow.Cells.Font.Size = 11;
            HeaderRow.Cells.Font.Bold = true;


            xlexcel.Visible = true;

        }

    }
}

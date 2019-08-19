using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

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

        public static bool DeleteFile(string[] sourceFilePaths, List<Attachment> attachmentsList)
        {
            var result = false;
            foreach (string sourceFilePath in sourceFilePaths)
            {
                try
                {
                    File.Delete(sourceFilePath);
                    MessageBox.Show("File deleted succesfully ", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    result = true;
                    attachmentsList.Remove(attachmentsList.Where(e => e.Path.Equals(sourceFilePath)).FirstOrDefault());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File was not deleted! " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return result;
        }

        public static bool SaveFile(string[] sourceFilePaths, string id, string destinationDirectory, List<Attachment> attachmentsList)
        {
            var result = false;
            foreach (string sourceFilePath in sourceFilePaths)
            {
                string fileName = System.IO.Path.GetFileName(sourceFilePath);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                try
                {
                    File.Copy(sourceFilePath, destinationDirectory + @"\" + fileName);


                    MessageBox.Show("File attached succesfully ", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    result = true;
                    attachmentsList.Add(new Attachment(destinationDirectory + @"\" + fileName));

                }
                catch (Exception ex)
                {
                    MessageBox.Show("File was not attached! " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                    // result =false;
                }
            }
            return result;
        }

        public static void ExportToExcel(DataTable DT,string projectName)
        {
            Excel.Application xlexcel;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Excel.Application
            {
                Visible = false,

                DisplayAlerts = false
            };
            xlWorkBook = xlexcel.Workbooks.Add(misValue);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);


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

            Excel.Range XlRange;
            XlRange = xlWorkSheet.UsedRange;
            int lastcol = XlRange.Columns.Count + 1;
                
            XlRange.Borders.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlack;
            XlRange.Font.Name = "Arial";
            XlRange.Cells.Font.Size = 10;

            for (int i = 1; i <= lastcol; i++)
            {                
                {
                    Excel.Range allColumns = (Excel.Range)xlWorkSheet.Columns[i];
                    allColumns.AutoFit();
                }
            }

            Excel.Range HeaderRow = (Excel.Range)xlWorkSheet.Range[xlWorkSheet.Cells[1, 1], xlWorkSheet.Cells[1, lastcol]];
            HeaderRow.Cells.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightGray;
            HeaderRow.Cells.Font.FontStyle = "Arial";
            HeaderRow.Font.Name = "Arial";
            HeaderRow.Cells.Font.Size = 11;
            HeaderRow.Cells.Font.Bold = true;


            xlWorkSheet.Name = projectName;

            xlexcel.Visible = true;

        }

        public static DataTable ImportDataToDataTable(string path)
        {
            Excel.Application xlexcel=null;
            Excel.Workbook xlWorkBook=null;
            Excel.Worksheet xlWorkSheet=null;
            
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Excel.Application
            {
                Visible = false,

                DisplayAlerts = false
            };
            xlWorkBook=xlexcel.Workbooks.Open(path);
            xlWorkSheet = xlWorkBook.Worksheets.get_Item(1);
            int firstRownumber = xlWorkSheet.Range["B:B"].Find("no").Row;           
            int lastRownumber = firstRownumber;
            while ((xlWorkSheet.Cells[lastRownumber, 2] as Excel.Range).Value!=null)
            {
                lastRownumber++;
            }

            Excel.Range rangeToImport = xlWorkSheet.Range[xlWorkSheet.Cells[firstRownumber, 3], xlWorkSheet.Cells[lastRownumber, 10]].Cells;
            object[,] array = (object[,])rangeToImport.Value;

            DataTable dt = new DataTable();
            for (int i = 1; i <= array.GetLength(1); i++)
            {
                dt.Columns.Add(array[1, i].ToString());
            }

            for (int j = 2; j < array.GetLength(0); j++)
            {
                // create a DataRow using .NewRow()
                DataRow row = dt.NewRow();

                // iterate over all columns to fill the row
                for (int i = 1; i <= array.GetLength(1); i++)
                {
                    row[i-1] = array[j, i];
                }

                // add the current row to the DataTable
                dt.Rows.Add(row);
            }

            xlWorkBook.Close();
            xlexcel.Quit();
            return dt;
        }
    }
}

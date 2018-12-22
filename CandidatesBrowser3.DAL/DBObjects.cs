using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CandidatesBrowser3.DAL
{
    public  static class DBObjects
    {
        public static string DBName;
        public static string ConnectionStringZaneta;//= @"Server=ZANETA-PC\SQLEXPRESS;database=" + DBName +";integrated Security=SSPI";
        public static string ConnectionStringMichal; //= @"Server=DESKTOP-3U4D69V\SQLEXPRESS;database=" + DBName +";integrated Security=SSPI";
        public static string ConnectionString;

        public static void SetConnectionString()
        {
         ConnectionStringZaneta = @"Server=ZANETA-PC\SQLEXPRESS;database=" + DBName +";integrated Security=SSPI; timeout=5000";
         ConnectionStringMichal = @"Server=DESKTOP-3U4D69V\SQLEXPRESS;database=" + DBName + ";integrated Security=SSPI; timeout=5000";
        }
        public static string ReadScalar(string SQL)
        {
            System.Data.DataTable results = new System.Data.DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString);

            string result = null;
            SqlCommand command = new SqlCommand(SQL, connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            int attemp = 1;
            bool success = false;

            for (attemp = 1; attemp < 4; attemp++)
            {
                if (success == true)
                {
                    break;
                }
                else
                {
                    try
                    {
                        connection.Open();
                        success = true;
                    }
                    catch
                    {
                        // string x = MessageBox.Show("System tried " + attemp + "time(s) to connect the database. Would you like to continue?", "Connection issue", MessageBoxButtons.YesNo).ToString();

                    }
                }

            }



            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            if (reader.HasRows == true)
            { result = reader[0].ToString(); }
            connection.Close();
            return result;

        }

        public static System.Data.DataTable GetTableFromSQL(string sql)
        {
                DataTable table = new DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, ConnectionString);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
           
                dataAdapter.SelectCommand.CommandTimeout = 5000;
                //dataAdapter.SelectCommand.Parameters.Add()
       
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                                 
            return table;
        }
        public static System.Data.DataTable GetTableFromSQL(string procedureName, Dictionary<string, string> Args)
        {
            DataTable table = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.SelectCommand = new SqlCommand();
            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            dataAdapter.SelectCommand.CommandText = procedureName;
            dataAdapter.SelectCommand.CommandTimeout = 5000;
            dataAdapter.SelectCommand.Connection = sqlConnection;
            sqlConnection.Open();
            
            //cmd.CommandTimeout = 1000;
            

            foreach (var param in Args)
            {
                dataAdapter.SelectCommand.Parameters.AddWithValue(param.Key, param.Value);
            }
            //dataAdapter.SelectCommand.Parameters.Add()

            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dataAdapter.Fill(table);

            return table;
            //DataTable table = new DataTable();
            //SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            //SqlCommand cmd = new SqlCommand();

            //cmd.CommandText = procedureName;
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandTimeout = 1000;
            //cmd.Connection = sqlConnection;
            //sqlConnection.Open();

            //foreach (var param in Args)
            //{
            //    cmd.Parameters.AddWithValue(param.Key, param.Value);
            //}

            //cmd.

            //return table;
        }

        public static void ExecProcedureWithArgs(string procedureName, Dictionary<string,string>Args)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Connection = sqlConnection;
            sqlConnection.Open();

            foreach (var param in Args)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }

            cmd.ExecuteNonQuery();
        }

    }
}

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

        public static DataTable GetTableFromSQL(string sql)
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
        public static DataTable GetTableFromSQL(string procedureName, Dictionary<string, string> Args)
        {
            DataTable table = new DataTable();
            try
            {
             
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                SqlConnection sqlConnection = new SqlConnection(ConnectionString);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.SelectCommand = new SqlCommand();
                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter.SelectCommand.CommandText = procedureName;
                dataAdapter.SelectCommand.CommandTimeout = 5000;
                dataAdapter.SelectCommand.Connection = sqlConnection;
                sqlConnection.Open();


                foreach (var param in Args)
                {
                    dataAdapter.SelectCommand.Parameters.AddWithValue(param.Key, param.Value);
                }


                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);

            }
            catch(Exception ex)
            {
                table = null;
            }


            return table;
      
        }

        public static void ExecSqlProcedure(string procedureName, Dictionary<dynamic, dynamic> Args)
        {
            try
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
            
            catch(Exception ex)
            {

            }
           

        }
        public static void ExecSqlProcedure(string procedureName)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000;
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                

                cmd.ExecuteNonQuery();

            }

            catch (Exception ex)
            {

            }


        }

        public static void UploadFileIntoDB(DataTable dt, string tableName, string sqlToCreate)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlToCreate, connection);
                command.ExecuteNonQuery();
                 SqlBulkCopy bulkCopy = new SqlBulkCopy(
                    connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                    );
                try
                {
                    bulkCopy.DestinationTableName = "##importTable";

                    bulkCopy.WriteToServer(dt);

                    ExecSqlProcedure("UPDATE_INFO_FROM_FILE");
                    
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    connection.Close();
                }
                // set the destination table name
               
            }
        }
        public static object GetExecProcedureWithArgsResult(string procedureName, Dictionary<dynamic, dynamic> Args)
        {
            object result=null;
            try
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
                cmd.Parameters.Add("@NewID",SqlDbType.Int);
                cmd.Parameters["@NewID"].Direction = ParameterDirection.Output; 
               // result = cmd.Parameters.Add("@NewID");
                cmd.ExecuteNonQuery();
                result = cmd.Parameters["@NewID"].Value;
            }

            catch (Exception ex)
            {

            }


            return result;
        }

    }
}

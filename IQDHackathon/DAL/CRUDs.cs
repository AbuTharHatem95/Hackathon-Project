using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class CRUDs
    {


        private static SqlConnection _Connection = new SqlConnection(clsSettings.connectionString);

        public static int AddNew(SqlCommand command)
        {
            command.Connection = _Connection;

            int ID = -1;
            try
            {
                _Connection.Open();
                object Reuslt = command.ExecuteScalar();
                if (Reuslt != null && int.TryParse(Reuslt.ToString(), out int R))
                {
                    ID = R;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _Connection.Close();
            }
            return ID;
        }

        public static DataTable GetAll(SqlCommand command)
        {
            DataTable dt = new DataTable();

            command.Connection = _Connection;
            try
            {
                _Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _Connection.Close();

            }
            return dt;
        }


        public static DataTable GetAllUsers(SqlCommand cmd)
        {

            DataTable dt = new DataTable();
            //SqlConnection con = new SqlConnection(clsSettings.connectionString);
            //string Query = "Select * From Users";
            cmd.Connection = _Connection;
            try
            {
                _Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }
            catch
            {

            }
            finally
            {
                _Connection.Close();
            }



            return dt;
        }

        public static bool UpdateOrDelete(SqlCommand command)
        {
            command.Connection = _Connection;
            int RowsAffected = 0;
            try
            {
                _Connection.Open();
                RowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _Connection.Close();
            }
            return RowsAffected > 0;
        }


        public static bool IsExist(SqlCommand command)
        {
            command.Connection = _Connection;
            bool isFound = false;
            try
            {
                _Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    isFound = true;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _Connection.Close();
            }
            return isFound;

        }







    }


}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Domain.Interfaces;

namespace AUPPRB.Domain.Services
{
    public class SyncronizationService:ISyncronizationService
    {    

        public List<string> GetDataFrom_t0005_ВремяЗанятия(string connString)
        {
            List<string> list = new List<string>();

            DataSet ds = new DataSet();
            OleDbConnection conn = GetConnection(connString);
            OleDbDataAdapter da = new OleDbDataAdapter("Select * from t0005_ВремяЗанятия", conn);
            da.Fill(ds);
            conn.Close();
            DataTable dt = ds.Tables[0];
            foreach (DataRow rows in dt.Rows)
            {
                list.Add(rows["Нач"].ToString() + " " + rows["Кон"].ToString());
              
            }
            return list;
        }



        private OleDbConnection GetConnection(string connString)
        {
            OleDbConnection oleDbConnection = new OleDbConnection();

            try
            {

                String connectionString = @"Provider=Microsoft.JET.OlEDB.4.0;"
             + @"Data Source=D:\4 курс учеба\AUPPRB\V09_ВУЗ.mdb";//TODO: moove to webconfig

                oleDbConnection = new OleDbConnection(connString);
                oleDbConnection.Open();

            }
            catch (Exception ex)
            {

                throw new Exception("Ошибка при открытии соединения к БД");
            }

            return oleDbConnection;
        }


        private void CloseConnection(string connString)
        {
            try
            {
                OleDbConnection oleDbConnection = new OleDbConnection(connString);

                oleDbConnection.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при закрытии соединения с БД");
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SerreVeure.Data
{
    public abstract class DataConnectorBase : IDisposable
    {
        protected SqlConnection cnx = null;

        protected DataConnectorBase()
        {
            try
            {
                cnx = new SqlConnection("Server=DESKTOP-OEVD34U;Database=SuperFlux;User Id=SuperUser;Password=MySecurePassword123!;");
                cnx.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ouverture de la connexion : {ex.Message}");
                throw;
            }
        }

        protected void ExecuteNonQuery(string sql)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, cnx))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la commande SQL : {ex.Message}");
                throw;
            }
        }

        protected DataTable ExecuteQuery(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand(sql, cnx))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la requête SQL : {ex.Message}");
                throw;
            }
        }

        protected List<T> ConvertDataTableToList<T>(DataTable dt, Func<DataRow, T> convert)
        {
            if (convert == null)
                throw new InvalidOperationException("Le paramètre convert ne peut pas être null");

            List<T> result = new List<T>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(convert(dr));
                }
            }
            return result;
        }

        #region Disposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (cnx != null)
                {
                    cnx.Close();
                    cnx.Dispose();
                    cnx = null;
                }
            }
        }

        #endregion
    }
}

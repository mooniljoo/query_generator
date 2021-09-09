using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;

/// <summary>
/// Database의 요약 설명입니다.
/// </summary>
///
namespace AppCommon
{
    public class Database
    {

        private SqlConnection conn = null;
        private SqlTransaction tr = null;

        public Database()
        {
            //
            // TODO: 생성자 논리를 여기에 추가합니다.
            //
        }

        public SqlConnection Connection
        {
            get { return conn; }
        }

        public SqlTransaction Transaction
        {
            get { return tr; }
        }

        public void Connect()
        {
            Connect(ConfigurationManager.AppSettings["ConnectionString"]);
        }
        public void Connect(string connectString)
        {
            if (conn == null) conn = new SqlConnection();
            conn.ConnectionString = connectString;
            conn.Open();
        }

        public void BeginTransaction(string transactionName)
        {
            tr = conn.BeginTransaction(transactionName);
        }
        public void BeginTransaction()
        {
            tr = conn.BeginTransaction();
        }

        public void ClearTransaction()
        {
            tr.Dispose();
            tr = null;
        }

        public void Rollback()
        {
            tr.Rollback();
            tr = null;
        }
        public void Rollback(string transactionName)
        {
            tr.Rollback(transactionName);
        }

        public void Commit()
        {
            tr.Commit();
        }

        public int Execute(string query)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            if (tr != null)
            {
                cmd.Transaction = tr;
            }
            return cmd.ExecuteNonQuery();
        }

        public DataSet GetDataSet(string query)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            if (tr != null)
            {
                cmd.Transaction = tr;
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataRow GetDataRow(string query)
        {
            DataSet ds;
            DataRow row;
            try
            {
                ds = GetDataSet(query);
                row = ds.Tables[0].Rows[0];
            }
            catch (Exception ex)
            {
                return null;
            }
            return row;
        }

        public DataRowCollection GetDataRows(string query)
        {
            DataSet ds;
            DataRowCollection dc;
            try
            {
                ds = GetDataSet(query);
                dc = ds.Tables[0].Rows;
            }
            catch (Exception ex)
            {
                return null;
            }
            return dc;
        }

        public DataTable GetDataTable(string query)
        {
            DataSet ds;
            DataTable dt;
            try
            {
                ds = GetDataSet(query);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
            return dt;
        }

        public SqlDataReader GetDataReader(string query)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            if (tr != null)
            {
                cmd.Transaction = tr;
            }
            return cmd.ExecuteReader();
        }

        public Hashtable GetRow(string query)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            if (tr != null)
            {
                cmd.Transaction = tr;
            }
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Hashtable ht = new Hashtable();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    ht.Add(dr.GetName(i), dr[i] ?? "");
                }
                dr.Close();
                return ht;
            }
            dr.Close();
            return null;

        }

        public int GetOneInt(string query)
        {
            try
            {
                return (int)GetOne(query);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string GetOneString(string query)
        {
            try
            {
                return (string)GetOne(query);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public object GetOne(string query)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            if (tr != null)
            {
                cmd.Transaction = tr;
            }
            return cmd.ExecuteScalar();
        }

        public string GetCurrentIdent(string table)
        {
            return ((decimal)GetOne("SELECT IDENT_CURRENT('" + table + "')")).ToString();
        }

        public void Close()
        {
            conn.Close();
        }
    }

}
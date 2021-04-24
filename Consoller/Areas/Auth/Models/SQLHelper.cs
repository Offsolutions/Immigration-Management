using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
public class SQLHelper
{
    SqlConnection con;
    SqlCommand com;
    SqlDataAdapter da;
    private SqlCommand cmd;
    private SqlConnection _connection;
    private SqlCommand _command;
    private SqlDataAdapter _adapter;
    private SqlDataReader _reader;
    private SqlDataReader DataReader;
    private DataSet ds;
    SqlParameter _sqlParameter = new SqlParameter();

    #region DB Connection
    private void OpenConnection()
    {
        if (_connection == null)
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        }
        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
        }
        _command = new SqlCommand();
        _command.Connection = _connection;
    } 
    #endregion

    #region connection closed
    private void CloseConnection()
    {
        SqlConnection.ClearPool(_connection);
        SqlConnection.ClearAllPools();

        if (_connection.State == ConnectionState.Open)
            _connection.Close();
    } 
    #endregion

    #region Constructor
    public SQLHelper()
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        con = new SqlConnection(constr);
        com = new SqlCommand();
        com.Connection = con;
        da = new SqlDataAdapter();
        da.SelectCommand = com;
    }

    public SQLHelper(string ConStr)
    {
        con = new SqlConnection(ConStr);
        com = new SqlCommand();
        com.Connection = con;
        da = new SqlDataAdapter();
        da.SelectCommand = com;
    }
    #endregion

    #region GetDataset
    public DataSet GetDataset(string qry)
    {
        com.CommandText = qry;
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }

    public DataSet GetDataset(SqlCommand com)
    {
        com.Connection = this.con;
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    #endregion

    #region GetTable
    public DataTable GetTable(string qry)
    {
        // try
        //  {
        com.CommandText = qry;
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
        //  }
        //  catch(Exception ex)
        //  {
        //      System.Web.HttpContext.Current.Response.Write(qry);

        //       System.Web.HttpContext.Current.Response.End();
        //      return new DataTable ();

        //  }
    }
    public DataTable GetTable(SqlCommand com)
    {
        com.Connection = this.con;
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }
    #endregion

    #region GetSingleValue
    public object GetSingleValue(string qry)
    {
        con.Close();
        com.CommandText = qry;
        con.Open();
        object obj = com.ExecuteScalar();
        con.Close();
        return obj;
    }
    public object GetSingleValue(SqlCommand com)
    {
        com.Connection = this.con;
        con.Open();
        object obj = com.ExecuteScalar();
        con.Close();
        return obj;
    }
    #endregion

    #region ExecuteNonQuery
    public void ExecuteNonQuery(string qry)
    {
        //try
        //{
        con.Close();
        com.CommandText = qry;
        con.Open();
        com.ExecuteNonQuery();
        con.Close();
        // }
        // catch (Exception ex)
        // {
        //    System.Web.HttpContext.Current.Response.Write(qry);
        //     System.Web.HttpContext.Current.Response.End();

        //  }
    }

    public int ExecuteNonQuery1(string qry)
    {
        con.Close();
        com.CommandText = qry;
        con.Open();
        return com.ExecuteNonQuery();


    }
    public void ExecuteNonQuery(SqlCommand com)
    {
        com.Connection = this.con;
        con.Open();
        com.ExecuteNonQuery();
        con.Close();
    }
    #endregion

    public int Return_Parameter_NonExecuteQuery(string _procedureName, SqlParameter[] _Parameters, SqlParameter[] _Outputparameter)
    {
        this.OpenConnection();
        SqlParameter _sqlParameter = new SqlParameter();
        SqlParameter _sqlOutputParameter = new SqlParameter();
        _command.CommandType = CommandType.StoredProcedure;
        _command.CommandText = _procedureName;
        if (_command.Parameters.Count > 0)
            _command.Parameters.Clear();
        foreach (SqlParameter LoopVar_Param in _Parameters)
        {
            _sqlParameter = LoopVar_Param;
            _command.Parameters.Add(_sqlParameter);
        }
        foreach (SqlParameter LoopVar_Param in _Outputparameter)
        {
            _sqlOutputParameter = LoopVar_Param;
            _command.Parameters.Add(_sqlOutputParameter);
            _command.Parameters[_sqlOutputParameter.ParameterName].Direction = ParameterDirection.Output;
        }
        _command.ExecuteNonQuery();
        int returnresult = Convert.ToInt32(_command.Parameters[_sqlOutputParameter.ParameterName].Value);
        return returnresult;
    }
    public int ExecuteNonQueryByProc(string _procedureName, SqlParameter[] _Parameters)
    {
        this.OpenConnection();
        SqlParameter _sqlParameter = new SqlParameter();
        _command.CommandType = CommandType.StoredProcedure;
        _command.CommandText = _procedureName;
        if (_command.Parameters.Count > 0)
            _command.Parameters.Clear();
        foreach (SqlParameter LoopVar_Param in _Parameters)
        {
            _sqlParameter = LoopVar_Param;
            _command.Parameters.Add(_sqlParameter);
        }
        int i = _command.ExecuteNonQuery();
        CloseConnection();
        return i;
    }
    public int ExecuteScalerByQuery(string _query)
    {
        try
        {
            this.OpenConnection();
            _command.CommandText = _query;
            int i = (int)_command.ExecuteScalar();
            CloseConnection();
            return i;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    public DataSet Get_DataSet(string strQry)
    {
        _adapter = new SqlDataAdapter(strQry, con);
        ds = new DataSet();
        _adapter.Fill(ds);
        return ds;
    }
    public void BindDropDownList(string Query, string Display, string Value, DropDownList Combo)
    {
        con.Open();
        cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = Query;
        cmd.Connection = con;
        DataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        cmd.Dispose();
        cmd = null;

        Combo.DataSource = DataReader;
        Combo.DataTextField = Display;
        Combo.DataValueField = Value;
        Combo.DataBind();
        Combo.Items.Insert(0, "SELECT");
        Combo.Items[0].Value = "0";
    }
}

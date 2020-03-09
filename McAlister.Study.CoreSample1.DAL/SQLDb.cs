using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using df=McAlister.Study.CoreSample1.Definitions;
using Microsoft.Data.SqlClient;

namespace McAlister.Study.CoreSample1.DAL
{
    public class SQLDb
    {
        private String _conStr;

        public SQLDb(String conStr)
        {
            _conStr = conStr;
        }

        private SqlConnection myCon;

        public SqlConnection GetSqlConnection()
        {
            if (myCon == null)
            {
                myCon = GetNewSqlConnection();
            }

            if (myCon != null)
            {
                if (myCon.State != ConnectionState.Open || myCon.State == ConnectionState.Broken)
                {
                    myCon = GetNewSqlConnection();
                }

            }

            SqlCommand myCom = new SqlCommand();
            myCom.Connection = myCon;
            myCom.CommandText = "select 1";

            SqlDataAdapter myDA = new SqlDataAdapter(myCom);
            DataTable oraDT = new DataTable();

            myDA.Fill(oraDT);
            oraDT.Dispose();
            myCom.Dispose();
            myDA.Dispose();

            //myCon.ConnectionTimeout = 200;
            return myCon;
        }

        public SqlConnection GetNewSqlConnection(bool useAffectedRows = false)
        {
            var newCon = new SqlConnection();
            newCon.ConnectionString = _conStr;
            newCon.Open();

            return newCon;
        }

        public SqlCommand GetSqlCommand()
        {
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = GetSqlConnection();

            return myCom;
        }

        public SqlCommand GetSqlCommand(SqlConnection myCon)
        {
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = myCon;

            return myCom;
        }


        // get my sql transaction
        public SqlTransaction GetTransaction(SqlConnection myCon)
        { 
            SqlTransaction myTrans = null;
            if (myCon == null)
                myCon = GetSqlConnection();
            myTrans = myCon.BeginTransaction();

            return myTrans;
        }

        public SqlDataReader GetDataUsingReader(string SQL)
        {
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = GetSqlConnection();
            return GetDataUsingReader(SQL, myCom);
        }

        public SqlDataReader GetDataUsingReader(string SQL, SqlCommand myCom)
        {
            myCom.CommandText = SQL;
            SqlDataReader myRdr = myCom.ExecuteReader();
            return myRdr;
        }

        public DataTable GetData(string SQL)
        {
            SqlCommand command = null;
            return GetData(SQL, command);
        }

        public DataTable GetData(string SQL, string Tablename)
        {
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = GetSqlConnection();
            myCom.CommandText = SQL;
            myCom.CommandTimeout = 0;
            SqlDataAdapter myDA = new SqlDataAdapter(myCom);
            DataTable myDT = new DataTable(Tablename);

            myDA.Fill(myDT);
            myCom.Dispose();
            myDA = null;

            return myDT;
        }

        public DataTable GetData(string SQL, SqlCommand myCom)
        {
            return GetData(SQL, myCom, false);
        }


        public DataTable GetData(string SQL, SqlTransaction SqlTrans)
        {

            SqlCommand myCom = new SqlCommand(SQL, SqlTrans.Connection, SqlTrans);
            return GetData(SQL, myCom, false);
        }

        public DataTable GetData(string SQL, SqlCommand myCom, bool doNotClearParams)
        {
            if (myCom == null)
            {
                myCom = new SqlCommand();
                myCom.Connection = GetSqlConnection();
            }

            if (doNotClearParams == false)
            {
                myCom.Parameters.Clear();
            }

            myCom.CommandText = SQL;
            myCom.CommandTimeout = 0;
            SqlDataAdapter myDA = new SqlDataAdapter(myCom);
            DataTable myDT = new DataTable();

            myDA.Fill(myDT);
            myCom.Dispose();
            myDA.Dispose();

            return myDT;
        }

        /// <summary>
        ///vis_applicant_library\Enrollment.cs line 33
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>

        public object GetScalar(string SQL, SqlCommand myCom = null)
        {
            bool localCommand = false;

            if (myCom == null)
            {
                myCom = GetNewSqlConnection().CreateCommand();
                localCommand = true;
            }

            myCom.Parameters.Clear();
            myCom.CommandText = SQL;

            object value = myCom.ExecuteScalar();

            if (localCommand)
            {
                myCom.Connection.Close();
                myCom.Dispose();
            }

            return value;
        }

        //
        public bool ExecuteQuery(List<String> SQLStatements)
        {

            SqlCommand myCom = new SqlCommand();
            myCom.Connection = GetSqlConnection();

            SqlTransaction transaction = myCom.Connection.BeginTransaction();

            try
            {
                foreach (string sql in SQLStatements)
                {
                    myCom.CommandText = sql;
                    myCom.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch (System.Exception e)
            {

                transaction.Rollback();
                throw e;
            }
        }

        /// <summary>
        /// vis_interface\MyInfo_Claim_Changes line 1035
        /// </summary>
        /// <param name="SQLStatements"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public SqlTransaction ExecuteQuery_Without_Commit(List<String> SQLStatements)
        {

            SqlCommand myCom = new SqlCommand();
            myCom.Connection = GetNewSqlConnection();

            SqlTransaction transaction = myCom.Connection.BeginTransaction();

            try
            {
                foreach (string sql in SQLStatements)
                {
                    myCom.CommandText = sql;
                    myCom.ExecuteNonQuery();
                }

                return transaction;
            }

            catch (System.Exception e)
            {

                transaction.Rollback();

                throw e;
            }
        }

        //
        public void ExecuteQuery(string SQL)
        {
            ExecuteQuery(SQL, null);
        }

        public int ExecuteQuery(string SQL, SqlCommand myCom)
        {
            if (myCom == null)
            {
                myCom = new SqlCommand();
                myCom.Connection = GetSqlConnection();
            }

            myCom.CommandText = SQL;
            return myCom.ExecuteNonQuery();
        }
    }
}

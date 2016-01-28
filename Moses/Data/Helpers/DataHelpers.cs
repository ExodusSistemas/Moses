using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Linq;
using System.Data.Common;
using System.Linq.Expressions;


namespace Moses.Data
{
    public static class DataFactoryHelper
    {

        #region TransactionHelpers

        public static SqlTransaction BeginTransaction(this DataContext context, string defaultConnectionString)
        {
            var conn = new SqlConnection(defaultConnectionString);
            conn.Open();
            return conn.BeginTransaction();
        }

        private static void OpenTransaction(this DataContext context)
        {

            if (context.Transaction == null)
            {
                if (context.Connection.State == ConnectionState.Closed)
                    context.Connection.Open();
            }

            if (context.Transaction == null)
            {
                if (context.Connection.State == ConnectionState.Closed)
                    context.Connection.Open();
            }

        }

        public static void CloseConnection(this DataContext context)
        {
            if (context.Connection.State == ConnectionState.Open) context.Connection.Close();
        }

        #endregion

        #region SpCommands

        /// <summary>
        /// Executa uma SP com parâmetros
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteSP(this DataContext context, string spName, params SqlParameter[] parameters)
        {
            SqlCommand cmd = CreateSpCommand(context, spName);
            foreach (var q in parameters)
                cmd.Parameters.Add(q);


            return cmd.ExecuteNonQuery();
        }

        public static SqlDataReader ExecuteReaderSP(this DataContext context, string spName)
        {
            SqlDataReader rdr = null;
            SqlCommand cmd = CreateSpCommand(context, spName);
            return rdr = cmd.ExecuteReader();
        }

        // run a stored procedure that takes a parameter
        public static DbDataReader ExecuteReaderSP(this DataContext context, string spName, params SqlParameter[] parameters)
        {
            DbDataReader rdr = null;
            SqlCommand cmd = CreateSpCommand(context, spName);
            
            //Isso é para inibir os paus de tempo limite excedido (http://stackoverflow.com/questions/10957272/call-stored-procedure-with-long-running-time)
            //cmd.CommandTimeout = 20000;
            cmd.CommandTimeout = context.CommandTimeout;

            foreach (var q in parameters) cmd.Parameters.Add(q);

            // execute the command
            return rdr = cmd.ExecuteReader();
        }

        /// <summary>
        /// Roda uma Stored Procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn">Conexão para executar a sp, não necessita estar aberta</param>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T ExecuteScalarSP<T>(this DataContext context, string spName, params SqlParameter[] parameters)
        {
            T output = default(T);
            SqlCommand cmd = CreateSpCommand(context, spName);

            foreach (var q in parameters)
                cmd.Parameters.Add(q);

            // execute the command
            return output = (T)cmd.ExecuteScalar();
        }

        /// <summary>
        /// Roda uma Function Sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn">Conexão para executar a sp, não necessita estar aberta</param>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T ExecuteScalarFunction<T>(this DataContext context, string functionName, params SqlParameter[] parameters)
        {
            T output = default(T);

            DbCommand cmd = new SqlCommand(functionName);
            cmd.Connection = context.Connection;
            cmd.CommandType = CommandType.Text;

            StringBuilder paramStrBdr = new StringBuilder();

            foreach (var q in parameters)
            {
                paramStrBdr.Append(q.ParameterName);
                cmd.Parameters.Add(q);
                paramStrBdr.Append(",");
            }
            paramStrBdr.Remove(paramStrBdr.Length - 1, 1);

            cmd.CommandText = string.Format("SELECT {0}({1})", functionName, paramStrBdr.ToString());

            // execute the command
            return output = (T)cmd.ExecuteScalar();
        }

        public static SqlCommand CreateSpCommand(this DataContext context, string spName, CommandType type = CommandType.StoredProcedure, params SqlParameter[] parameters )
        {
            SqlCommand cmd = new SqlCommand(spName, (SqlConnection)context.Connection);
            //Para considerar o timeout
            cmd.CommandTimeout = context.CommandTimeout;
            cmd.CommandType = type;
            if ( parameters != null)
                cmd.Parameters.AddRange(parameters); 
            return cmd;
        }

        public static SqlCommand CreateTransactionSpCommand(this DataContext context, string spName, CommandType type = CommandType.StoredProcedure, params SqlParameter[] parameters )
        {
            SqlCommand cmd = new SqlCommand(spName, (SqlConnection)context.Connection);
            cmd.CommandType = type;
            if (parameters != null)
                cmd.Parameters.AddRange(parameters); 
            return cmd;
        }

        #endregion

        
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                  Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                    Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }

    public static class DbHelper
    {
        //falta incluir os nullables   
        public static string RetrieveString(this DbDataReader reader, int keyId)
        {
            return reader.IsDBNull(keyId) == true ? null : reader.GetString(keyId);
        }

        public static DateTime RetrieveDateTime(this DbDataReader reader, int keyId)
        {
            return reader.IsDBNull(keyId) == true ? new DateTime() : reader.GetDateTime(keyId);
        }

        public static Int64 RetrieveInt64(this DbDataReader reader, int keyId)
        {
            return reader.IsDBNull(keyId) == true ? 0 : reader.GetInt64(keyId);
        }

        public static int RetrieveInt32(this DbDataReader reader, int keyId)
        {
            return reader.IsDBNull(keyId) == true ? 0 : reader.GetInt32(keyId);
        }

        public static short RetrieveInt16(this DbDataReader reader, int keyId)
        {
            return reader.IsDBNull(keyId) == true ? (short)0 : reader.GetInt16(keyId);
        }

        public static decimal RetrieveDecimal(this DbDataReader reader, int keyId)
        {
            return reader.IsDBNull(keyId) == true ? 0 : reader.GetDecimal(keyId);
        }

        public static Guid RetrieveGuid(this DbDataReader reader, int keyId)
        {
            return reader.IsDBNull(keyId) == true ? Guid.Empty : reader.GetGuid(keyId);
        }




    }
}

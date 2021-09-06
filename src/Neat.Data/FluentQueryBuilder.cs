using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Neat.Data
{
    public class FluentQueryBuilder
    {
        private DbQueryMapper _db;

        /// <summary>
        /// Creates a new instance of FluentQueryBuilder
        /// </summary>
        protected internal FluentQueryBuilder(DbQueryMapper db)
        {
            _db = db;
        }

        /// <summary>
        /// Sets the command to be executed.
        /// </summary>
        /// <param name="commandText">query command</param>
        /// <returns></returns>
        public FluentQueryBuilder AssignCommand(string commandText) // CommandType commandType , params object[] parameters
        {
            // The Command will be recreated (CommandText, Parameters, ...)
            _db.Command = _db.CreateCommand(commandText);
            _db.Command.Connection = _db.CreateConnection();
            return this;
        }

        /// <summary>
        /// Add a new parameter to the current query.
        /// </summary>
        /// <typeparam name="T">Type of this parameter</typeparam>
        /// <param name="name">Name of this parameter</param>
        /// <param name="value">Value of this parameter</param>
        /// <returns></returns>
        public FluentQueryBuilder AddParameter<T>(string name, T value)
        {
            _db.AddParameter(name, value);
            return this;
        }

        /// <summary>
        /// Add a new parameter to the current query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">Name of this parameter</param>
        /// <param name="value">Value of this paramater</param>
        /// <param name="type">Type of this parameter</param>
        /// <returns></returns>
        public FluentQueryBuilder AddParameter<T>(string name, T value, DbType type)
        {
            _db.AddParameter(name, value, type);
            return this;
        }

        /// <summary>
        /// Add a new parameter to the current query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">Name of this parameter</param>
        /// <param name="value">Value of this paramater</param>
        /// <param name="type">Type of this parameter</param>
        /// <param name="size">Size of this parameter</param>
        /// <returns></returns>
        public FluentQueryBuilder AddParameter<T>(string name, T value, DbType type, int size)
        {
            _db.AddParameter(name, value, type, size);
            return this;
        }

        /// <summary>
        /// Add a new parameter to the current query.
        /// </summary>
        /// <typeparam name="T">Type of this parameter</typeparam>
        /// <param name="values">Object contains properties to define parameter names and values</param>
        /// <returns></returns>
        public FluentQueryBuilder AddParameter<T>(T values)
        {
            _db.AddParameter(values);
            return this;
        }

        public List<T> ExecuteList<T>() where T : new()
        {
            return _db.ExecuteList<T>();
        }

        public List<T> ExecuteList<T>(Converter<DbDataReader, T> converter) where T : new()
        {
            return _db.ExecuteList<T>(converter);
        }

        public int ExecuteNonQuery()
        {
            return _db.ExecuteNonQuery();
        }

        public T ExecuteObject<T>(Converter<DbDataReader, T> converter) where T : new()
        {
            return _db.ExecuteObject<T>(converter);
        }

        public T ExecuteObject<T>() where T : new()
        {
            return _db.ExecuteObject<T>();
        }

        public T ExecuteScalar<T>()
        {
            return _db.ExecuteScalar<T>();
        }

        ///// <summary>
        ///// Includes an active transaction to the current query.
        ///// </summary>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //public FluentCommander WithTransaction(DbTransaction transaction)
        //{
        //    _dbc.Transaction = transaction;
        //    return this;
        //}
    }
}

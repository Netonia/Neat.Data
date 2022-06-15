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
        /// Warning: The Command will be recreated (CommandText, CommandType, Parameters, ...)
        /// </summary>
        /// <param name="commandText">command text</param>
        /// <param name="commandType">command type</param>
        /// <returns></returns>
        public FluentQueryBuilder AssignCommand(string commandText, CommandType commandType = CommandType.Text) // params object[] parameters
        {
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

        public int ExecuteNonQuery()
        {
            return _db.ExecuteNonQuery();
        }

        public List<T> ToList<T>() where T : new()
        {
            return _db.ExecuteList<T>();
        }

        public List<T> ToList<T>(Converter<DbDataReader, T> converter) where T : new()
        {
            return _db.ExecuteList<T>(converter);
        }

        public T ToObject<T>(Converter<DbDataReader, T> converter) where T : new()
        {
            return _db.ExecuteObject<T>(converter);
        }

        public T ToObject<T>() where T : new()
        {
            return _db.ExecuteObject<T>();
        }

        public T ToScalar<T>()
        {
            return _db.ExecuteScalar<T>();
        }
    }
}

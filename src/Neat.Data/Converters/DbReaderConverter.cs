namespace Neat.Data.Converters
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Reflection;

    /// <summary>
    /// The data reader converter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbReaderConverter<T>
        where T : new()
    {
        #region Private Fields

        /// <summary>
        /// The _last reader.
        /// </summary>
        private DbDataReader _lastReader;

        /// <summary>
        /// The _mappings.
        /// </summary>
        private Mapping[] _mappings;

        #endregion

        #region Public Methods

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The generic type.</returns>
        public T Convert(DbDataReader reader)
        {
            if (this._mappings == null || reader != this._lastReader)
            {
                this._mappings = this.MapProperties(reader);
            }

            var o = new T();

            foreach (var mapping in this._mappings)
            {
                var prop = mapping.Property;
                var rawValue = reader.GetValue(mapping.Index);
                var value = ObjectConverter.To(prop.PropertyType, rawValue);
                prop.SetValue(o, value, null);
            }

            this._lastReader = reader;

            return o;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The map properties.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The <see cref="Mapping"/>.</returns>
        private Mapping[] MapProperties(DbDataReader reader)
        {
            var fieldCount = reader.FieldCount;

            var fields = new Dictionary<string, int>(fieldCount);

            for (var i = 0; i < fieldCount; i++)
            {
                fields.Add(reader.GetName(i).ToLowerInvariant(), i);
            }

            var type = typeof(T);

            var mapping = new List<Mapping>(fieldCount);

            foreach (var prop in type.GetProperties())
            {
                var name = prop.Name.ToLowerInvariant();

                int index;

                if (fields.TryGetValue(name, out index))
                {
                    mapping.Add(new Mapping() { Index = index, Property = prop });
                }
            }

            return mapping.ToArray();
        }

        #endregion

        #region Private Classes

        /// <summary>
        /// The mapping.
        /// </summary>
        private class Mapping
        {
            #region Public Fields

            /// <summary>
            /// The index.
            /// </summary>
            public int Index;

            /// <summary>
            /// The property.
            /// </summary>
            public PropertyInfo Property;

            #endregion
        }

        #endregion
    }
}
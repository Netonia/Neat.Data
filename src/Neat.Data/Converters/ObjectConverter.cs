namespace Neat.Data.Converters
{
    using System;
    using System.Globalization;

    /// <summary>
    /// The object converter.
    /// </summary>
    public class ObjectConverter
    {
        #region Public Methods

        /// <summary>
        /// Converts an objet to a generic type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The generic type.</returns>
        public static T To<T>(object value)
        {
            return To<T>(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts an objet to a generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The generic type.</returns>
        public static T To<T>(object value, IFormatProvider provider)
        {
            if (value is T)
            {
                return (T)value;
            }

            if (value == null || value == DBNull.Value)
            {
                return default(T);
            }

            return (T)To(typeof(T), value, provider);
        }

        /// <summary>
        /// Converts an objet to a generic type.
        /// </summary>
        /// <param name="type"> The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object To(Type type, object value)
        {
            return To(type, value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts an objet to a generic type.
        /// </summary>
        /// <param name="type">    The type.</param>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="object"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static object To(Type type, object value, IFormatProvider provider)
        {
            if (type == null)
            {
                throw new ArgumentNullException("targetType");
            }

            var isNullable = IsNullable(type);

            if (isNullable)
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (value == null || value == DBNull.Value)
            {
                if (isNullable || !type.IsValueType)
                {
                    return null;
                }
                else
                {
                    return Activator.CreateInstance(type);
                }
            }

            var typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return ToBooleanInternal(value, provider);

                case TypeCode.SByte:
                    return ToSByteInternal(value, provider);

                case TypeCode.Int16:
                    return ToInt16Internal(value, provider);

                case TypeCode.Int32:
                    return ToInt32Internal(value, provider);

                case TypeCode.Int64:
                    return ToInt64Internal(value, provider);

                case TypeCode.Byte:
                    return ToByteInternal(value, provider);

                case TypeCode.UInt16:
                    return ToUInt16Internal(value, provider);

                case TypeCode.UInt32:
                    return ToUInt32Internal(value, provider);

                case TypeCode.UInt64:
                    return ToUInt64Internal(value, provider);

                case TypeCode.Decimal:
                    return ToDecimalInternal(value, provider);

                case TypeCode.Single:
                    return ToSingleInternal(value, provider);

                case TypeCode.Double:
                    return ToDoubleInternal(value, provider);

                case TypeCode.Char:
                    return ToCharInternal(value, provider);

                case TypeCode.DateTime:
                    return ToDateTimeInternal(value, provider);

                case TypeCode.String:
                    return ToStringInternal(value, provider);

                case TypeCode.Object:
                    if (type == typeof(Guid))
                    {
                        return ToGuidInternal(value, null);
                    }

                    break;
            }

            // fallback to System.Convert for IConvertible types
            return Convert.ChangeType(value, typeCode, provider);
        }

        /// <summary>
        /// Returns a boolean.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool ToBoolean(object value, IFormatProvider provider)
        {
            return To<bool>(value, provider, ToBooleanInternal);
        }

        /// <summary>
        /// Returns a boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool ToBoolean(object value)
        {
            return To<bool>(value, CultureInfo.CurrentCulture, ToBooleanInternal);
        }

        /// <summary>
        /// Returns a byte.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="byte"/>.</returns>
        public static byte ToByte(object value, IFormatProvider provider)
        {
            return To<byte>(value, provider, ToByteInternal);
        }

        /// <summary>
        /// Returns a byte.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="byte"/>.</returns>
        public static byte ToByte(object value)
        {
            return To<byte>(value, CultureInfo.CurrentCulture, ToByteInternal);
        }

        /// <summary>
        /// Returns a char.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="char"/>.</returns>
        public static char ToChar(object value, IFormatProvider provider)
        {
            return To<char>(value, provider, ToCharInternal);
        }

        /// <summary>
        /// Returns a char.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="char"/>.</returns>
        public static char ToChar(object value)
        {
            return To<char>(value, CultureInfo.CurrentCulture, ToCharInternal);
        }

        /// <summary>
        /// Returns a date time.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime ToDateTime(object value, IFormatProvider provider)
        {
            return To<DateTime>(value, provider, ToDateTimeInternal);
        }

        /// <summary>
        /// Returns a date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime ToDateTime(object value)
        {
            return To<DateTime>(value, CultureInfo.CurrentCulture, ToDateTimeInternal);
        }

        /// <summary>
        /// Returns a db value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object ToDBValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            return value;
        }

        /// <summary>
        /// Returns a decimal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="decimal"/>.</returns>
        public static decimal ToDecimal(object value, IFormatProvider provider)
        {
            return To<decimal>(value, provider, ToDecimalInternal);
        }

        /// <summary>
        /// Returns a decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="decimal"/>.</returns>
        public static decimal ToDecimal(object value)
        {
            return To<decimal>(value, CultureInfo.CurrentCulture, ToDecimalInternal);
        }

        /// <summary>
        /// Returns a double.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="double"/>.</returns>
        public static double ToDouble(object value, IFormatProvider provider)
        {
            return To<double>(value, provider, ToDoubleInternal);
        }

        /// <summary>
        /// Returns a double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="double"/>.</returns>
        public static double ToDouble(object value)
        {
            return To<double>(value, CultureInfo.CurrentCulture, ToDoubleInternal);
        }

        /// <summary>
        /// Returns a guid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="Guid"/>.</returns>
        public static Guid ToGuid(object value)
        {
            return To<Guid>(value, null, ToGuidInternal);
        }

        /// <summary>
        /// Returns a int 16.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="short"/>.</returns>
        public static short ToInt16(object value, IFormatProvider provider)
        {
            return To<short>(value, provider, ToInt16Internal);
        }

        /// <summary>
        /// Returns a int 16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="short"/>.</returns>
        public static short ToInt16(object value)
        {
            return To<short>(value, CultureInfo.CurrentCulture, ToInt16Internal);
        }

        /// <summary>
        /// Returns a int 32.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int ToInt32(object value, IFormatProvider provider)
        {
            return To<int>(value, provider, ToInt32Internal);
        }

        /// <summary>
        /// Returns a int 32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int ToInt32(object value)
        {
            return To<int>(value, CultureInfo.CurrentCulture, ToInt32Internal);
        }

        /// <summary>
        /// Returns a int 64.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="long"/>.</returns>
        public static long ToInt64(object value, IFormatProvider provider)
        {
            return To<long>(value, provider, ToInt64Internal);
        }

        /// <summary>
        /// Returns a int 64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="long"/>.</returns>
        public static long ToInt64(object value)
        {
            return To<long>(value, CultureInfo.CurrentCulture, ToInt64Internal);
        }

        /// <summary>
        /// Returns a nullable boolean.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable boolean.</returns>
        public static bool? ToNullableBoolean(object value, IFormatProvider provider)
        {
            return ToNullable<bool>(value, provider, ToBooleanInternal);
        }

        /// <summary>
        /// Returns a nullable boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable boolean.</returns>
        public static bool? ToNullableBoolean(object value)
        {
            return ToNullable<bool>(value, CultureInfo.CurrentCulture, ToBooleanInternal);
        }

        /// <summary>
        /// Returns a nullable byte.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable byte.</returns>
        public static byte? ToNullableByte(object value, IFormatProvider provider)
        {
            return ToNullable<byte>(value, provider, ToByteInternal);
        }

        /// <summary>
        /// Returns a nullable byte.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable byte.</returns>
        public static byte? ToNullableByte(object value)
        {
            return ToNullable<byte>(value, CultureInfo.CurrentCulture, ToByteInternal);
        }

        /// <summary>
        /// Returns a nullable char.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable char.</returns>
        public static char? ToNullableChar(object value, IFormatProvider provider)
        {
            return ToNullable<char>(value, provider, ToCharInternal);
        }

        /// <summary>
        /// Returns a nullable char.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable char.</returns>
        public static char? ToNullableChar(object value)
        {
            return ToNullable<char>(value, CultureInfo.CurrentCulture, ToCharInternal);
        }

        /// <summary>
        /// Returns a nullable date time.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable DateTime.</returns>
        public static DateTime? ToNullableDateTime(object value, IFormatProvider provider)
        {
            return ToNullable<DateTime>(value, provider, ToDateTimeInternal);
        }

        /// <summary>
        /// Returns a nullable date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable DateTime.</returns>
        public static DateTime? ToNullableDateTime(object value)
        {
            return ToNullable<DateTime>(value, CultureInfo.CurrentCulture, ToDateTimeInternal);
        }

        /// <summary>
        /// Returns a nullable decimal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable decimal.</returns>
        public static decimal? ToNullableDecimal(object value, IFormatProvider provider)
        {
            return ToNullable<decimal>(value, provider, ToDecimalInternal);
        }

        /// <summary>
        /// Returns a nullable decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable decimal.</returns>
        public static decimal? ToNullableDecimal(object value)
        {
            return ToNullable<decimal>(value, CultureInfo.CurrentCulture, ToDecimalInternal);
        }

        /// <summary>
        /// Returns a nullable double.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable double.</returns>
        public static double? ToNullableDouble(object value, IFormatProvider provider)
        {
            return ToNullable<double>(value, provider, ToDoubleInternal);
        }

        /// <summary>
        /// Returns a nullable double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable double.</returns>
        public static double? ToNullableDouble(object value)
        {
            return ToNullable<double>(value, CultureInfo.CurrentCulture, ToDoubleInternal);
        }

        /// <summary>
        /// Returns a nullable guid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable Guid.</returns>
        public static Guid? ToNullableGuid(object value)
        {
            return ToNullable<Guid>(value, null, ToGuidInternal);
        }

        /// <summary>
        /// Returns a nullable int 16.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable short.</returns>
        public static short? ToNullableInt16(object value, IFormatProvider provider)
        {
            return ToNullable<short>(value, provider, ToInt16Internal);
        }

        /// <summary>
        /// Returns a nullable int 16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable short.</returns>
        public static short? ToNullableInt16(object value)
        {
            return ToNullable<short>(value, CultureInfo.CurrentCulture, ToInt16Internal);
        }

        /// <summary>
        /// Returns a nullable int 32.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable int.</returns>
        public static int? ToNullableInt32(object value, IFormatProvider provider)
        {
            return ToNullable<int>(value, provider, ToInt32Internal);
        }

        /// <summary>
        /// Returns a nullable int 32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable int.</returns>
        public static int? ToNullableInt32(object value)
        {
            return ToNullable<int>(value, CultureInfo.CurrentCulture, ToInt32Internal);
        }

        /// <summary>
        /// Returns a nullable int 64.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable long.</returns>
        public static long? ToNullableInt64(object value, IFormatProvider provider)
        {
            return ToNullable<long>(value, provider, ToInt64Internal);
        }

        /// <summary>
        /// Returns a nullable int 64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable long.</returns>
        public static long? ToNullableInt64(object value)
        {
            return ToNullable<long>(value, CultureInfo.CurrentCulture, ToInt64Internal);
        }

        /// <summary>
        /// Returns a nullable s byte.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable sbyte.</returns>
        public static sbyte? ToNullableSByte(object value, IFormatProvider provider)
        {
            return ToNullable<sbyte>(value, provider, ToSByteInternal);
        }

        /// <summary>
        /// Returns a nullable s byte.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable sbyte.</returns>
        public static sbyte? ToNullableSByte(object value)
        {
            return ToNullable<sbyte>(value, CultureInfo.CurrentCulture, ToSByteInternal);
        }

        /// <summary>
        /// Returns a nullable single.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable float.</returns>
        public static float? ToNullableSingle(object value, IFormatProvider provider)
        {
            return ToNullable<float>(value, provider, ToSingleInternal);
        }

        /// <summary>
        /// Returns a nullable single.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable float.</returns>
        public static float? ToNullableSingle(object value)
        {
            return ToNullable<float>(value, CultureInfo.CurrentCulture, ToSingleInternal);
        }

        /// <summary>
        /// Returns a nullable u int 16.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable ushort.</returns>
        public static ushort? ToNullableUInt16(object value, IFormatProvider provider)
        {
            return ToNullable<ushort>(value, provider, ToUInt16Internal);
        }

        /// <summary>
        /// Returns a nullable u int 16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable ushort.</returns>
        public static ushort? ToNullableUInt16(object value)
        {
            return ToNullable<ushort>(value, CultureInfo.CurrentCulture, ToUInt16Internal);
        }

        /// <summary>
        /// Returns a nullable u int 32.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable uint.</returns>
        public static uint? ToNullableUInt32(object value, IFormatProvider provider)
        {
            return ToNullable<uint>(value, provider, ToUInt32Internal);
        }

        /// <summary>
        /// Returns a nullable u int 32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable uint.</returns>
        public static uint? ToNullableUInt32(object value)
        {
            return ToNullable<uint>(value, CultureInfo.CurrentCulture, ToUInt32Internal);
        }

        /// <summary>
        /// Returns a nullable u int 64.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The nullable ulong.</returns>
        public static ulong? ToNullableUInt64(object value, IFormatProvider provider)
        {
            return ToNullable<ulong>(value, provider, ToUInt64Internal);
        }

        /// <summary>
        /// Returns a nullable u int 64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The nullable ulong.</returns>
        public static ulong? ToNullableUInt64(object value)
        {
            return ToNullable<ulong>(value, CultureInfo.CurrentCulture, ToUInt64Internal);
        }

        /// <summary>
        /// Returns a s byte.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="sbyte"/>.</returns>
        public static sbyte ToSByte(object value, IFormatProvider provider)
        {
            return To<sbyte>(value, provider, ToSByteInternal);
        }

        /// <summary>
        /// Returns a s byte.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="sbyte"/>.</returns>
        public static sbyte ToSByte(object value)
        {
            return To<sbyte>(value, CultureInfo.CurrentCulture, ToSByteInternal);
        }

        /// <summary>
        /// Returns a single.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="float"/>.</returns>
        public static float ToSingle(object value, IFormatProvider provider)
        {
            return To<float>(value, provider, ToSingleInternal);
        }

        /// <summary>
        /// Returns a single.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="float"/>.</returns>
        public static float ToSingle(object value)
        {
            return To<float>(value, CultureInfo.CurrentCulture, ToSingleInternal);
        }

        /// <summary>
        /// Returns a string.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToString(object value, IFormatProvider provider)
        {
            return To<string>(value, provider, ToStringInternal);
        }

        /// <summary>
        /// Returns a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToString(object value)
        {
            return To<string>(value, CultureInfo.CurrentCulture, ToStringInternal);
        }

        /// <summary>
        /// Returns a string internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToStringInternal(object value, IFormatProvider provider)
        {
            return Convert.ToString(value, provider);
        }

        /// <summary>
        /// Returns a u int 16.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="ushort"/>.</returns>
        public static ushort ToUInt16(object value, IFormatProvider provider)
        {
            return To<ushort>(value, provider, ToUInt16Internal);
        }

        /// <summary>
        /// Returns a u int 16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="ushort"/>.</returns>
        public static ushort ToUInt16(object value)
        {
            return To<ushort>(value, CultureInfo.CurrentCulture, ToUInt16Internal);
        }

        /// <summary>
        /// Returns a u int 32.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint ToUInt32(object value, IFormatProvider provider)
        {
            return To<uint>(value, provider, ToUInt32Internal);
        }

        /// <summary>
        /// Returns a u int 32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        public static uint ToUInt32(object value)
        {
            return To<uint>(value, CultureInfo.CurrentCulture, ToUInt32Internal);
        }

        /// <summary>
        /// Returns a u int 64.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static ulong ToUInt64(object value, IFormatProvider provider)
        {
            return To<ulong>(value, provider, ToUInt64Internal);
        }

        /// <summary>
        /// Returns a u int 64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        public static ulong ToUInt64(object value)
        {
            return To<ulong>(value, CultureInfo.CurrentCulture, ToUInt64Internal);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The is nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Returns a.
        /// </summary>
        /// <param name="value">    The value.</param>
        /// <param name="provider"> The provider.</param>
        /// <param name="converter">The converter.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The generic type.</returns>
        private static T To<T>(object value, IFormatProvider provider, Func<object, IFormatProvider, T> converter)
        {
            if (value is T)
            {
                return (T)value;
            }

            if (value == null || value == DBNull.Value)
            {
                return default(T);
            }

            return converter(value, provider);
        }

        /// <summary>
        /// Returns a boolean internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool ToBooleanInternal(object value, IFormatProvider provider)
        {
            string s = value as string;
            bool b;

            if (s != null && bool.TryParse(s, out b))
            {
                return b;
            }

            if ((s = value.ToString()) != null)
            {
                int i;

                if (int.TryParse(s, out i))
                {
                    return i != 0;
                }
            }

            return Convert.ToBoolean(value, provider);
        }

        /// <summary>
        /// Returns a byte internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="byte"/>.</returns>
        private static byte ToByteInternal(object value, IFormatProvider provider)
        {
            return Convert.ToByte(value, provider);
        }

        /// <summary>
        /// Returns a char internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="char"/>.</returns>
        private static char ToCharInternal(object value, IFormatProvider provider)
        {
            var s = value as string;

            if (!string.IsNullOrEmpty(s))
            {
                return s[0];
            }

            return Convert.ToChar(value, provider);
        }

        /// <summary>
        /// Returns a date time internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        private static DateTime ToDateTimeInternal(object value, IFormatProvider provider)
        {
            return Convert.ToDateTime(value, provider);
        }

        /// <summary>
        /// Returns a decimal internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="decimal"/>.</returns>
        private static decimal ToDecimalInternal(object value, IFormatProvider provider)
        {
            return Convert.ToDecimal(value, provider);
        }

        /// <summary>
        /// Returns a double internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="double"/>.</returns>
        private static double ToDoubleInternal(object value, IFormatProvider provider)
        {
            return Convert.ToDouble(value, provider);
        }

        // This method accepts IFormatProvider just to match the signature required by To<T> and
        // ToNullable<T>, but it is not used
        /// <summary>
        /// Returns a guid internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="Guid"/>.</returns>
        private static Guid ToGuidInternal(object value, IFormatProvider provider)
        {
            byte[] bytes = value as byte[];

            if (bytes != null)
            {
                return new Guid(bytes);
            }

            return new Guid(value.ToString());
        }

        /// <summary>
        /// Returns a int 16 internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="short"/>.</returns>
        private static short ToInt16Internal(object value, IFormatProvider provider)
        {
            return Convert.ToInt16(value, provider);
        }

        /// <summary>
        /// Returns a int 32 internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="int"/>.</returns>
        private static int ToInt32Internal(object value, IFormatProvider provider)
        {
            return Convert.ToInt32(value, provider);
        }

        /// <summary>
        /// Returns a int 64 internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="long"/>.</returns>
        private static long ToInt64Internal(object value, IFormatProvider provider)
        {
            return Convert.ToInt64(value, provider);
        }

        /// <summary>
        /// Returns a nullable.
        /// </summary>
        /// <param name="value">    The value.</param>
        /// <param name="provider"> The provider.</param>
        /// <param name="converter">The converter.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The nullable generic type.</returns>
        private static T? ToNullable<T>(
            object value,
            IFormatProvider provider,
            Func<object, IFormatProvider, T> converter) where T : struct
        {
            if (value is T)
            {
                return (T)value;
            }

            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return converter(value, provider);
        }

        /// <summary>
        /// Returns a s byte internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="sbyte"/>.</returns>
        private static sbyte ToSByteInternal(object value, IFormatProvider provider)
        {
            return Convert.ToSByte(value, provider);
        }

        /// <summary>
        /// Returns a single internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="float"/>.</returns>
        private static float ToSingleInternal(object value, IFormatProvider provider)
        {
            return Convert.ToSingle(value, provider);
        }

        /// <summary>
        /// Returns a u int 16 internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="ushort"/>.</returns>
        private static ushort ToUInt16Internal(object value, IFormatProvider provider)
        {
            return Convert.ToUInt16(value, provider);
        }

        /// <summary>
        /// Returns a u int 32 internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="uint"/>.</returns>
        private static uint ToUInt32Internal(object value, IFormatProvider provider)
        {
            return Convert.ToUInt32(value, provider);
        }

        /// <summary>
        /// Returns a u int 64 internal.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        private static ulong ToUInt64Internal(object value, IFormatProvider provider)
        {
            return Convert.ToUInt64(value, provider);
        }

        #endregion
    }
}
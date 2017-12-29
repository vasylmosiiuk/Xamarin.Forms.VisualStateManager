//
// TypeConversionExtensions.cs
//
// Author:
//       Stephane Delcroix <stephane@mi8.be>
//
// Copyright (c) 2013 Mobile Inception
// Copyright (c) 2014 Xamarin, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml.Internals;

namespace Forms.VisualStateManager.Helpers
{
    internal static class TypeConversionExtensions
    {
        internal static string[] TypeConvertersType =
        {
            "Xamarin.Forms.TypeConverterAttribute",
            "System.ComponentModel.TypeConverterAttribute"
        };

        internal static object ConvertTo(this object value, Type toType, IServiceProvider serviceProvider)
        {
            object GetConverter()
            {
                var converterTypeName = toType.GetTypeInfo().CustomAttributes.GetTypeConverterTypeName();
                if (converterTypeName == null)
                    return null;

                var convertertype = Type.GetType(converterTypeName);
                return Activator.CreateInstance(convertertype);
            }

            return ConvertTo(value, toType, GetConverter, serviceProvider);
        }

        private static string GetTypeConverterTypeName(this IEnumerable<CustomAttributeData> attributes)
        {
            var converterAttribute =
                attributes.FirstOrDefault(cad => TypeConvertersType.Contains(cad.AttributeType.FullName));
            if (converterAttribute == null)
                return null;
            if (converterAttribute.ConstructorArguments[0].ArgumentType == typeof(string))
                return (string) converterAttribute.ConstructorArguments[0].Value;
            if (converterAttribute.ConstructorArguments[0].ArgumentType == typeof(Type))
                return ((Type) converterAttribute.ConstructorArguments[0].Value).AssemblyQualifiedName;
            return null;
        }

        internal static object ConvertTo(this object value, Type toType, Func<object> getConverter,
            IServiceProvider serviceProvider)
        {
            if (value == null)
                return null;

            if (value is string str)
            {
                //If there's a [TypeConverter], use it
                var converter = getConverter?.Invoke();
                var xfTypeConverter = converter as TypeConverter;
                if (xfTypeConverter is IExtendedTypeConverter xfExtendedTypeConverter)
                    return xfExtendedTypeConverter.ConvertFromInvariantString(str, serviceProvider);
                if (xfTypeConverter != null)
                    return xfTypeConverter.ConvertFromInvariantString(str);
                var converterType = converter?.GetType();
                var convertFromStringInvariant = converterType?.GetRuntimeMethod("ConvertFromInvariantString",
                    new[] {typeof(string)});
                if (convertFromStringInvariant != null)
                    return convertFromStringInvariant.Invoke(converter, new object[] {str});

                //If the type is nullable, as the value is not null, it's safe to assume we want the built-in conversion
                if (toType.GetTypeInfo().IsGenericType && toType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    toType = Nullable.GetUnderlyingType(toType);

                //Obvious Built-in conversions
                if (toType.GetTypeInfo().IsEnum)
                    return Enum.Parse(toType, str, false);
                if (toType == typeof(sbyte))
                    return sbyte.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(short))
                    return short.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(int))
                    return int.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(long))
                    return long.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(byte))
                    return byte.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(ushort))
                    return ushort.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(uint))
                    return uint.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(ulong))
                    return ulong.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(float))
                    return float.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(double))
                    return double.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(bool))
                    return bool.Parse(str);
                if (toType == typeof(TimeSpan))
                    return TimeSpan.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(DateTime))
                    return DateTime.Parse(str, CultureInfo.InvariantCulture);
                if (toType == typeof(char))
                {
                    char.TryParse(str, out var c);
                    return c;
                }
                if (toType == typeof(string) && str.StartsWith("{}", StringComparison.Ordinal))
                    return str.Substring(2);
                if (toType == typeof(string))
                    return value;
                if (toType == typeof(decimal))
                    return decimal.Parse(str, CultureInfo.InvariantCulture);
            }

            //if the value is not assignable and there's an implicit conversion, convert
            if (!toType.IsInstanceOfType(value))
            {
                var opImplicit = value.GetType().GetImplicitConversionOperator(value.GetType(), toType)
                                 ?? toType.GetImplicitConversionOperator(value.GetType(), toType);

                if (opImplicit != null)
                {
                    value = opImplicit.Invoke(null, new[] {value});
                    return value;
                }
            }

            var nativeValueConverterService = DependencyService.Get<INativeValueConverterService>();

            if (nativeValueConverterService != null &&
                nativeValueConverterService.ConvertTo(value, toType, out var nativeValue))
                return nativeValue;

            return value;
        }

        internal static MethodInfo GetImplicitConversionOperator(this Type onType, Type fromType, Type toType)
        {
            var mi = onType.GetRuntimeMethod("op_Implicit", new[] {fromType});
            if (mi == null) return null;
            if (!mi.IsSpecialName) return null;
            if (!mi.IsPublic) return null;
            if (!mi.IsStatic) return null;
            if (!toType.IsAssignableFrom(mi.ReturnType)) return null;

            return mi;
        }
    }
}
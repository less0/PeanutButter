﻿using System.Linq;
using System.Reflection;
#if BUILD_PEANUTBUTTER_DUCKTYPING_INTERNAL
using Imported.PeanutButter.DuckTyping.Extensions;
#else
using PeanutButter.DuckTyping.Extensions;
#endif

#if BUILD_PEANUTBUTTER_DUCKTYPING_INTERNAL
namespace Imported.PeanutButter.DuckTyping.AutoConversion.Converters
#else
namespace PeanutButter.DuckTyping.AutoConversion.Converters
#endif
{
    internal abstract class GenericStringConverterBase<T>: ConverterBase
    {
        protected readonly MethodInfo _tryParse = GetTryParseMethod();

        private static MethodInfo GetTryParseMethod()
        {
            return typeof(T)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(mi => DuckTypingHelperExtensions.IsTryParseMethod(mi));
        }
    }
}
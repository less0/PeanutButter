﻿using System;
using System.Collections;
using System.Collections.Generic;

#if BUILD_PEANUTBUTTER_INTERNAL
namespace Imported.PeanutButter.Utils
#else
namespace PeanutButter.Utils
#endif
{
    /// <summary>
    /// Provides extensions to convert from non-generic IDictionary to a generic one
    /// </summary>
#if BUILD_PEANUTBUTTER_INTERNAL
    internal
#else
    public
#endif
        static class DictionaryExtensions
    {
        /// <summary>
        /// Convert the given IDictionary to IDictionary&lt;TKey, TValue&gt; with
        /// the provided key and value generator functions
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="keyGenerator"></param>
        /// <param name="valueGenerator"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
#if BUILD_PEANUTBUTTER_INTERNAL
        internal
#else
        public
#endif
            static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(
                this IDictionary dict,
                Func<DictionaryEntry, TKey> keyGenerator,
                Func<DictionaryEntry, TValue> valueGenerator
            )
        {
            var result = new Dictionary<TKey, TValue>();
            if (dict is null)
            {
                return result;
            }

            foreach (DictionaryEntry item in dict)
            {
                result[keyGenerator(item)] = valueGenerator(item);
            }

            return result;
        }

        /// <summary>
        /// Converts the given non-generic IDictionary to IDictionary&lt;TKey, TValue&gt;
        /// with hard casting of keys and values in the source to the provided
        /// types
        /// </summary>
        /// <param name="dict"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
#if BUILD_PEANUTBUTTER_INTERNAL
internal
#else
        public
#endif
            static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(
                this IDictionary dict
            )
        {
            return dict.ToDictionary(o => (TKey) o.Key, o => (TValue) o.Value);
        }

        /// <summary>
        /// Find an item in or add an item to a dictionary
        /// - operation is thread-safe: dictionary is locked during search & add
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TValue FindOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dict,
            TKey key,
            TValue value
        )
        {
            return dict.FindOrAdd(key, () => value);
        }

        /// <summary>
        /// Find an item in or add an item to a dictionary
        /// - operation is thread-safe: dictionary is locked during search & add
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="generator"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TValue FindOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dict,
            TKey key,
            Func<TValue> generator
        )
        {
            if (dict is null)
            {
                throw new ArgumentNullException(nameof(dict));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (generator is null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            lock (dict)
            {
                if (dict.TryGetValue(key, out var existing))
                {
                    return existing;
                }
                var generated = generator();
                dict.Add(key, generated);
                return generated;
            }
        }

    }
}
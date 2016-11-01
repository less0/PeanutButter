﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PeanutButter.DuckTyping.Extensions;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace PeanutButter.DuckTyping.Tests.Extensions
{
    [TestFixture]
    public class TestDuckTypingHelperExtensions : AssertionHelper
    {
        [Test]
        public void IsCaseSensitive_OperatingOnCaseSensitiveDictionary_ShouldReturnTrue()
        {
            //--------------- Arrange -------------------
            var input = new Dictionary<string, object>();

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = input.IsCaseSensitive();

            //--------------- Assert -----------------------
            Expect(result, Is.True);
        }

        private static IEqualityComparer<string>[] _caseInsensitiveComparers = new[]
        {
            StringComparer.OrdinalIgnoreCase,
            StringComparer.CurrentCultureIgnoreCase,
            StringComparer.InvariantCultureIgnoreCase
        };

        [TestCaseSource(nameof(_caseInsensitiveComparers))]
        public void IsCaseSensitive_OperatingOnCaseInSensitiveDictionary_ShouldReturnFalse(
            IEqualityComparer<string> comparer
        )
        {
            //--------------- Arrange -------------------
            var input = new Dictionary<string, object>(comparer);

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = input.IsCaseSensitive();

            //--------------- Assert -----------------------
            Expect(result, Is.False);
        }

        [Test]
        public void IsCaseSensitive_OperatingOnSomethingWithoutComparerProperty_ShouldResortToBruteForce_EmptyIsNotSensitive()
        {
            //--------------- Arrange -------------------
            var dict = new MyDictionary(true);

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = dict.IsCaseSensitive();

            //--------------- Assert -----------------------
            Expect(result, Is.False);
        }

        [Test]
        public void IsCaseSensitive_OperatingOnSomethingWithoutComparerProperty_ShouldResortToBruteForce_CaseSensitive()
        {
            //--------------- Arrange -------------------
            var dict = new MyDictionary(true);
            dict.Add(new KeyValuePair<string, object>("Foo", "bar"));

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = dict.IsCaseSensitive();

            //--------------- Assert -----------------------
            Expect(result, Is.True);
        }

        [Test]
        public void ToCaseInsensitiveDictionary_OperatingOnCaseInsensitiveDictionary_ShouldReturnOriginal()
        {
            //--------------- Arrange -------------------
            var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", 1 }
            };

            //--------------- Assume ----------------
            Expect(dict.IsCaseSensitive(), Is.False);

            //--------------- Act ----------------------
            var result = dict.ToCaseInsensitiveDictionary();

            //--------------- Assert -----------------------
            Expect(result, Is.EqualTo(dict));
        }

        [Test]
        public void ToCaseInsensitiveDictionary_OperatingOnCaseSensitiveFlatDictionary_ShouldReturnNewCaseInsensitiveVariant()
        {
            //--------------- Arrange -------------------
            var expected = GetRandomInt();
            var dict = new Dictionary<string, object>()
            {
                { "Id", expected }
            };

            //--------------- Assume ----------------
            Expect(dict.IsCaseSensitive(), Is.True);

            //--------------- Act ----------------------
            var result = dict.ToCaseInsensitiveDictionary();

            //--------------- Assert -----------------------
            Expect(result, Is.Not.Null);
            Expect(result.IsCaseSensitive(), Is.False);
            Expect(() => result["id"], Throws.Nothing);
        }

        [Test]
        public void ToCaseInsensitiveDictionary_ShouldGoCaseInsensitiveAllTheWayDown()
        {
            //--------------- Arrange -------------------
            var expected = GetRandomInt();
            var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                { "Inner", new Dictionary<string, object>() { {  "Id", expected } } }
            };

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            var result = dict.ToCaseInsensitiveDictionary();

            //--------------- Assert -----------------------
            Expect((result["Inner"] as Dictionary<string, object>)["id"], Is.EqualTo(expected));
        }

        [Test]
        public void ToCaseInsensitiveDictionary_ShouldNotBeConfusedBySelfReferencingDictionaries()
        {
            //--------------- Arrange -------------------
            var outer = new Dictionary<string, object>();
            var inner = new Dictionary<string, object>();
            outer["Inner"] = inner;
            inner["Outer"] = outer;

            //--------------- Assume ----------------

            //--------------- Act ----------------------
            Expect(() =>
                outer.ToCaseInsensitiveDictionary(),
                Throws.Nothing);

            //--------------- Assert -----------------------
        }




        public class MyDictionary : IDictionary<string, object>
        {
            public MyDictionary(bool isCaseSensitive)
            {
                _isCaseSensitive = isCaseSensitive;
            }
            private List<KeyValuePair<string, object>> _data = new List<KeyValuePair<string, object>>();
            private bool _isCaseSensitive;

            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(KeyValuePair<string, object> item)
            {
                _data.Add(item);
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public bool Remove(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public int Count { get; }
            public bool IsReadOnly { get; }
            public bool ContainsKey(string key)
            {
                return _data.Any(kvp => KeysMatch(kvp.Key, key));
            }

            private bool KeysMatch(string k1, string k2)
            {
                return _isCaseSensitive
                        ? k1 == k2
                        : k1?.ToLower() == k2?.ToLower();
            }


            public void Add(string key, object value)
            {
                throw new NotImplementedException();
            }

            public bool Remove(string key)
            {
                throw new NotImplementedException();
            }

            public bool TryGetValue(string key, out object value)
            {
                if (!Keys.Select(k => _isCaseSensitive ? k : k.ToLower()).Contains(_isCaseSensitive ? key : key.ToLower()))
                {
                    value = null;
                    return false;
                }
                value = _data.First(kvp => kvp.Key == key).Value;
                return true;
            }

            public object this[string key]
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public ICollection<string> Keys => _data.Select(kvp => kvp.Key).ToList();
            public ICollection<object> Values => _data.Select(kvp => kvp.Value).ToList();
        }


    }
}
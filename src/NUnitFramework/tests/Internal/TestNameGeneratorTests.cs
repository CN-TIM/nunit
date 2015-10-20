﻿// ***********************************************************************
// Copyright (c) 2015 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.Reflection;

namespace NUnit.Framework.Internal.Tests
{
    public class TestNameGeneratorTests
    {
        private MethodInfo _simpleTest;
        private MethodInfo _genericTest;

        [SetUp]
        public void InitializeMethodInfos()
        {
            _simpleTest = GetType().GetMethod("TestMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            _genericTest = GetType().GetMethod("GenericTest", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [TestCase("FIXED", ExpectedResult = "FIXED")]
        [TestCase("{m}", ExpectedResult = "TestMethod")]
        [TestCase("{n}", ExpectedResult = "NUnit.Framework.Internal.Tests")]
        [TestCase("{c}", ExpectedResult = "TestNameGeneratorTests")]
        [TestCase("{C}", ExpectedResult = "NUnit.Framework.Internal.Tests.TestNameGeneratorTests")]
        [TestCase("{M}", ExpectedResult = "NUnit.Framework.Internal.Tests.TestNameGeneratorTests.TestMethod")]
        [TestCase("{m}_SpecialCase", ExpectedResult = "TestMethod_SpecialCase")]
        [TestCase("{n}.{c}.{m}", ExpectedResult = "NUnit.Framework.Internal.Tests.TestNameGeneratorTests.TestMethod")]
        [TestCase("{x}", ExpectedResult = "{x}")]
        [TestCase("{n}.{c.{m}", ExpectedResult = "NUnit.Framework.Internal.Tests.{c.{m}")]
        [TestCase("{m}{a}", ExpectedResult = "TestMethod")]
        public string SimpleTestNames(string pattern)
        {
            return new TestNameGenerator(pattern).GetDisplayName(_simpleTest);
        }

        [TestCase("{m}{a}", new object[] { 1, 2 }, ExpectedResult = "TestMethod(1,2)")]
        [TestCase("{m}{a:50}", new object[] { 1, 2 }, ExpectedResult = "TestMethod(1,2)")]
        [TestCase("{m}{a}", new object[] { "Now is the time for all good men to come to the aid of their country." },
            ExpectedResult = "TestMethod(\"Now is the time for all good men to c...\")")]
        [TestCase("{m}{a:20}", new object[] { "Now is the time for all good men to come to the aid of their country." },
            ExpectedResult = "TestMethod(\"Now is the time f...\")")]
        [TestCase("{m}{a:40}", new object[] { "Now is the time for all good men to come to the aid of their country." },
            ExpectedResult = "TestMethod(\"Now is the time for all good men to c...\")")]
        [TestCase("{m}{a:100}", new object[] { "Now is the time for all good men to come to the aid of their country." },
            ExpectedResult = "TestMethod(\"Now is the time for all good men to come to the aid of their country.\")")]
        [TestCase("{m}{a:20}", new object[] { 42, "Now is the time for all good men to come to the aid of their country.", 5.2 },
            ExpectedResult = "TestMethod(42,\"Now is the time f...\",5.2d)")]
        public string ParameterizedTests(string pattern, object[] args)
        {
            return new TestNameGenerator(pattern).GetDisplayName(_simpleTest, args);
        }

        [TestCase("FIXED", ExpectedResult="FIXED")]
        [TestCase("{m}",   ExpectedResult="GenericTest<T,U,V>")]
        [TestCase("{n}", ExpectedResult = "NUnit.Framework.Internal.Tests")]
        [TestCase("{c}", ExpectedResult = "TestNameGeneratorTests")]
        [TestCase("{C}", ExpectedResult = "NUnit.Framework.Internal.Tests.TestNameGeneratorTests")]
        [TestCase("{M}", ExpectedResult = "NUnit.Framework.Internal.Tests.TestNameGeneratorTests.GenericTest<T,U,V>")]
        [TestCase("{m}_SpecialCase", ExpectedResult = "GenericTest<T,U,V>_SpecialCase")]
        [TestCase("{n}.{c}.{m}", ExpectedResult = "NUnit.Framework.Internal.Tests.TestNameGeneratorTests.GenericTest<T,U,V>")]
        [TestCase("{x}", ExpectedResult = "{x}")]
        [TestCase("{n}.{c.{m}", ExpectedResult = "NUnit.Framework.Internal.Tests.{c.{m}")]
        public string GenericTestNames(string pattern)
        {
            return new TestNameGenerator(pattern).GetDisplayName(_genericTest);
        }

        #region Methods Used as Data

        private void TestMethod() { }

        private void GenericTest<T, U, V>() { }

        #endregion
    }
}

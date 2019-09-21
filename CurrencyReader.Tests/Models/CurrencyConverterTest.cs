using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CurrencyReader.Models;

namespace CurrencyReader.Tests.Models
{
    [TestClass]
    public class CurrencyConverterTest
    {
        [TestMethod]
        public void TestConvertWithThousandths()
        {
            var model1 = new CurrencyConverter()
            {
                InputNumber = "999,999,999.99"
            };

            var model2 = new CurrencyConverter()
            {
                InputNumber = "999999999.99"
            };

            var convertResult1 = model1.Convert();
            Assert.IsNotNull(convertResult1);
            Assert.IsTrue(convertResult1.IsSuccessful);

            var convertResult2 = model2.Convert();
            Assert.IsNotNull(convertResult2);
            Assert.IsTrue(convertResult2.IsSuccessful);

            Assert.AreEqual("Nine Hundred and Ninety Nine Million Nine Hundred and Ninety Nine Thousand Nine Hundred and Ninety Nine Dollars and Ninety Nine Cents",
                convertResult1.Data, "Thousandths should be allowed to input");
            Assert.AreEqual(convertResult1.Data,
                convertResult2.Data, "Numbers with thousandths should be read the same as the numbers without thousandths");
        }

        [TestMethod]
        public void TestConvertWithScientificNotition()
        {
            var model = new CurrencyConverter()
            {
                InputNumber = "9.1234567E+25"
            };

            var convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Ninety One Septillion Two Hundred and Thirty Four Sextillion Five Hundred and Sixty Seven Quintillion Dollars",
                convertResult.Data, "Scientific notition should be allowed to input");
        }

        [TestMethod]
        public void TestConvert()
        {
            var model = new CurrencyConverter()
            {
                InputNumber = "130423.05"
            };

            var convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("One Hundred and Thirty Thousand Four Hundred and Twenty Three Dollars and Five Cents", convertResult.Data);

            model.InputNumber = "1000010000.15";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("One Billion Ten Thousand Dollars and Fifteen Cents", convertResult.Data);

            model.InputNumber = "1.151";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("One Dollar and Fifteen Cents", convertResult.Data);

            model.InputNumber = "1.01";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("One Dollar and One Cent", convertResult.Data);

            model.InputNumber = "240,000,100.00";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Two Hundred and Forty Million One Hundred Dollars", convertResult.Data);

            model.InputNumber = "240,000,100.12";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Two Hundred and Forty Million One Hundred Dollars and Twelve Cents", convertResult.Data);

            model.InputNumber = "240,000,010.47";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Two Hundred and Forty Million and Ten Dollars and Forty Seven Cents", convertResult.Data);
            
            model.InputNumber = "201,010,000.1";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Two Hundred and One Million Ten Thousand Dollars and Ten Cents", convertResult.Data);

            model.InputNumber = "201,010.2";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Two Hundred and One Thousand and Ten Dollars and Twenty Cents", convertResult.Data);

            model.InputNumber = "201,100.1855";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Two Hundred and One Thousand One Hundred Dollars and Nineteen Cents", convertResult.Data);
            
            model.InputNumber = "201,100,000";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Two Hundred and One Million One Hundred Thousand Dollars", convertResult.Data);

            model.InputNumber = "100,000,000,000,000,000,000,000.99";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("One Hundred Sextillion Dollars and Ninety Nine Cents", convertResult.Data);
            
            model.InputNumber = "100,000,000,000,000,000,000,000.9999";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("One Hundred Sextillion and One Dollars", convertResult.Data);


            model.InputNumber = "99,999,999,999,999,999,999,999,999.99";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Ninety Nine Septillion Nine Hundred and Ninety Nine Sextillion Nine Hundred and Ninety Nine Quintillion" +
                " Nine Hundred and Ninety Nine Quadrillion Nine Hundred and Ninety Nine Trillion Nine Hundred and Ninety Nine Billion Nine Hundred" +
                " and Ninety Nine Million Nine Hundred and Ninety Nine Thousand Nine Hundred and Ninety Nine Dollars and Ninety Nine Cents", convertResult.Data);
        }

        [TestMethod]
        public void TestConvertZero()
        {
            var model = new CurrencyConverter()
            {
                InputNumber = "0"
            };
            
            var convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Zero", convertResult.Data);
        }

        [TestMethod]
        public void TestConvertNegativeNumber()
        {
            var model = new CurrencyConverter()
            {
                InputNumber = "-1.151"
            };

            var convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Negative One Dollar and Fifteen Cents", convertResult.Data);

            model.InputNumber = "-1.155";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual("Negative One Dollar and Sixteen Cents", convertResult.Data);
        }

        [TestMethod]
        public void TestConvertWithInvalidNumber()
        {
            var model = new CurrencyConverter()
            {
                InputNumber = "1234.11a"
            };

            var convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsFalse(convertResult.IsSuccessful);
            Assert.AreEqual("Please enter a valid number", convertResult.ResultMessage, "If input string is not valid number, error message should be generated");

            model.InputNumber = string.Empty;
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsFalse(convertResult.IsSuccessful);
            Assert.AreEqual("Please enter a valid number", convertResult.ResultMessage, "If input string is not valid number, error message should be displayed");
            
            model.InputNumber = null;
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsFalse(convertResult.IsSuccessful);
            Assert.AreEqual("Please enter a valid number", convertResult.ResultMessage, "If input string is not valid number, error message should be displayed");

            model.InputNumber = "8E+28"; // 8E+28 is larger than decimal.MaxValue
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsFalse(convertResult.IsSuccessful);
            Assert.AreEqual("Please enter a valid number", convertResult.ResultMessage, "If input string is not valid number, error message should be displayed");

            model.InputNumber = "1234.11";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual(string.Empty, convertResult.ResultMessage, "If input string is valid, error message should be empty");
        }

        [TestMethod]
        public void TestConvertWithTooLongInput()
        {
            var model = new CurrencyConverter()
            {
                InputNumber = "99,999,999,999,999,999,999,999,999.990"
            };

            var convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsFalse(convertResult.IsSuccessful);
            Assert.AreEqual("Please input a number which has maximum 28 digits", convertResult.ResultMessage, "If input string has more than 28 digits, error message should be displayed");

            model.InputNumber = "99,999,999,999,999,999,999,999,999.99";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual(string.Empty, convertResult.ResultMessage, "If input string is a valid number with (<=28) digits, error message should not be displayed");
        }

        [TestMethod]
        public void TestConvertWithOutOfRangeData()
        {
            var model = new CurrencyConverter()
            {
                InputNumber = "100,000,000,000,000,000,000,000,000"
            };

            var convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsFalse(convertResult.IsSuccessful);
            Assert.AreEqual("The number input should between -99999999999999999999999999.99 and 99999999999999999999999999.99",
                convertResult.ResultMessage, "If input number is not in [-99999999999999999999999999.99, 99999999999999999999999999.99], error message should be displayed");

            model.InputNumber = "99,999,999,999,999,999,999,999,999.99";
            convertResult = model.Convert();
            Assert.IsNotNull(convertResult);
            Assert.IsTrue(convertResult.IsSuccessful);
            Assert.AreEqual(string.Empty, convertResult.ResultMessage,
                "If input number is a valid number in [-99999999999999999999999999.99, 99999999999999999999999999.99], error message should not be displayed");
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileParser;
using System.Data;
using System.Collections.Generic;

namespace FileParserTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFileOpen()
        {
            var result = Program.GetInputFile(null);
            Assert.AreEqual(result,"data.csv");
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void TestInvalidFileOpen()
        {
            var args = new string[1] { "invalidfilename.csv" };
            Program.GetInputFile(args);
        }
        [TestMethod]
        public void TestFileRead()
        {
            var result = Program.ParseFile("data.csv");
            Assert.AreEqual(result.Count, 8);
        }
        [TestMethod]
        public void TestProcessNames()
        {
            var people = new List<Person>();
            people.Add(new Person { FirstName = "Adam", LastName = "Baker", Address = "1 Zebra St", PhoneNo = "8755523265" });
            people.Add(new Person { FirstName = "Baker", LastName = "Charles", Address = "2 Avenue Ave", PhoneNo = "8755523266" });
            people.Add(new Person { FirstName = "Diane", LastName = "Baker", Address = "3 Penguin St", PhoneNo = "87555232657" });
            people.Add(new Person { FirstName = "Adam", LastName = "Elliot", Address = "4 Skillet St", PhoneNo = "87555232658" });
            var result = Program.ProcessNameData(people);
            for (var i = 0; i < result.Rows.Count; i++)
            {
                var row = result.Rows[i];
                switch (i)
                {
                    case 0:
                        Assert.AreEqual("Baker", row["Name"]);
                        Assert.AreEqual("3", row["Count"]);
                        break;
                    case 1:
                        Assert.AreEqual("Adam", row["Name"]);
                        Assert.AreEqual("2", row["Count"]);
                        break;
                    case 2:
                        Assert.AreEqual("Charles", row["Name"]);
                        Assert.AreEqual("1", row["Count"]);
                        break;
                    case 3:
                        Assert.AreEqual("Diane", row["Name"]);
                        Assert.AreEqual("1", row["Count"]);
                        break;
                    case 4:
                        Assert.AreEqual("Elliot", row["Name"]);
                        Assert.AreEqual("1", row["Count"]);
                        break;
                }
            }
        }
        [TestMethod]
        public void TestSortAddress()
        {
            var people = new List<Person>();
            people.Add(new Person { FirstName="FirstName1",LastName="LastName1",Address="1 Zebra St",PhoneNo="8755523265" });
            people.Add(new Person { FirstName = "FirstName2", LastName = "LastName2", Address = "2 Avenue Ave", PhoneNo = "8755523266" });
            people.Add(new Person { FirstName = "FirstName3", LastName = "LastName3", Address = "3 Penguin St", PhoneNo = "87555232657" });
            var result = Program.SortAddressData(people);
            Assert.AreEqual("2 Avenue Ave", result.Rows[0]["Address"]);
            Assert.AreEqual("3 Penguin St", result.Rows[1]["Address"]);
            Assert.AreEqual("1 Zebra St", result.Rows[2]["Address"]);
        }
        [TestMethod]
        public void TestFileWrite()
        {
            var data = new DataTable();
            data.Columns.Add("Test");
            var row = data.NewRow();
            row["Test"] = "Test data";
            Program.WriteOuputFile("FileWriteTest.txt", data);
            Assert.IsTrue(System.IO.File.Exists("FileWriteTest.txt"));
            System.IO.File.Delete("FileWriteTest.txt");
        }
    }
}

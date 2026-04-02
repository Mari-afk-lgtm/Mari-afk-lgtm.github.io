
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary1;

namespace UnitTestProject1
{
    [TestClass]
    public class SlovarTests
    {
        private string testFile = "test.txt";
        private Class1 dict;

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(testFile,
                "абв\nабвг\nааа\nгде\nгдее\nуник\nуникум", Encoding.UTF8);
            dict = new Class1(testFile);
        }
        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(testFile))
                File.Delete(testFile);
        }
        [TestMethod]
        public void TestLoadDictionary()
        {
            Assert.AreEqual(7, dict.Count);
        }
        [TestMethod]
        public void TestAddWord()
        {
            dict.AddWord("новый");
            Assert.AreEqual(8, dict.Count);
        }
        [TestMethod]
        public void TestAddDuplicateWord()
        {
            int oldCount = dict.Count;
            dict.AddWord("абв");
            Assert.AreEqual(oldCount, dict.Count);
        }
        [TestMethod]
        public void TestRemoveWord()
        {
            dict.RemoveWord("абв");
            Assert.AreEqual(6, dict.Count);
        }
        [TestMethod]
        public void TestFindUniqueLettersWords()
        {
            var result = dict.FindUniqueLettersWords(3);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains("абв"));
            Assert.IsTrue(result.Contains("где"));
            Assert.IsFalse(result.Contains("ааа"));
        }
        [TestMethod]
        public void TestFuzzySearch()
        {
            var result = dict.FuzzySearch("абв");
            Assert.IsTrue(result.Contains("абв"));
            Assert.IsTrue(result.Contains("абвг"));
        }
        [TestMethod]
        public void TestFuzzySearchNoResults()
        {
            var result = dict.FuzzySearch("xyz");
            Assert.AreEqual(0, result.Count);
        }
        [TestMethod]
        public void TestGetWordsStartingWith()
        {
            var result = dict.GetWordsStartingWith("аб");
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains("абв"));
            Assert.IsTrue(result.Contains("абвг"));
        }
        [TestMethod]
        public void TestSaveToFile()
        {
            string saveFile = "saved.txt";
            dict.SaveToFile(saveFile);
            Assert.IsTrue(File.Exists(saveFile));
            File.Delete(saveFile);
        }
    }
}
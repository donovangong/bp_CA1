using BPCalculator;
using System.ComponentModel.DataAnnotations;

namespace UnitTest
{
    [TestClass]
    [DoNotParallelize]
    public sealed class Core_func_test
    {
        [TestMethod]
        public void TestMethod1()
        {
            var bp = new BloodPressure { Systolic = 80, Diastolic = 50 };
            Assert.AreEqual(BPCategory.Low, bp.Category);

            bp = new BloodPressure { Systolic = 89, Diastolic = 59 };
            Assert.AreEqual(BPCategory.Low, bp.Category);

            bp = new BloodPressure { Systolic = 90, Diastolic = 60 };
            Assert.AreEqual(BPCategory.Ideal, bp.Category);

            bp = new BloodPressure { Systolic = 110, Diastolic = 75 };
            Assert.AreEqual(BPCategory.Ideal, bp.Category);

            bp = new BloodPressure { Systolic = 119, Diastolic = 79 };
            Assert.AreEqual(BPCategory.Ideal, bp.Category);

            bp = new BloodPressure { Systolic = 120, Diastolic = 80 };
            Assert.AreEqual(BPCategory.PreHigh, bp.Category);

            bp = new BloodPressure { Systolic = 130, Diastolic = 85 };
            Assert.AreEqual(BPCategory.PreHigh, bp.Category);

            bp = new BloodPressure { Systolic = 139, Diastolic = 89 };
            Assert.AreEqual(BPCategory.PreHigh, bp.Category);

            bp = new BloodPressure { Systolic = 140, Diastolic = 90 };
            Assert.AreEqual(BPCategory.High, bp.Category);

            bp = new BloodPressure { Systolic = 150, Diastolic = 95 };
            Assert.AreEqual(BPCategory.High, bp.Category);

            bp = new BloodPressure { Systolic = BloodPressure.SystolicMax, Diastolic = BloodPressure.DiastolicMax };
            Assert.AreEqual(BPCategory.High, bp.Category);

            bp = new BloodPressure { Systolic = 50, Diastolic = 30 };
            var context = new ValidationContext(bp);
            var results = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(bp, context, results, true);
            Assert.IsFalse(valid);

            bp = new BloodPressure { Systolic = 200, Diastolic = 110 };
            context = new ValidationContext(bp);
            results = new List<ValidationResult>();
            valid = Validator.TryValidateObject(bp, context, results, true);
            Assert.IsFalse(valid);

        }

        [TestMethod]
        public void AddToHistory_StoresValuesCorrectly()
        {
            BloodPressure.History.Clear();
            
            var bp = new BloodPressure { Systolic = 120, Diastolic = 70 };
            bp.AddToHistory();

            Assert.AreEqual(1, BloodPressure.History.Count);
            Assert.AreEqual(120, BloodPressure.History.Peek().Systolic);
            Assert.AreEqual(70, BloodPressure.History.Peek().Diastolic);
        }

        [TestMethod]
        public void SysSeries_And_DiaSeries_ReturnCorrectData()
        {
            BloodPressure.History.Clear();
            
            new BloodPressure { Systolic = 100, Diastolic = 60 }.AddToHistory();
            new BloodPressure { Systolic = 110, Diastolic = 70 }.AddToHistory();

            var sys = BloodPressure.SysSeries;
            var dia = BloodPressure.DiaSeries;

            CollectionAssert.AreEqual(new[] { 100, 110 }, sys);
            CollectionAssert.AreEqual(new[] { 60, 70 }, dia);
        }

        [TestMethod]
        public void History_DoesNotExceedTenItems()
        {
            BloodPressure.History.Clear();
            for (int i = 0; i < 12; i++)
            {
                new BloodPressure { Systolic = 100 + i, Diastolic = 60 + i }.AddToHistory();
            }

            Assert.AreEqual(10, BloodPressure.History.Count);

            // First two records dropped
            Assert.AreEqual(102, BloodPressure.History.Peek().Systolic);
            Assert.AreEqual(62, BloodPressure.History.Peek().Diastolic);
        }



    }
}

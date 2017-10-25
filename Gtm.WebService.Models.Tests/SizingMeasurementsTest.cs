using System;
using Gtm.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtm.WebService.Models.Tests
{
   [TestClass]
   public class SizingMeasurementsTest
   {
      [TestMethod]
      public void ShouldThrowExceptionsOnConstructorParameters()
      {
         AssertArgumentNullException.IsThrownForParameter(() => new SizingMeasurements.TypeMeasurements(null), "measurementType");
         AssertArgumentException.IsThrownForParameter(() => new SizingMeasurements.TypeMeasurements(string.Empty), "measurementType");

         AssertArgumentNullException.IsThrownForParameter(() => new SizingMeasurements.SizeMeasurement(null, "measurement"), "size");
         AssertArgumentNullException.IsThrownForParameter(() => new SizingMeasurements.SizeMeasurement("size", null), "measurement");

         AssertArgumentException.IsThrownForParameter(() => new SizingMeasurements.SizeMeasurement(string.Empty, "measurement"), "size");
         AssertArgumentException.IsThrownForParameter(() => new SizingMeasurements.SizeMeasurement("size", string.Empty), "measurement");
      }

      [TestMethod]
      public void SizingChartShouldAllowAccessToProperties()
      {
         var created = new SizingMeasurements();

         Assert.IsNotNull(created.Sizes, "Sizes should not be null.");
         Assert.AreEqual(0, created.Sizes.Count, "Sizes should be empty.");

         Assert.IsNotNull(created.Measurements, "Measurements should not be null.");
         Assert.AreEqual(0, created.Measurements.Count, "Measurements should be empty");
      }

      [TestMethod]
      public void SizingMeasurementsShouldCloneProperly()
      {
         var created = new SizingMeasurements();
         created.Sizes.Add(CreateTestString());
         created.Measurements.Add(new SizingMeasurements.TypeMeasurements(CreateTestString()));

         created.Measurements[0].SizeMeasurements.Add(new SizingMeasurements.SizeMeasurement(CreateTestString(), CreateTestString()));

         var cloned = created.Clone();

         Assert.AreNotSame(created, cloned, "Should not be the same reference.");
         Assert.AreNotSame(created.Sizes, cloned.Sizes, "Sizes should not be same references.");
         Assert.AreNotSame(created.Measurements, cloned.Measurements, "Measurements should not be same references.");

         AssertSizingMeasurementsAreEqual(created, cloned);
      }

      [TestMethod]
      public void TypeMeasurementsShouldAllowAccessToProperties()
      {
         var created = new SizingMeasurements.TypeMeasurements("testType");

         Assert.AreEqual("testType", created.MeasurementType, "MeasurementType should be equal.");
         Assert.IsNotNull(created.SizeMeasurements, "Measurements should not be null.");
         Assert.AreEqual(0, created.SizeMeasurements.Count, "Measurements should be empty.");
      }

      [TestMethod]
      public void TypeMeasurementsShouldCloneProperly()
      {
         var created = new SizingMeasurements.TypeMeasurements(Guid.NewGuid().ToString("N"));
         created.SizeMeasurements.Add(new SizingMeasurements.SizeMeasurement(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N")));

         var cloned = created.Clone();

         Assert.AreNotSame(created, cloned, "Should not be the same reference.");
         Assert.AreNotSame(created.SizeMeasurements, cloned.SizeMeasurements, "SizeMeasurements should not be the same reference.");
         AssertTypeMeasurementsAreEqual(created, cloned);
      }

      [TestMethod]
      public void SizeMeasurementShouldAllowAccessToProperties()
      {
         var created = new SizingMeasurements.SizeMeasurement("XS", "0");

         Assert.AreEqual("XS", created.Size, "Size should be equal.");
         Assert.AreEqual("0", created.Measurement, "Measurement should be equal.");
      }

      [TestMethod]
      public void SizeMeasurementShouldCloneProperly()
      {
         var created = new SizingMeasurements.SizeMeasurement(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
         var cloned = created.Clone();

         Assert.AreNotSame(created, cloned, "Should not be the same reference.");
         AssertSizeMeasurementsAreEqual(created, cloned);
      }

      [TestMethod]
      public void ShouldAllowSerialization()
      {
         var original = new SizingMeasurements
         {
            Sizes = { "XS", "S" },

            Measurements =
            {
               new SizingMeasurements.TypeMeasurements("Chest")
               {
                  SizeMeasurements =
                  {
                     new SizingMeasurements.SizeMeasurement("XS", "1"),
                     new SizingMeasurements.SizeMeasurement("S", "2")
                  }
               }
            }
         };

         var deserialized = DataContractSerialization.GetDeserializedInstance(original);

         Assert.IsNotNull(deserialized, "Deserialized instance should not be null.");

         AssertSizingMeasurementsAreEqual(original, deserialized);
      }

      private static string CreateTestString()
      {
         return Guid.NewGuid().ToString("N");
      }

      private static void AssertSizingMeasurementsAreEqual(SizingMeasurements expected, SizingMeasurements actual)
      {
         Assert.AreEqual(expected.Sizes.Count, actual.Sizes.Count, "Sizes should have same number of items.");

         for (var i = 0; i < expected.Sizes.Count; i++)
            Assert.AreEqual(expected.Sizes[i], actual.Sizes[i]);

         Assert.AreEqual(expected.Measurements.Count, actual.Measurements.Count, "Measurements should have same number of items.");

         for (var i = 0; i < expected.Measurements.Count; i++)
            AssertTypeMeasurementsAreEqual(expected.Measurements[i], actual.Measurements[i]);
      }

      private static void AssertTypeMeasurementsAreEqual(SizingMeasurements.TypeMeasurements expected, SizingMeasurements.TypeMeasurements actual)
      {
         Assert.AreEqual(expected.MeasurementType, actual.MeasurementType, "MeasuremenType should be equal.");
         Assert.AreEqual(expected.SizeMeasurements.Count, actual.SizeMeasurements.Count, "SizeMeasurements should have same number of items.");

         for (var i = 0; i < expected.SizeMeasurements.Count; i++)
            AssertSizeMeasurementsAreEqual(expected.SizeMeasurements[i], actual.SizeMeasurements[i]);
      }

      private static void AssertSizeMeasurementsAreEqual(SizingMeasurements.SizeMeasurement expected, SizingMeasurements.SizeMeasurement actual)
      {
         Assert.AreEqual(expected.Size, actual.Size, "Size should be equal.");
         Assert.AreEqual(expected.Measurement, actual.Measurement, "Measurement should be equal.");
      }
   }
}

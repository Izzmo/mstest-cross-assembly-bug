using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gtm.TestUtilities;
using Gtm.Common;

namespace Gtm.WebService.Models.Tests
{
   [TestClass]
   public class DesignTest
   {
      private VersionInfo created;
      private Product product;
      private ImageUrls imageUrls;
      private Dictionary<string, Size> defaultSizes = new Dictionary<string, Size>
      {
         { "M", new Size("M", 5.0m) }
      };

      private Guid guid = Guid.NewGuid();

      [TestInitialize]
      public void TestInit()
      {
         product = new Product("Hanes Tee", "14500TU", "Red", defaultSizes);
         imageUrls = new ImageUrls(new Uri("http://test"), new Uri("http://test"), new Uri("http://test"), new Uri("http://test"));
         created = new VersionInfo("Tester");
      }

      [TestMethod]
      public void ShouldThrowArgumentExceptionOnInvalidParametersInConstructor()
      {
         AssertArgumentException.IsThrownForParameter(() => new Design(Guid.Empty, product, imageUrls, created), "identifier");

         AssertArgumentNullException.IsThrownForParameter(() => new Design(guid, null, imageUrls, created), "product");
         AssertArgumentNullException.IsThrownForParameter(() => new Design(guid, product, null, created), "imageUrls");
         AssertArgumentNullException.IsThrownForParameter(() => new Design(guid, product, imageUrls, null), "createdInfo");
      }

      [TestMethod]
      public void ShouldSetPropertiesCorrectly()
      {
         var design = new Design(guid, product, imageUrls, created);

         Assert.AreEqual(guid, design.Identifier, "Guid should be equal.");
         Assert.IsNotNull(design.Product, "Product should be set.");
         Assert.IsNotNull(design.ImageUrls, "ImageUrls should be set.");
         Assert.AreEqual(created.By, design.Created.By, "Created By should be equal.");
         Assert.IsFalse(design.IsEmbellished, "Should not be embellished.");
      }

      [TestMethod]
      public void ShouldOutputTrueIsEmbellished()
      {
         var design = new Design(guid, product, imageUrls, created, true);

         Assert.IsTrue(design.IsEmbellished);
      }

      [TestMethod]
      public void ShouldSerializeDataContract()
      {
         var design = new Design(guid, product, imageUrls, created);
         var deserializedDesign = DataContractSerialization.GetDeserializedInstance(design);

         Assert.AreEqual(design.Created.By, deserializedDesign.Created.By, "Created By should be equal.");
         Assert.AreEqual(design.Created.On, deserializedDesign.Created.On, "Created On should be equal.");
         Assert.AreEqual(design.Identifier, deserializedDesign.Identifier, "Identifier should be equal.");
         Assert.AreEqual(design.IsEmbellished, deserializedDesign.IsEmbellished, "IsEmbellished should be equal.");
         Assert.AreEqual(design.ImageUrls.Back, deserializedDesign.ImageUrls.Back, "ImageUrls count should be equal.");
         Assert.AreEqual(design.Product.Name, deserializedDesign.Product.Name, "Product count should be equal.");
      }

      [TestMethod]
      public void ShouldBeAbleToClone()
      {
         var design = new Design(guid, product, imageUrls, created);

         var cloned = design.Clone();

         Assert.AreEqual(design.Created.By, cloned.Created.By, "Created.By should be equal.");
         Assert.AreEqual(design.Identifier, cloned.Identifier, "Identifier should be equal.");
         Assert.AreNotSame(design.ImageUrls, cloned.ImageUrls, "ImageUrls should not be same reference.");
         Assert.AreNotSame(design.Product, cloned.Product, "Product should not be same reference.");
      }
   }
}

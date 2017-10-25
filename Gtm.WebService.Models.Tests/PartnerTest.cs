using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gtm.TestUtilities;

namespace Gtm.WebService.Models.Tests
{
   [TestClass]
   public class PartnerTest
   {
      private const string DefaultName = "Team Snap";
      private Guid defaultAuthorizationToken = Guid.NewGuid();

      [TestMethod]
      public void ShouldSerializeDataContract()
      {
         var instanceForSerialization = new Partner(DefaultName, defaultAuthorizationToken);
         var deserializedInstance = DataContractSerialization.GetDeserializedInstance(instanceForSerialization);

         Assert.AreNotEqual(Guid.Empty, deserializedInstance.AuthorizationToken, "AuthorizationToken should not be empty.");
         Assert.AreEqual(DefaultName, deserializedInstance.Name, "Name should be equal.");
      }

      [TestMethod]
      public void ShouldThrowArgumentExceptionOnInvalidParamentersInConstructor()
      {
         AssertArgumentException.IsThrownForParameter(() => new Partner("Partner Name", Guid.Empty), "authorizationToken");
         AssertArgumentException.IsThrownForParameter(() => new Partner(string.Empty, Guid.NewGuid()), "name");

         AssertArgumentNullException.IsThrownForParameter(() => new Partner(null, Guid.NewGuid()), "name");
      }

      [TestMethod]
      public void ShouldBeAbleToClone()
      {
         var partner = new Partner(DefaultName, defaultAuthorizationToken);

         var cloned = partner.Clone();

         Assert.AreEqual(partner.Name, cloned.Name, "Name should be equal.");
         Assert.AreEqual(partner.AuthorizationToken, cloned.AuthorizationToken, "AuthorizationToken should be equal.");
      }
   }
}

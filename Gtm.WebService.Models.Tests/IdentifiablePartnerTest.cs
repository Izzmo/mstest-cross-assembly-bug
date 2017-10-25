using System;
using Gtm.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtm.WebService.Models.Tests
{
   [TestClass]
   public class IdentifiablePartnerTest
   {
      private const string DefaultPartnerName = "Team Snap";

      private Guid defaultPartnerIdentifier = Guid.Parse("e075758e-c010-4eaa-9967-dd01171b738a");
      private Guid defaultPartnerAuthorizationToken = Guid.NewGuid();

      [TestMethod]
      public void ShouldThrowArgumentExceptionOnInvalidParametersInConstructor()
      {
         AssertException<ArgumentException>.IsThrownForParameter(() => new IdentifiablePartner(Guid.Empty, DefaultPartnerName, defaultPartnerAuthorizationToken), "identifier");
      }

      [TestMethod]
      public void ShouldBeAbleToAccessPropertiesWhenUsing3ParamConstructor()
      {
         var identifiablePartner = new IdentifiablePartner(defaultPartnerIdentifier, "Team Snap", Guid.NewGuid());

         Assert.AreEqual(defaultPartnerIdentifier, identifiablePartner.Identifier, "Identifier should be equal from constructor.");
      }

      [TestMethod]
      public void ShouldSerializeDataContract()
      {
         var instanceForSerialization = new IdentifiablePartner(defaultPartnerIdentifier, DefaultPartnerName, defaultPartnerAuthorizationToken);
         var deserializedInstance = DataContractSerialization.GetDeserializedInstance(instanceForSerialization);

         Assert.AreEqual(instanceForSerialization.Identifier, deserializedInstance.Identifier, "Identifier should be equal from constructor.");
         Assert.AreEqual(instanceForSerialization.Name, deserializedInstance.Name, "Name should be equal.");
      }

      [TestMethod]
      public void ShouldBeAbleToClone()
      {
         var partner = new IdentifiablePartner(defaultPartnerIdentifier, DefaultPartnerName, defaultPartnerAuthorizationToken);

         var cloned = partner.Clone();

         Assert.AreEqual(partner.AuthorizationToken, cloned.AuthorizationToken, "AuthorizationToken should be equal.");
         Assert.AreEqual(partner.Identifier, cloned.Identifier, "Identifier should be equal.");
         Assert.AreEqual(partner.Name, cloned.Name, "Name should be equal.");
      }
   }
}

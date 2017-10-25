using Microsoft.ServiceFabric.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtm.WebService.Models.Tests
{
   [TestClass]
   public abstract class TransactionalRepositoryTest<TRepository>
   {
      private MockReliableStateManagerSpy stateManagerSpy;

      public TRepository Repository { get; private set; }

      [TestInitialize]
      public void TransactionalRepositoryTestInitialize()
      {
         stateManagerSpy = new MockReliableStateManagerSpy();
         Repository = CreateRepository(stateManagerSpy);
      }

      protected void AssertTransactionWasCommitted()
      {
         Assert.IsTrue(stateManagerSpy.TransanctionIsCreated, "Transaction was not created.");
         Assert.IsTrue(stateManagerSpy.TransactionIsCommitted, "Transaction was created but not committed.");
      }

      protected void AssertTransactionWasAborted()
      {
         Assert.IsTrue(stateManagerSpy.TransanctionIsCreated, "An aborted transaction was expected, but no transaction was created.");
         Assert.IsTrue(stateManagerSpy.TransactionIsCommitted, "A transaction was expected to be aborted, but was not.");
      }

      protected abstract TRepository CreateRepository(IReliableStateManager stateManager);
   }
}

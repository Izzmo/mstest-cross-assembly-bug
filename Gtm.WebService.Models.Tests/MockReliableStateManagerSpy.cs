using System;
using Microsoft.ServiceFabric.Data;
using ServiceFabric.Mocks;

namespace Gtm.WebService.Models.Tests
{
   public class MockReliableStateManagerSpy : MockReliableStateManager, IReliableStateManager
   {
      public MockTransactionSpy Transaction { get; private set; }

      public bool TransanctionIsCreated => Transaction != null;

      public bool TransactionIsCommitted => Transaction != null && Transaction.IsCommitted;

      public bool TransactionIsAborted => Transaction != null && Transaction.IsAborted;

      ITransaction IReliableStateManager.CreateTransaction()
      {
         if (Transaction != null && !Transaction.IsCompleted)
            throw new InvalidOperationException("Only one transaction is currently supported.");

         Transaction = new MockTransactionSpy(CreateTransaction());

         return Transaction;
      }
   }
}

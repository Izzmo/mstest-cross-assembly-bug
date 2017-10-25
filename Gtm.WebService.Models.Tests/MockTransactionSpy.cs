using System;
using System.Threading.Tasks;
using ServiceFabric.Mocks;
using Microsoft.ServiceFabric.Data;

namespace Gtm.WebService.Models.Tests
{
   public class MockTransactionSpy : MockTransaction, ITransaction
   {
      private readonly ITransaction transaction;

      internal MockTransactionSpy(ITransaction transaction)
      {
         this.transaction = transaction;
      }

      public bool IsCommitted { get; private set; }

      public bool IsAborted { get; private set; }

      public bool IsCompleted => IsCommitted || IsAborted;

      Task ITransaction.CommitAsync()
      {
         IsCommitted = true;
         return transaction.CommitAsync();
      }

      void IDisposable.Dispose()
      {
         Dispose();
         IsAborted = true;
      }
   }
}

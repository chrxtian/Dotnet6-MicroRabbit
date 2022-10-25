using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Banking.Data.Context
{
    public class TransferRepository : ITransferRepository
    {
        private readonly TransferDbContext _ctx;

        public TransferRepository(TransferDbContext ctx)
        {
            _ctx = ctx;
        }
        public IEnumerable<TransferLog> GetTransferLogs()
        {
            return _ctx.TransferLogs;
        }
    }
}

using System;

namespace WhizzSchema.Entities
{
    public class TransactionEntry
    {
        public long Id { get; set; }
        public string Command { get; set; }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
        public Guid UserId { get; set; }
        public long Executed { get; set; }
    }
}

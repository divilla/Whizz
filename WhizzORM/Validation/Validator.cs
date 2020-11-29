using WhizzORM.Entities;

namespace WhizzORM.Validation
{
    public class Validator
    {
        public ColumnEntity ColumnEntity { get; }

        public Validator(ColumnEntity columnEntity)
        {
            ColumnEntity = columnEntity;
        }
    }
}

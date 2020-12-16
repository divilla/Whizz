using WhizzJsonRepository.Services;

namespace WhizzApplication
{
    public class QuitePosDb : PgDatabase
    {
        public QuitePosDb(string connectionString)
            : base(connectionString)
        {}
    }
}
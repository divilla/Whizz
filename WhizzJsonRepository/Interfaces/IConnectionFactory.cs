using Npgsql;

namespace WhizzJsonRepository.Interfaces
{
    public interface IConnectionFactory
    {
        IDatabase Database { get; }
        NpgsqlConnection Connection { get; }
        void Dispose();
    }
}
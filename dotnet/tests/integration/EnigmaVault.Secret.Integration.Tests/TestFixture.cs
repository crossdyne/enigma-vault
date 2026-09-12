using EnigmaVault.Secret.Integration.Tests.Contexts;
using EnigmaVault.Secret.Infrastructure.Persistence.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Integration.Tests
{
    public class TestFixture : IDisposable
    {
        private readonly SqliteConnection _connection;
        public TestEnigmaContext DbContext { get; }

        public TestFixture()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<EnigmaContext>()
                .UseSqlite(_connection)
                .Options;

            DbContext = new TestEnigmaContext(options);
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            DbContext.Dispose();
            _connection.Dispose();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Npgsql;
using Npgsql.NameTranslation;
using Npgsql.TypeMapping;
using NpgsqlTypes;
using IsolationLevel = System.Data.IsolationLevel;

namespace WhizzJsonRepository.Interfaces
{
    public interface INpgsqlConnection
    {
        /// <summary>
        /// The connection-specific type mapper - all modifications affect this connection only,
        /// and are lost when it is closed.
        /// </summary>
        INpgsqlTypeMapper TypeMapper { get; }

        /// <summary>
        /// Gets or sets the string used to connect to a PostgreSQL database. See the manual for details.
        /// </summary>
        /// <value>The connection string that includes the server name,
        /// the database name, and other parameters needed to establish
        /// the initial connection. The default value is an empty string.
        /// </value>
#nullable disable
        string ConnectionString
#nullable restore
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the delegate used to generate a password for new database connections.
        /// </summary>
        /// <remarks>
        /// This delegate is executed when a new database connection is opened that requires a password.
        /// <see cref="NpgsqlConnectionStringBuilder.Password">Password</see> and
        /// <see cref="NpgsqlConnectionStringBuilder.Passfile">Passfile</see> connection string
        /// properties have precedence over this delegate. It will not be executed if a password is
        /// specified, or the specified or default Passfile contains a valid entry.
        /// Due to connection pooling this delegate is only executed when a new physical connection
        /// is opened, not when reusing a connection that was previously opened from the pool.
        /// </remarks>
        ProvidePasswordCallback ProvidePasswordCallback { get; set; }

        /// <summary>
        /// Backend server host name.
        /// </summary>
        string? Host { get; }

        /// <summary>
        /// Backend server port.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Gets the time to wait while trying to establish a connection
        /// before terminating the attempt and generating an error.
        /// </summary>
        /// <value>The time (in seconds) to wait for a connection to open. The default value is 15 seconds.</value>
        int ConnectionTimeout { get; }

        /// <summary>
        /// Gets the time to wait while trying to execute a command
        /// before terminating the attempt and generating an error.
        /// </summary>
        /// <value>The time (in seconds) to wait for a command to complete. The default value is 20 seconds.</value>
        int CommandTimeout { get; }

        ///<summary>
        /// Gets the name of the current database or the database to be used after a connection is opened.
        /// </summary>
        /// <value>The name of the current database or the name of the database to be
        /// used after a connection is opened. The default value is the empty string.</value>
        string? Database { get; }

        /// <summary>
        /// Gets the string identifying the database server (host and port)
        /// </summary>
        string DataSource { get; }

        /// <summary>
        /// Whether to use Windows integrated security to log in.
        /// </summary>
        bool IntegratedSecurity { get; }

        /// <summary>
        /// User name.
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        /// <value>A bitwise combination of the <see cref="System.Data.ConnectionState">ConnectionState</see> values. The default is <b>Closed</b>.</value>
        ConnectionState FullState { get; }

        /// <summary>
        /// Gets whether the current state of the connection is Open or Closed
        /// </summary>
        /// <value>ConnectionState.Open, ConnectionState.Closed or ConnectionState.Connecting</value>
        ConnectionState State { get; }

        /// <summary>
        /// Selects the local Secure Sockets Layer (SSL) certificate used for authentication.
        /// </summary>
        /// <remarks>
        /// See <see href="https://msdn.microsoft.com/en-us/library/system.net.security.localcertificateselectioncallback(v=vs.110).aspx"/>
        /// </remarks>
        ProvideClientCertificatesCallback? ProvideClientCertificatesCallback { get; set; }

        /// <summary>
        /// Verifies the remote Secure Sockets Layer (SSL) certificate used for authentication.
        /// Ignored if <see cref="NpgsqlConnectionStringBuilder.TrustServerCertificate"/> is set.
        /// </summary>
        /// <remarks>
        /// See <see href="https://msdn.microsoft.com/en-us/library/system.net.security.remotecertificatevalidationcallback(v=vs.110).aspx"/>
        /// </remarks>
        RemoteCertificateValidationCallback? UserCertificateValidationCallback { get; set; }

        /// <summary>
        /// Version of the PostgreSQL backend.
        /// This can only be called when there is an active connection.
        /// </summary>
        Version PostgreSqlVersion { get; }

        /// <summary>
        /// PostgreSQL server version.
        /// </summary>
        string ServerVersion { get; }

        /// <summary>
        /// Process id of backend server.
        /// This can only be called when there is an active connection.
        /// </summary>
// ReSharper disable once InconsistentNaming
        int ProcessID { get; }

        /// <summary>
        /// Reports whether the backend uses the newer integer timestamp representation.
        /// Note that the old floating point representation is not supported.
        /// Meant for use by type plugins (e.g. NodaTime)
        /// </summary>
        bool HasIntegerDateTimes { get; }

        /// <summary>
        /// The connection's timezone as reported by PostgreSQL, in the IANA/Olson database format.
        /// </summary>
        string Timezone { get; }

        /// <summary>
        /// Holds all PostgreSQL parameters received for this connection. Is updated if the values change
        /// (e.g. as a result of a SET command).
        /// </summary>
        IReadOnlyDictionary<string, string> PostgresParameters { get; }

        IContainer Container { get; }
        ISite Site { get; set; }

        /// <summary>
        /// Opens a database connection with the property settings specified by the
        /// <see cref="NpgsqlConnection.ConnectionString">ConnectionString</see>.
        /// </summary>
        void Open();

        /// <summary>
        /// This is the asynchronous version of <see cref="NpgsqlConnection.Open"/>.
        /// </summary>
        /// <remarks>
        /// Do not invoke other methods and properties of the <see cref="NpgsqlConnection"/> object until the returned Task is complete.
        /// </remarks>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task OpenAsync(CancellationToken cancellationToken);

#nullable disable
        void EnlistTransaction(Transaction transaction)
#nullable restore
            ;

        /// <summary>
        /// Releases the connection. If the connection is pooled, it will be returned to the pull and made available for re-use.
        /// If it is non-pooled, the physical connection will be closed.
        /// </summary>
        void Close();

        /// <summary>
        /// Releases the connection. If the connection is pooled, it will be returned to the pull and made available for re-use.
        /// If it is non-pooled, the physical connection will be closed.
        /// </summary>
#if !NET461 && !NETSTANDARD2_0
        Task CloseAsync()
#else
        public Task CloseAsync()
#endif
            ;

        /// <summary>
        /// Releases all resources used by the <see cref="NpgsqlConnection">NpgsqlConnection</see>.
        /// </summary>
#if !NET461 && !NETSTANDARD2_0
        ValueTask DisposeAsync()
#else
        public async ValueTask DisposeAsync()
#endif
            ;

        /// <summary>
        /// Fires when PostgreSQL notices are received from PostgreSQL.
        /// </summary>
        /// <remarks>
        /// PostgreSQL notices are non-critical messages generated by PostgreSQL, either as a result of a user query
        /// (e.g. as a warning or informational notice), or due to outside activity (e.g. if the database administrator
        /// initiates a "fast" database shutdown).
        ///
        /// Note that notices are very different from notifications (see the <see cref="NpgsqlConnection.Notification"/> event).
        /// </remarks>
        event NoticeEventHandler? Notice;

        /// <summary>
        /// Fires when PostgreSQL notifications are received from PostgreSQL.
        /// </summary>
        /// <remarks>
        /// PostgreSQL notifications are sent when your connection has registered for notifications on a specific channel via the
        /// LISTEN command. NOTIFY can be used to generate such notifications, allowing for an inter-connection communication channel.
        ///
        /// Note that notifications are very different from notices (see the <see cref="NpgsqlConnection.Notice"/> event).
        /// </remarks>
        event NotificationEventHandler? Notification;

        /// <summary>
        /// Begins a binary COPY FROM STDIN operation, a high-performance data import mechanism to a PostgreSQL table.
        /// </summary>
        /// <param name="copyFromCommand">A COPY FROM STDIN SQL command</param>
        /// <returns>A <see cref="NpgsqlBinaryImporter"/> which can be used to write rows and columns</returns>
        /// <remarks>
        /// See http://www.postgresql.org/docs/current/static/sql-copy.html.
        /// </remarks>
        NpgsqlBinaryImporter BeginBinaryImport(string copyFromCommand);

        /// <summary>
        /// Begins a binary COPY TO STDOUT operation, a high-performance data export mechanism from a PostgreSQL table.
        /// </summary>
        /// <param name="copyToCommand">A COPY TO STDOUT SQL command</param>
        /// <returns>A <see cref="NpgsqlBinaryExporter"/> which can be used to read rows and columns</returns>
        /// <remarks>
        /// See http://www.postgresql.org/docs/current/static/sql-copy.html.
        /// </remarks>
        NpgsqlBinaryExporter BeginBinaryExport(string copyToCommand);

        /// <summary>
        /// Begins a textual COPY FROM STDIN operation, a data import mechanism to a PostgreSQL table.
        /// It is the user's responsibility to send the textual input according to the format specified
        /// in <paramref name="copyFromCommand"/>.
        /// </summary>
        /// <param name="copyFromCommand">A COPY FROM STDIN SQL command</param>
        /// <returns>
        /// A TextWriter that can be used to send textual data.</returns>
        /// <remarks>
        /// See http://www.postgresql.org/docs/current/static/sql-copy.html.
        /// </remarks>
        TextWriter BeginTextImport(string copyFromCommand);

        /// <summary>
        /// Begins a textual COPY TO STDOUT operation, a data export mechanism from a PostgreSQL table.
        /// It is the user's responsibility to parse the textual input according to the format specified
        /// in <paramref name="copyToCommand"/>.
        /// </summary>
        /// <param name="copyToCommand">A COPY TO STDOUT SQL command</param>
        /// <returns>
        /// A TextReader that can be used to read textual data.</returns>
        /// <remarks>
        /// See http://www.postgresql.org/docs/current/static/sql-copy.html.
        /// </remarks>
        TextReader BeginTextExport(string copyToCommand);

        /// <summary>
        /// Begins a raw binary COPY operation (TO STDOUT or FROM STDIN), a high-performance data export/import mechanism to a PostgreSQL table.
        /// Note that unlike the other COPY API methods, <see cref="NpgsqlConnection.BeginRawBinaryCopy"/> doesn't implement any encoding/decoding
        /// and is unsuitable for structured import/export operation. It is useful mainly for exporting a table as an opaque
        /// blob, for the purpose of importing it back later.
        /// </summary>
        /// <param name="copyCommand">A COPY TO STDOUT or COPY FROM STDIN SQL command</param>
        /// <returns>A <see cref="NpgsqlRawCopyStream"/> that can be used to read or write raw binary data.</returns>
        /// <remarks>
        /// See http://www.postgresql.org/docs/current/static/sql-copy.html.
        /// </remarks>
        NpgsqlRawCopyStream BeginRawBinaryCopy(string copyCommand);

        /// <summary>
        /// Maps a CLR enum to a PostgreSQL enum type for use with this connection.
        /// </summary>
        /// <remarks>
        /// CLR enum labels are mapped by name to PostgreSQL enum labels.
        /// The translation strategy can be controlled by the <paramref name="nameTranslator"/> parameter,
        /// which defaults to <see cref="NpgsqlSnakeCaseNameTranslator"/>.
        /// You can also use the <see cref="PgNameAttribute"/> on your enum fields to manually specify a PostgreSQL enum label.
        /// If there is a discrepancy between the .NET and database labels while an enum is read or written,
        /// an exception will be raised.
        ///
        /// Can only be invoked on an open connection; if the connection is closed the mapping is lost.
        ///
        /// To avoid mapping the type for each connection, use the <see cref="NpgsqlConnection.MapEnumGlobally{TEnum}"/> method.
        /// </remarks>
        /// <param name="pgName">
        /// A PostgreSQL type name for the corresponding enum type in the database.
        /// If null, the name translator given in <paramref name="nameTranslator"/>will be used.
        /// </param>
        /// <param name="nameTranslator">
        /// A component which will be used to translate CLR names (e.g. SomeClass) into database names (e.g. some_class).
        /// Defaults to <see cref="NpgsqlSnakeCaseNameTranslator"/>
        /// </param>
        /// <typeparam name="TEnum">The .NET enum type to be mapped</typeparam>
        void MapEnum<TEnum>(string? pgName = null, INpgsqlNameTranslator? nameTranslator = null)
            where TEnum : struct, Enum;

        /// <summary>
        /// Maps a CLR type to a PostgreSQL composite type for use with this connection.
        /// </summary>
        /// <remarks>
        /// CLR fields and properties by string to PostgreSQL enum labels.
        /// The translation strategy can be controlled by the <paramref name="nameTranslator"/> parameter,
        /// which defaults to <see cref="NpgsqlSnakeCaseNameTranslator"/>.
        /// You can also use the <see cref="PgNameAttribute"/> on your members to manually specify a PostgreSQL enum label.
        /// If there is a discrepancy between the .NET and database labels while a composite is read or written,
        /// an exception will be raised.
        ///
        /// Can only be invoked on an open connection; if the connection is closed the mapping is lost.
        ///
        /// To avoid mapping the type for each connection, use the <see cref="NpgsqlConnection.MapCompositeGlobally{T}"/> method.
        /// </remarks>
        /// <param name="pgName">
        /// A PostgreSQL type name for the corresponding enum type in the database.
        /// If null, the name translator given in <paramref name="nameTranslator"/>will be used.
        /// </param>
        /// <param name="nameTranslator">
        /// A component which will be used to translate CLR names (e.g. SomeClass) into database names (e.g. some_class).
        /// Defaults to <see cref="NpgsqlSnakeCaseNameTranslator"/>
        /// </param>
        /// <typeparam name="T">The .NET type to be mapped</typeparam>
        void MapComposite<T>(string? pgName = null, INpgsqlNameTranslator? nameTranslator = null) where T : new();

        /// <summary>
        /// Waits until an asynchronous PostgreSQL messages (e.g. a notification) arrives, and
        /// exits immediately. The asynchronous message is delivered via the normal events
        /// (<see cref="NpgsqlConnection.Notification"/>, <see cref="NpgsqlConnection.Notice"/>).
        /// </summary>
        /// <param name="timeout">
        /// The time-out value, in milliseconds, passed to <see cref="Socket.ReceiveTimeout"/>.
        /// The default value is 0, which indicates an infinite time-out period.
        /// Specifying -1 also indicates an infinite time-out period.
        /// </param>
        /// <returns>true if an asynchronous message was received, false if timed out.</returns>
        bool Wait(int timeout);

        /// <summary>
        /// Waits until an asynchronous PostgreSQL messages (e.g. a notification) arrives, and
        /// exits immediately. The asynchronous message is delivered via the normal events
        /// (<see cref="NpgsqlConnection.Notification"/>, <see cref="NpgsqlConnection.Notice"/>).
        /// </summary>
        /// <param name="timeout">
        /// The time-out value is passed to <see cref="Socket.ReceiveTimeout"/>.
        /// </param>
        /// <returns>true if an asynchronous message was received, false if timed out.</returns>
        bool Wait(TimeSpan timeout);

        /// <summary>
        /// Waits until an asynchronous PostgreSQL messages (e.g. a notification) arrives, and
        /// exits immediately. The asynchronous message is delivered via the normal events
        /// (<see cref="NpgsqlConnection.Notification"/>, <see cref="NpgsqlConnection.Notice"/>).
        /// </summary>
        void Wait();

        /// <summary>
        /// Waits asynchronously until an asynchronous PostgreSQL messages (e.g. a notification)
        /// arrives, and exits immediately. The asynchronous message is delivered via the normal events
        /// (<see cref="NpgsqlConnection.Notification"/>, <see cref="NpgsqlConnection.Notice"/>).
        /// </summary>
        Task WaitAsync();

        /// <summary>
        /// Waits asynchronously until an asynchronous PostgreSQL messages (e.g. a notification)
        /// arrives, and exits immediately. The asynchronous message is delivered via the normal events
        /// (<see cref="NpgsqlConnection.Notification"/>, <see cref="NpgsqlConnection.Notice"/>).
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        Task WaitAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns the supported collections
        /// </summary>
        DataTable GetSchema();

        /// <summary>
        /// Returns the schema collection specified by the collection name.
        /// </summary>
        /// <param name="collectionName">The collection name.</param>
        /// <returns>The collection specified.</returns>
        DataTable GetSchema(string? collectionName);

        /// <summary>
        /// Returns the schema collection specified by the collection name filtered by the restrictions.
        /// </summary>
        /// <param name="collectionName">The collection name.</param>
        /// <param name="restrictions">
        /// The restriction values to filter the results.  A description of the restrictions is contained
        /// in the Restrictions collection.
        /// </param>
        /// <returns>The collection specified.</returns>
        DataTable GetSchema(string? collectionName, string?[]? restrictions);

        /// <summary>
        /// Clones this connection, replacing its connection string with the given one.
        /// This allows creating a new connection with the same security information
        /// (password, SSL callbacks) while changing other connection parameters (e.g.
        /// database or pooling)
        /// </summary>
        NpgsqlConnection CloneWith(string connectionString);

        /// <summary>
        /// This method changes the current database by disconnecting from the actual
        /// database and connecting to the specified.
        /// </summary>
        /// <param name="dbName">The name of the database to use in place of the current database.</param>
        void ChangeDatabase(string dbName);

        /// <summary>
        /// Unprepares all prepared statements on this connection.
        /// </summary>
        void UnprepareAll();

        /// <summary>
        /// Flushes the type cache for this connection's connection string and reloads the types for this connection only.
        /// Type changes will appear for other connections only after they are re-opened from the pool.
        /// </summary>
        void ReloadTypes();

        DbTransaction BeginTransaction();
        DbTransaction BeginTransaction(IsolationLevel isolationLevel);
        Task ChangeDatabaseAsync(string databaseName, CancellationToken cancellationToken);
        DbCommand CreateCommand();
        Task OpenAsync();
        ValueTask<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
        ValueTask<DbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken);
        event StateChangeEventHandler StateChange;
        void Dispose();
        string ToString();
        event EventHandler Disposed;
        object GetLifetimeService();
        object InitializeLifetimeService();
    }
}
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Nop.Core;

namespace Nop.Data
{
    /// <summary>
    /// Represents a data provider
    /// </summary>
    public partial interface INopDataProvider
    {
        #region Methods

        /// <summary>
        /// Create the database
        /// </summary>
        /// <param name="collation">Collation</param>
        /// <param name="triesToConnect">Count of tries to connect to the database after creating; set 0 if no need to connect after creating</param>
        void CreateDatabase(string collation, int triesToConnect = 10);

        /// <summary>
        /// Creates a connection to a database
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>Connection to a database</returns>
        IDbConnection CreateDbConnection(string connectionString);

        /// <summary>
        /// Creates a new temporary storage and populate it using data from provided query
        /// </summary>
        /// <param name="storeKey">Name of temporary storage</param>
        /// <param name="query">Query to get records to populate created storage with initial data</param>
        /// <typeparam name="TItem">Storage record mapping class</typeparam>
        /// <returns>IQueryable instance of temporary storage</returns>
        ITempDataStorage<TItem> CreateTempDataStorage<TItem>(string storageKey, IQueryable<TItem> query)
            where TItem : class;

        /// <summary>
        /// Initialize database
        /// </summary>
        void InitializeDatabase();

        /// <summary>
        /// Insert a new entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        TEntity InsertEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

        /// <summary>
        /// Updates record in table, using values from entity parameter. 
        /// Record to update identified by match on primary key value from obj value.
        /// </summary>
        /// <param name="entity">Entity with data to update</param>
        /// <typeparam name="TEntity">Entity type</typeparam>
        void UpdateEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

        /// <summary>
        /// Deletes record in table. Record to delete identified
        /// by match on primary key value from obj value.
        /// </summary>
        /// <param name="entity">Entity for delete operation</param>
        /// <typeparam name="TEntity">Entity type</typeparam>
        void DeleteEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

        /// <summary>
        /// Performs delete records in a table
        /// </summary>
        /// <param name="entities">Entities for delete operation</param>
        /// <typeparam name="TEntity">Entity type</typeparam>
        void BulkDeleteEntities<TEntity>(IList<TEntity> entities) where TEntity : BaseEntity;

        /// <summary>
        /// Performs delete records in a table by a condition
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>Number of deleted records</returns>
        int BulkDeleteEntities<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;

        /// <summary>
        /// Performs bulk insert entities operation
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="entities">Collection of Entities</param>
        void BulkInsertEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;

        /// <summary>
        /// Gets the name of a foreign key
        /// </summary>
        /// <param name="foreignTable">Foreign key table</param>
        /// <param name="foreignColumn">Foreign key column name</param>
        /// <param name="primaryTable">Primary table</param>
        /// <param name="primaryColumn">Primary key column name</param>
        /// <returns>Name of a foreign key</returns>
        string CreateForeignKeyName(string foreignTable, string foreignColumn, string primaryTable, string primaryColumn);

        /// <summary>
        /// Gets the name of an index
        /// </summary>
        /// <param name="targetTable">Target table name</param>
        /// <param name="targetColumn">Target column name</param>
        /// <returns>Name of an index</returns>
        string GetIndexName(string targetTable, string targetColumn);

        /// <summary>
        /// Returns queryable source for specified mapping class for current connection,
        /// mapped to database table or view.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>Queryable source</returns>
        IQueryable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// Get the current identity value
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <returns>Integer identity; null if cannot get the result</returns>
        int? GetTableIdent<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// Checks if the specified database exists, returns true if database exists
        /// </summary>
        /// <returns>Returns true if the database exists.</returns>
        bool DatabaseExists();

        /// <summary>
        /// Creates a backup of the database
        /// </summary>
        void BackupDatabase(string fileName);

        /// <summary>
        /// Restores the database from a backup
        /// </summary>
        /// <param name="backupFileName">The name of the backup file</param>
        void RestoreDatabase(string backupFileName);

        /// <summary>
        /// Re-index database tables
        /// </summary>
        void ReIndexTables();

        /// <summary>
        /// Build the connection string
        /// </summary>
        /// <param name="nopConnectionString">Connection string info</param>
        /// <returns>Connection string</returns>
        string BuildConnectionString(INopConnectionStringInfo nopConnectionString);

        /// <summary>
        /// Set table identity (is supported)
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="ident">Identity value</param>
        void SetTableIdent<TEntity>(int ident) where TEntity : BaseEntity;

        /// <summary>
        /// Truncates database table
        /// </summary>
        /// <param name="resetIdentity">Performs reset identity column</param>
        void Truncate<TEntity>(bool resetIdentity = false) where TEntity : BaseEntity;

        /// <summary>
        /// Get hash values of a stored entity field
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="keySelector">A key selector which should project to a dictionary key</param>
        /// <param name="fieldSelector">A field selector to apply a transform to a hash value</param>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>Dictionary</returns>
        IDictionary<int, string> GetFieldHashes<TEntity>(Expression<Func<TEntity, bool>> predicate, 
            Expression<Func<TEntity, int>> keySelector,
            Expression<Func<TEntity, object>> fieldSelector) where TEntity : BaseEntity;

        #endregion

        #region Properties

        /// <summary>
        /// Name of database provider
        /// </summary>
        string ConfigurationName { get; }

        /// <summary>
        /// Gets allowed a limit input value of the data for hashing functions, returns 0 if not limited
        /// </summary>
        int SupportedLengthOfBinaryHash { get; }

        /// <summary>
        /// Gets a value indicating whether this data provider supports backup
        /// </summary>
        bool BackupSupported { get; }

        #endregion
    }
}
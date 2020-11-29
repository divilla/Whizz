using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WhizzORM.Schema;

namespace WhizzSchema.Schema
{
    public class RelationSchema
    {
        public DbSchema DbSchema { get; }
        public string SchemaName { get; }
        public string RelationName { get; }
        public string RelationOwner { get; }
        public string Kind { get; }
        public bool CanSelect { get; }
        public bool CanInsert { get; }
        public bool CanUpdate { get; }
        public bool CanDelete { get; }
        public ImmutableDictionary<string, ColumnSchema> Columns { get; }
        public ImmutableArray<string> ColumnNames { get; }
        public ImmutableArray<string> PrimaryKeyColumns { get; }
        public ImmutableArray<ForeignKeySchema> ForeignKeyColumns { get; }
        public ImmutableArray<UniqueIndexSchema> UniqueIndexes { get; }

        public RelationSchema(DbSchema dbSchema, string relationName, string schemaName)
        {
            var relationEntity =
                dbSchema.RelationEntities.SingleOrDefault(s => s.SchemaName == schemaName && s.RelationName == relationName);
            
            DbSchema = dbSchema;
            SchemaName = schemaName;
            RelationName = relationName;
            RelationOwner = relationEntity.RelationOwner;
            Kind = relationEntity.RelationKind;
            CanSelect = relationEntity.CanSelect;
            CanInsert = relationEntity.CanInsert;
            CanUpdate = relationEntity.CanUpdate;
            CanDelete = relationEntity.CanDelete;
            
            Columns = dbSchema.ColumnEntities
                .Where(q => q.SchemaName == SchemaName && q.RelationName == RelationName)
                .ToDictionary(
                    k => k.ColumnName, 
                    s => new ColumnSchema(
                        s.SchemaName, 
                        s.RelationName, 
                        s.ColumnName, 
                        s.Position, 
                        s.TypeOid, 
                        s.DataType, 
                        s.TypeType,
                        s.Size,
                        s.Modifier,
                        s.Dimension,
                        s.CharacterMaximumLength, 
                        s.NumericPrecision,
                        s.NumericScale,
                        s.EnumValues,
                        s.DefaultValue, 
                        s.IsNotNull,
                        s.IsGenerated, 
                        s.IsPrimaryKey,
                        s.IsRequired,
                        s.IsUpdatable,
                        s.ColumnComment, 
                        dbSchema.TypeEntities.ContainsKey(s.TypeOid) ? dbSchema.TypeEntities[s.TypeOid].Type : null))
                .ToImmutableDictionary();
            
            ColumnNames = dbSchema.ColumnEntities
                .Where(q => q.SchemaName == SchemaName && q.RelationName == RelationName)
                .Select(s => s.ColumnName)
                .ToImmutableArray();
            
            PrimaryKeyColumns = dbSchema.ColumnEntities
                .Where(q => q.SchemaName == SchemaName && q.RelationName == RelationName && q.IsPrimaryKey)
                .Select(s => s.ColumnName)
                .ToImmutableArray();
            
            ForeignKeyColumns = dbSchema.ForeignKeyEntities
                .Where(q => q.SchemaName == SchemaName && q.TableName == RelationName)
                .Select(s => new ForeignKeySchema(s.SchemaName, s.TableName, s.ColumnName, s.ConstraintName, s.PrimaryKeySchemaName, s.PrimaryKeyTableName, s.PrimaryKeyColumnName))
                .ToImmutableArray();

            UniqueIndexes = dbSchema.UniqueIndexEntities.Where(q => q.TableName == RelationName && q.SchemaName == SchemaName)
                .Select(s => new UniqueIndexSchema(s.SchemaName, s.TableName, s.IndexName, s.ColumnNames))
                .ToArray()
                .ToImmutableArray();
        }

        public string Quote(string value)
        {
            return DbSchema.Quote(value);
        }
    }
}

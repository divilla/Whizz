using System.Linq;
using System.Text;
using WhizzBase.Extensions;
using WhizzBuilder.Base;
using WhizzSchema;
using WhizzSchema.Interfaces;

namespace WhizzBuilder
{
    public class Application
    {
        public static string NewLineChar => _newLineCharacter;
        private static string _newLineCharacter;
        
        private Config _config;
        private IDbSchema _schema;

        public Application(Config config, IDbSchema schema)
        {
            _config = config;
            _schema = schema;
            _newLineCharacter = config.NewLineCharacter;
        }

        public void Build()
        {
            var result = Builder.CreateFile(f =>
            {
                f.SetTargetFolder(_config.TargetFolder)
                    .SetFilename(_config.Filename);

                f.AddUsing("System");
                f.AddUsing("WhizzBase.Attributes");
                
                if (_config.BaseEntityClassNamespace != null)
                    f.AddUsing(_config.BaseEntityClassNamespace);
                
                foreach (var usingNamespace in _schema.UsingNamespaces)
                    f.AddUsing(usingNamespace);
                
                f.AddNamespace(n =>
                {
                    n.Name(_config.Namespace);

                    foreach (var schema in _schema.Schemas.Values)
                    {
                        foreach (var relation in schema.Relations.Values)
                        {
                            n.AddClass(c =>
                            {
                                if (relation.Kind == "v" || relation.Kind == "m")
                                    c.AddViewAttribute(_schema.GetEscapedRelationName(relation.RelationName, relation.SchemaName));
                                else
                                    c.AddTableAttribute(_schema.GetEscapedRelationName(relation.RelationName, relation.SchemaName));
                                
                                c.Name(relation.SchemaName == DbSchema.DefaultSchema
                                    ? $"{relation.RelationName.ToPascalCase()}"
                                    : $"{relation.SchemaName.ToPascalCase()}{relation.RelationName.ToPascalCase()}");

                                if (_config.AddPartialModifier)
                                    c.ClassModifiers(m => m.Partial);

                                if (_config.BaseEntityClass != null)
                                    c.AddBaseClass(_config.BaseEntityClass);

                                foreach (var column in relation.Columns.Values.OrderBy(o => o.Position))
                                {
                                    c.AddProperty(p =>
                                    {
                                        if (relation.PrimaryKeyColumns.Length == 1 && column.IsPrimaryKey)
                                            p.AddPrimaryKeyAttribute();
                                        else if (relation.PrimaryKeyColumns.Length > 1 && column.IsPrimaryKey)
                                            p.AddCompositePrimaryKeyAttribute();

                                        if (column.IsRequired && relation.Kind != "v" && relation.Kind != "m")
                                            p.AddRequiredAttribute();
                                        else if (column.IsReadOnly || relation.Kind == "v" || relation.Kind == "m")
                                            p.AddReadonlyAttribute();

                                        foreach (var foreignKey in relation.ForeignKeyColumns
                                            .Where(q => q.ColumnName == column.ColumnName))
                                        {
                                            p.AddForeignKeyAttribute(
                                                _schema.GetEscapedRelationName(foreignKey.PrimaryKeyTableName, foreignKey.PrimaryKeySchemaName),
                                                _schema.EscapedQuote(foreignKey.PrimaryKeyColumnName));
                                        }

                                        foreach (var uniqueIndex in relation.UniqueIndexes
                                            .Where(q => q.ColumnNames.Contains(column.ColumnName)))
                                        {
                                            p.AddUniqueIndexAttribute(_schema.EscapedQuote(uniqueIndex.IndexName));
                                        }

                                        p.AddColumnAttribute(_schema.EscapedQuote(column.ColumnName), column.Position);

                                        p.Name(column.ColumnName.ToPascalCase());

                                        p.Type(column.ClrType != null 
                                            ? column.ClrType.ToKeywordName().AddArrayDimension(column.Dimension) 
                                            : typeof(string).ToKeywordName());
                                        
                                        if (_config.MarkDirty && relation.Kind != "v" && relation.Kind != "m")
                                            p.MarkDirty();
                                    });
                                }
                            });
                        }
                    }
                });
            });
            
            if (!string.IsNullOrEmpty(_config.TargetFolder) 
                && !string.IsNullOrEmpty(_config.Filename) 
                && System.IO.Directory.Exists(_config.TargetFolder))
                System.IO.File.WriteAllText($"{_config.TargetFolder}/{_config.Filename}", result, Encoding.UTF8);
        }
    }
}
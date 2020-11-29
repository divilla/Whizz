using System;
using System.Linq;
using Utf8Json;
using WhizzBase.Base;
using WhizzBase.Extensions;
using WhizzBuilder.Base;
using WhizzSchema;
using WhizzSchema.Interfaces;

namespace WhizzBuilder
{
    static class Program
    {
        public static Config Config { get; private set; }
        public static IDbSchema Schema { get; private set; }
        
        static void Main(string[] args)
        {
            if (args.Length == 0)
                throw new DbException("Path to config.json must be specified as parameter at runtime.");
            if (!System.IO.File.Exists(args[0]))
                throw new DbException("File config.json does not exist.");

            using (var configFileStream = System.IO.File.OpenRead(args[0]))
            {
                Config = JsonSerializer.Deserialize<Config>(configFileStream);
            }

            Config.TargetFolder = string.IsNullOrEmpty(Config.TargetFolder)
                ? System.IO.Directory.GetCurrentDirectory()
                : Config.TargetFolder;

            Schema = new DbSchema(Config.ConnectionString);
            
            var result = Builder.CreateFile(f =>
            {
                f.SetTargetFolder(Config.TargetFolder)
                    .SetFilename(Config.Filename);

                if (Config.BaseEntityClassNamespace != null)
                    f.AddUsing(Config.BaseEntityClassNamespace);
                
                foreach (var usingNamespace in Schema.UsingNamespaces)
                    f.AddUsing(usingNamespace);
                
                f.AddNamespace(n =>
                {
                    n.Name(Config.Namespace);

                    foreach (var schema in Schema.Schemas.Values)
                    {
                        foreach (var relation in schema.Relations.Values)
                        {
                            n.AddClass(c =>
                            {
                                c.AddTableAttribute(Schema.GetQualifiedRelationName(relation.RelationName, relation.SchemaName));
                                
                                c.Name(relation.SchemaName == DbSchema.DefaultSchema
                                    ? $"{relation.RelationName.ToPascalCase()}"
                                    : $"{relation.SchemaName.ToPascalCase()}{relation.RelationName.ToPascalCase()}");

                                if (Config.AddPartialModifier)
                                    c.ClassModifiers(m => m.Partial);

                                if (Config.BaseEntityClass != null)
                                    c.AddBaseClass(Config.BaseEntityClass);

                                foreach (var column in relation.Columns.Values.OrderBy(o => o.Position))
                                {
                                    c.AddProperty(p =>
                                    {
                                        p.Name(column.ColumnName.ToPascalCase());

                                        p.Type(column.ClrType != null 
                                            ? column.ClrType.ToKeywordName().AddArrayDimension(column.Dimension) 
                                            : typeof(string).ToKeywordName());
                                        
                                        if (Config.MarkDirty && relation.Kind != "v" && relation.Kind != "m")
                                            p.MarkDirty();
                                    });
                                }
                            });
                        }
                    }

                    n.AddClass(c => c.Name("TestEntity")
                            .AddTableAttribute("test")
                            .ClassModifiers(m => m.Partial)
                            .AddProperty(p => p.Name("Id")
                                .AddPrimaryKeyAttribute()
                                .AddCompositePrimaryKeyAttribute()
                                .AddForeignKeyAttribute("table", "column")
                                .AddUniqueIndexAttribute("uniqueIndexName")
                                .AddColumnAttribute("id", 1)
                                .Type("Guid")
                                .MarkDirty())
                            .AddProperty(p => p.Name("Name")
                                .AddColumnAttribute("name", 2)
                                .Type("string")
                                .MarkDirty())
                        )
                        .AddClass(c => c.Name("Test2Entity")
                            .AddTableAttribute("test")
                            .ClassModifiers(m => m.Partial));
                });
            });
            Console.WriteLine(result);
        }
    }
}
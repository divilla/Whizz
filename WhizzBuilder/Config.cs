namespace WhizzBuilder
{
    public class Config
    {
        public string ConnectionString { get; set; }
        public string TargetFolder { get; set; }
        public string Filename { get; set; }
        public string NewLineCharacter { get; set; }
        public string Namespace { get; set; }
        public bool AddPartialModifier { get; set; }
        public bool MarkDirty { get; set; }
        public string BaseEntityClass { get; set; }
        public string BaseEntityClassNamespace { get; set; }
    }
}

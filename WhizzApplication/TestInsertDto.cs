using System;

namespace WhizzApplication
{
    public class TestInsertDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string LongDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Money { get; set; }
        public DateTime DateTime { get; set; }
        public bool Active { get; set; }
    }
}
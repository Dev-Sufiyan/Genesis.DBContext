using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Repository.Tests
{
    public class TestModel
    {
        public int IntField { get; set; }
        public long LongField { get; set; }
        public string StringField { get; set; } = string.Empty;
        public DateTime DateTimeField { get; set; }
        public bool BoolField { get; set; }
        public double DoubleField { get; set; }
        public decimal DecimalField { get; set; }
    }
}

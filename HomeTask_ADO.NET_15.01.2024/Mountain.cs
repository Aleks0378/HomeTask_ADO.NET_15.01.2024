using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask_ADO.NET_15._01._2024
{
    internal class Mountain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Height: {Height}, Country: {Country}";
        }
    }
}

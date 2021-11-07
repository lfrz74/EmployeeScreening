using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeScreening.Models
{
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public Decimal longitude { get; set; }
        public Decimal latitude { get; set; }
    }
}

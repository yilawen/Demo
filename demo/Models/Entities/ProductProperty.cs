using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models.Entities
{
    [Table("ProductProperty", Schema = "dbo")]
    public class ProductProperty
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public string Description { get; set; }
        public string UseWay { get; set; }
        public string DefineMethod { get; set; }
        public bool Status { get; set; }
        public bool Enable { get; set; }
    }
}
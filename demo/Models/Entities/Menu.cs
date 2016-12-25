using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace demo.Utilities.Entities
{
    [Table("Menus", Schema = "dbo")]
    public class Menu
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string MenuName { get; set; }
        public string LinkUrl { get; set; }
        public bool Status { get; set; }
        public int ParentId { get; set; }
        public int? Sort { get; set; }
    }
}
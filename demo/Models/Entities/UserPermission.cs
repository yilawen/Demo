using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wood.Models.Entities
{
    [Table("UserPermissions", Schema = "dbo")]
    public class UserPermission
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string UserId { get; set; }
    }
}
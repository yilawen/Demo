using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace demo.Models.Entities
{
    [Table("Users", Schema = "dbo")]
    public class User
    {
        public string Id { get; set; } //生成Guid字符串
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string LastLoginIP { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int RoleGrade { get; set; }
        public int Status { get; set; }
    }
}
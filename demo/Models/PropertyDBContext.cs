using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using Wood.Utilities;
using System.Data;
using Wood.Models.Entities;

namespace Wood.Models
{
    public class PropertyDBContext : DbContext
    {
        public DbSet<ProductProperty> ProductProperties { get; set; }

        public List<ProductProperty> GetAllProperties()
        {
            return ProductProperties.ToList();
        }

        public int GetPropertiesAmount()
        {
            return ProductProperties.Count();
        }

        public List<ProductProperty> GetProperties(int offset, int amount)
        {
            int start = --offset * amount;
            return ProductProperties.OrderBy(p => p.Id).Skip(start).Take(amount).ToList();
        }

        public void UpdateProperty(ProductProperty property)
        {
            ProductProperties.Attach(property);
            Entry(property).Property("Status").IsModified = true;
            SaveChanges();
        }
    }
}
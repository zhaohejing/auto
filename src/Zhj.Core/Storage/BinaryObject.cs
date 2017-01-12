using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace MyCompanyName.AbpZeroTemplate.Storage
{
    [Table("AppBinaryObjects")]
    public class BinaryObject : Entity<Guid>
    {
        [Required]
        public string Url { get; set; }
      

        public BinaryObject()
        {
            Id = Guid.NewGuid();
        }

        public BinaryObject(string url)
            : this()
        {
            Url = url;
        }
   
        public BinaryObject(Guid id, string url) {
            Id = id;
            Url = url;
        
        }
    }
}

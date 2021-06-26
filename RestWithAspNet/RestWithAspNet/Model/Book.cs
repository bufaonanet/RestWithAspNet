using RestWithAspNet.Model.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithAspNet.Model
{
    [Table("book")]
    public class Book : Entity
    {    

        [Column("title")]
        public string Title { get; set; }

        [Column("author")]
        public string Author { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("launch_date")]
        public DateTime LaunchDate { get; set; }
    }
}

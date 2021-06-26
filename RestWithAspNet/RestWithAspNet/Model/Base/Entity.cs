using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithAspNet.Model.Base
{
    public class Entity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}

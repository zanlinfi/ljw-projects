using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EntityClass
{
    [Table("TBook")]   //表名
    public class Book
    {
        [Key]    //主键
        [Column("Id")]  //列名
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //主键自增
        //[JsonIgnore]
        public int Id { get; set; }
        [Column("Title")]

        public string? Title { get; set; }
        [Column("Price")]
        public double? Price { get; set; }
        [Column("AuthorName")]
        public string? AuthorName { get; set; }


    }
}
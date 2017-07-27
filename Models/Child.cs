using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BagAPI.Models
{
  public class Child
  {
    public Child()
    {
      Delivered = 0;
    }

    [Key]
    public int ChildId { get; set; }

    [Required]
    public string Name { get; set; }
    public int Delivered { get; set; }
    public virtual ICollection<Toy> Toys { get; set; }
    public virtual ICollection<FavoriteReindeer> Favorites { get; set; }

  }
}

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BagAPI.Models
{
  public class Reindeer
  {
    [Key]
    public int ReindeerId { get; set; }

    [Required]
    public string Name { get; set; }

    public virtual ICollection<FavoriteReindeer> Favorites { get; set; }
  }
}

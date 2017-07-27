using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BagAPI.Models
{
  public class FavoriteReindeer
  {
    [Key]
    public int FavoriteReindeerId { get; set; }

    [Required]
    public int ReindeerId { get; set; }

    [Required]
    public int ChildId { get; set; }

    public virtual Child Child { get; set; }
    public virtual Reindeer Reindeer { get; set; }
  }
}

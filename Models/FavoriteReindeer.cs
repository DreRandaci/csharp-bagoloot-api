using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BagoLootAPI.Models
{
  public class FavoriteReindeer
  {
    [Key]
    public int FavoriteReindeerId { get; set; }

    [Required]
    public int ReindeerId { get; set; }

    [Required]
    public int ChildId { get; set; }

    public Child Child { get; set; }
    public Reindeer Reindeer { get; set; }
  }
}

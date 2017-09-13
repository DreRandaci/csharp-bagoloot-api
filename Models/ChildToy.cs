using System.ComponentModel.DataAnnotations;

namespace BagoLootAPI.Models
{
  public class ChildToy
  {
    [Required]
    public string ChildName { get; set; }

    [Required]
    public string ToyName { get; set; }
  }
}

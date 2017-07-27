using System.ComponentModel.DataAnnotations;

namespace BagAPI.Models
{
  public class ChildToy
  {
    [Required]
    public string ChildName { get; set; }

    [Required]
    public string ToyName { get; set; }
  }
}

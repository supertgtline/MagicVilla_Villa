using System.ComponentModel.DataAnnotations;

namespace Magic_Web.Models.Dto;

public class VillaNumberUpdateDTO
{
    [Required]
    public int VillaNo { get; set; }
    [Required]
    public int VillaID { get; set; }
    public string SpecialDetails { get; set; }
}
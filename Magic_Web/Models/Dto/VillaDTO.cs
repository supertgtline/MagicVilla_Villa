using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Magic_Web.Models.Dto;

public class VillaDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }
    public string Details { get; set; }
    public double Rate { get; set; }
    public int Occupancy { get; set; }
    public int Sqft { get; set; }
    public string ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    public string Amenity { get; set; }
}
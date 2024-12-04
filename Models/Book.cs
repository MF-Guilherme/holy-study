using System.ComponentModel.DataAnnotations;

namespace HolyStudy.Models;

public class Book
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(4)]
    public string Abbreviation { get; set; }
    
    public ICollection<Passage> Passages { get; set; } = new List<Passage>();
}
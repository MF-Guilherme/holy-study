using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HolyStudy.Models;

public class Passage
{
    public int Id { get; set; }
    
    [Required]
    public int BookId { get; set; }
    [ForeignKey("BookId")]
    public Book Book { get; set; }
    
    [Required]
    public int ThemeId { get; set; }
    [ForeignKey("ThemeId")]
    public Theme Theme { get; set; }
    
    [Required]
    public string Chapter { get; set; }
    
    [Required]
    public int StartVerse { get; set; }
    
    public int? EndVerse { get; set; }
    public string? Description { get; set; }

}
using System.ComponentModel.DataAnnotations;

namespace HolyStudy.Models;

public class Theme
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "O nome do tema é obrigatório.")]
    [MaxLength(150)]
    public string Name { get; set; }
    
    public bool IsFavorite { get; set; }
    
    public ICollection<Passage> Passages { get; set; } = new List<Passage>();
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto.Tools;

public class AddToolDto
{
    [Required]
    public int CatalogId { get; set; }
    
    [Required]
    public string Title { get; set; }
    public string? SerialNumber { get; set; }
    public string? Note { get; set; }
}
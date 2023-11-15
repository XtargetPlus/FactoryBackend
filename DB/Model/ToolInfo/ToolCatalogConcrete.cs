using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.ToolInfo;
    public class ToolCatalogConcrete : IValidatableObject
    {
        /// <summary>
        /// Идентификатор инструмента
        /// </summary>
        public int ToolId { get; set; }
        public Tool? Tool { get; set; }
        /// <summary>
        /// Идентификатор каталога
        /// </summary>
        public int ToolCatalogId { get; set; }
        public ToolCatalog ToolCatalogs { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ToolId < 1)
                yield return new ValidationResult("Не удалось найти инструмент");
            if (ToolCatalogId < 1)
                yield return new ValidationResult("Не удалось найти каталог");


        }
    }

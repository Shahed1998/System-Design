using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Products.DTOs
{
    public record ProductDto(
        Guid Id,
        string Name,
        List<string> Category,
        string Description,
        string ImageFile,
        decimal Price
    );
}

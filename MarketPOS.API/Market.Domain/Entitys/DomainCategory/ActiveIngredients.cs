﻿using System.ComponentModel.DataAnnotations;

namespace Market.Domain.Entitys.DomainCategory;

public class ActiveIngredients : BaseEntity
{
    [Required]
    public string? Name { get; set; }

   public ICollection<Product> products { get; set; } = new List<Product>();


}

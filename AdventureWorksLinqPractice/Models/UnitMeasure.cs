﻿using System;
using System.Collections.Generic;

namespace AdventureWorksLinqPractice.Models;

/// <summary>
/// Unit of measure lookup table.
/// </summary>
public partial class UnitMeasure
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public string UnitMeasureCode { get; set; }

    /// <summary>
    /// Unit of measure description.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<BillOfMaterial> BillOfMaterials { get; set; } = new List<BillOfMaterial>();

    public virtual ICollection<Product> ProductSizeUnitMeasureCodeNavigations { get; set; } = new List<Product>();

    public virtual ICollection<ProductVendor> ProductVendors { get; set; } = new List<ProductVendor>();

    public virtual ICollection<Product> ProductWeightUnitMeasureCodeNavigations { get; set; } = new List<Product>();
}

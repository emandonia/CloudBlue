using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class PrivilegeCategory
{
    public int Id { get; set; }

    public string? PrivilegeCategoryName { get; set; }

    public virtual ICollection<Privilege> Privileges { get; set; } = new List<Privilege>();
}

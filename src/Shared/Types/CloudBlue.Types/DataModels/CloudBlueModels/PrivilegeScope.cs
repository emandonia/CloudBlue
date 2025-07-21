using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class PrivilegeScope
{
    public int Id { get; set; }

    public string? PrivilegeScopeName { get; set; }

    public virtual ICollection<EntityPrivilege> EntityPrivileges { get; set; } = new List<EntityPrivilege>();
}

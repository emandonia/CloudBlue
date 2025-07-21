using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class Privilege
{
    public int Id { get; set; }

    public string PrivilegeName { get; set; } = null!;

    public int PrivilegeCategoryId { get; set; }

    public string? PrivilegeMetaData { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public virtual PrivilegeCategory PrivilegeCategory { get; set; } = null!;
}

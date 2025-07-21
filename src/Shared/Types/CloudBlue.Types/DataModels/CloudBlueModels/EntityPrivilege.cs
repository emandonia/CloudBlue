using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class EntityPrivilege
{
    public long Id { get; set; }

    public int PrivilegeId { get; set; }

    public int UserGroupId { get; set; }

    public int UserTypeId { get; set; }

    public long UserId { get; set; }

    public int PrivilegeScopeId { get; set; }

    public virtual PrivilegeScope PrivilegeScope { get; set; } = null!;
}

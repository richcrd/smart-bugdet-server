using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class Language : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}
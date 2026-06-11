using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class Status : BaseEntity
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";

    public ICollection<User> Users { get; set; } = [];
    public ICollection<People> Peoples { get; set; } = [];
}
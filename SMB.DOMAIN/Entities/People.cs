using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class People : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    
    public long StatusId { get; set; }
    public Status Status { get; set; } = null!;
    
    public User? User { get; set; }
}
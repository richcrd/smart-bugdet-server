using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class Currency : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public int DecimalPlaces { get; set; } = 2;
}
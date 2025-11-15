using ScrewGameCard.Domain.Entities.Common;

namespace ScrewGameCard.Domain.Entities;

public class RefreshToken : IAuditable
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public Guid PlayerId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public virtual Player Player { get; set; }
}
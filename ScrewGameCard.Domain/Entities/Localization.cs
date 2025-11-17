using ScrewGameCard.Domain.Entities.Common;

namespace ScrewGameCard.Domain.Entities
{
    public class Localization : IAuditable
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required string Language { get; set; }
        public required string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

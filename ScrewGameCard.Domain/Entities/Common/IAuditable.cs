namespace ScrewGameCard.Domain.Entities.Common
{
    public interface IAuditable
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

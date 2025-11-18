namespace ScrewGameCard.DomainShared;

public class CachingDurationOption
{
    public const string SectionName = "CachingDuration";
    public int LocalizationCacheDurationInHours { get; set; } = 24; 
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Domain.Abstract;

namespace Web.Domain;

public class DictionarySettingsEntity : BaseEntity
{
    public DictionarySettingsEntity(
        Guid dictionaryId,
        string code,
        string description,
        DateTime startDate,
        DateTime endDate,
        List<string> availableProperties,
        List<PermissionEntity> permissions)
    {
        DictionaryId = dictionaryId;
        Code = code;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        AvailableProperties = availableProperties;
        Permissions = permissions;
    }

    [Key]
    public Guid DictionaryId { get; set; }

    [ForeignKey(nameof(DictionaryId))]
    public required DictionaryEntity Dictionary { get; set; }

    public List<PermissionEntity> Permissions { get; set; } = [];

    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    //TODO AvailableProperties is array Guid ForeignKeys to dictionary 
    // rename to AvailableDictionary
    public List<string> AvailableProperties { get; set; } = new(); // jsonb
}

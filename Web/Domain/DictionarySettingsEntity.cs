using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Domain.Abstract;

namespace Web.Domain;

public class DictionarySettingsEntity : BaseEntity
{
    public DictionarySettingsEntity() { }
    public DictionarySettingsEntity(
        Guid id,
        Guid dictionaryId,
        string code,
        string description,
        DateTime startDate, 
        DateTime endDate,
        List<Guid> availableDictionaries,
        List<PermissionEntity> permissions)
    {
        Id = id;
        DictionaryId = dictionaryId;
        Code = code;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        AvailableDictionaries = availableDictionaries;
        Permissions = permissions;
    }
    public Guid DictionaryId { get; set; }

    [ForeignKey(nameof(DictionaryId))]
    public DictionaryEntity Dictionary { get; set; }

    public List<PermissionEntity> Permissions { get; set; }

    [MaxLength(50)]
    public string Code { get; set; }

    [MaxLength(100)]
    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    //TODO AvailableProperties is array Guid ForeignKeys to dictionary 
    // rename to AvailableDictionary
    public List<Guid> AvailableDictionaries { get; set; }
    
    [Timestamp]
    public uint Version { get; set; }
}

using Web.Domain;

namespace Web.Endpoints.Requests;

public class DictionarySettingsDto
{
    public List<Guid> AvailableDictionaries { get; set; } = new();
    public List<PermissionEntity> Permissions { get; set; } = new();
}

public class DictionaryResultDto
{
    public List<DictionaryEntityDto> AvailableDictionaries { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}

public class DictionaryEntityDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

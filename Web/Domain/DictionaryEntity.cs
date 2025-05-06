using Web.Domain.Abstract;

namespace Web.Domain;

public class DictionaryEntity : BaseEntity
{
    public string Title { get; set; } = string.Empty;
}
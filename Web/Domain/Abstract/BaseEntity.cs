namespace Web.Domain.Abstract;

public class BaseEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
}
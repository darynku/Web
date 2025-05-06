using System.ComponentModel.DataAnnotations;

namespace Web.Domain.Abstract;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
}
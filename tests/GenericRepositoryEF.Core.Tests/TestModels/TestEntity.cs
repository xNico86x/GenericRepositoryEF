using GenericRepositoryEF.Core.Interfaces;

namespace GenericRepositoryEF.Core.Tests.TestModels
{
    public class TestEntity : IEntity<int>, IAuditableEntity, ISoftDelete
    {
        public int Id { get; set; }
        object IEntity.Id 
        { 
            get => Id;
            set => Id = (int)value; 
        }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Auditable properties
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        // Soft delete properties
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
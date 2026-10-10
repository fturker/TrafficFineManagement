namespace TrafficFineManagement.Web.Models.Entities
{
    public abstract class UpdatableEntity : BaseEntity
    {
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
    }
}

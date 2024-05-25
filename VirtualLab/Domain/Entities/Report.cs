namespace VirtualLab.Domain.Entities
{
    //TODO: дописать сущность
    public class Report : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime LastSentAt { get; set; }
        public string Description { get; set; } //TODO: заменить на Link, будет подгрузка файла

        public Guid UserLabId { get; set; }
        public UserLab UserLab { get; set; }
    }
}

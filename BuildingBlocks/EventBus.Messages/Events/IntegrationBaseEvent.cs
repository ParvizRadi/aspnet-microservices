namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public IntegrationBaseEvent()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }

        public IntegrationBaseEvent(Guid guid, DateTime createDate)
        {
            Id = guid;
            CreateDate = createDate;
        }

        public Guid Id { get; private set; }
        public DateTime CreateDate { get; private set; }
    }
}

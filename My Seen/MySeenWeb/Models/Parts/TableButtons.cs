namespace MySeenWeb.Models.Parts
{
    public class TableButtons
    {
        public int Id { get; set; }
        public bool Shared { get; set; }

        public TableButtons(int id, bool shared)
        {
            Id = id;
            Shared = shared;
        }
    }
}

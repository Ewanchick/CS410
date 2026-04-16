namespace ZuulRemake.Web.Models
{
    public class GameState
    {
        public string RoomName { get; set; } = "";
        public string RoomDescription { get; set; } = "";
        public int PlayerHP { get; set; }

        public List<string> Messages { get; set; } = new();
        public List<string> Inventory { get; set; } = new();
    }
}

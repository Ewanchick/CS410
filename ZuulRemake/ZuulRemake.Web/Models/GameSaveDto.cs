namespace ZuulRemake.Web.Models
{
    public class GameSaveDto
    {
        public string PlayerName { get; set; } = "";
        public int HP { get; set; }
        public int Level { get; set; }
        public int MaxWeight { get; set; }
        public string CurrentRoomName { get; set; } = "";
        public List<string> InventoryItemNames { get; set; } = new();
        public List<string> CollectedItemNames { get; set; } = new();
        public List<string> DefeatedMonsterNames { get; set; } = new();
    }
}

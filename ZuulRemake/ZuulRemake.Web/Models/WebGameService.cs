using ZuulRemake.Classes;

namespace ZuulRemake.Web.Models
{
    public interface IGameService
    {
        GameState GetGameState(HttpContext context);
        void SaveState(HttpContext context);
        GameViewModel GetViewModel(HttpContext context);
        void Move(HttpContext context);
        void PickUpItem(HttpContext context);
        void Attack(HttpContext context);
    }

    public class WebGameService : IGameService
    {
        NavigationManager navMan = new();
        CombatManager comMan = new(Player p, Monster m);


        GameState GetGameState(HttpContext context)
        {

        };

        void SaveState(HttpContext context)
        {

        };

        GameViewModel GetViewModel(HttpContext context)
        {

        };

        void Move(HttpContext context)
        {

        };

        void PickUpItem(HttpContext context)
        {

        };

        void Attack(HttpContext context)
        {

        };
    }
}

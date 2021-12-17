using WormsWorld.model;

namespace WormsWorld
{
    public interface IWormAiService
    {
        WormAction MakeDecision(Worm worm, Coord curCoord);
    }
}
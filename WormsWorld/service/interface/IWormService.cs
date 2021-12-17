using System.Collections.Immutable;
using WormsWorld.model;

namespace WormsWorld
{
    public interface IWormService
    {
        bool IsCellEmpty(Coord coord);
        void NextStep(Coord newFoodCoord);
        ImmutableDictionary<Coord, Worm> GetState();
    }
}
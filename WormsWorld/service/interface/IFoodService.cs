using WormsWorld.model;

namespace WormsWorld
{
    public interface IFoodService : IRemovable
    {
        Coord NextStep(Coord newCoord);
        Coord GenerateCell();
    }
}
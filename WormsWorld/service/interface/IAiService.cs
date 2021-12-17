using WormsWorld.model;

namespace WormsWorld
{
    public interface IAiService
    {
        Coord GetNearest(Coord myCoord);
    }
}
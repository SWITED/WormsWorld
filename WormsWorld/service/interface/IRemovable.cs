using WormsWorld.model;

namespace WormsWorld
{
    public interface IRemovable
    {
        void ClearCell(Coord coord);
        bool IsCellEmpty(Coord coord);
    }
}
namespace WormsWorld.service
{
    public class NameService : INameService
    {
        private int _curWormNum = 1;
        public string GenerateRandomName()
        {
            return "Karl_" + _curWormNum++;
        }
    }
}
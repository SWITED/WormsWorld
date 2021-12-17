using System;
using WormsWorld.model;

namespace WormsWorld
{
    public static class WorldUtil
    {
        public static int NextNormal(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();

            var u2 = r.NextDouble();

            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            var randNormal = mu + sigma * randStdNormal;

            return (int) Math.Round(randNormal);
        }

        public static int TaxicabDistance(Coord start, Coord end)
        {
            var vec = end - start;
            return Math.Abs(vec.GetX()) + Math.Abs(vec.GetY());
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace HexedSubworlds.Core
{
    public static class SubworldLoader
    {
        public static IList<Subworld> Subworlds = new List<Subworld>();
        public static int Count = 0;
        public static int NextId => Count++;

        public static Subworld GetByType(int type) => Subworlds.First((sw) => sw.Type == type);
    }
}

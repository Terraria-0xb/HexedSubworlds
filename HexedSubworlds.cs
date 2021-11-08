using log4net;
using Terraria.ModLoader;

namespace HexedSubworlds
{
	public class HexedSubworlds : Mod
	{
		public static HexedSubworlds Instance => ModContent.GetInstance<HexedSubworlds>();
		public static ILog Log => Instance.Logger;
	}
}
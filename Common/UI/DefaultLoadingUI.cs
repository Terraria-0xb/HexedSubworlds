using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace HexedSubworlds.Common.UI
{
    public class DefaultLoadingUI : UIState
    {
        public override void OnInitialize()
        {
            UITextPanel<string> textPanel = new UITextPanel<string>("Hello, World!");
            textPanel.HAlign = textPanel.VAlign = 0.5f;
            Append(textPanel);

            base.OnInitialize();
        }
    }
}

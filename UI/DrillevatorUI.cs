using Microsoft.Xna.Framework;
using MiningDimension.Players;
using MiningDimension.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace MiningDimension.UI
{
    public class DrillevatorUI : UIState
    {
        UIPanel panel;
        // TODO: Sort this whole block out so it isnt massive as hell
        public override void OnInitialize()
        {
            panel = new();
            panel.Width.Set(800, 0);
            panel.Height.Set(600, 0);
            panel.VAlign = panel.HAlign = 0.5f;
            Append(panel);

            UIText normalOreHeader = new("Normal Ores");
            normalOreHeader.HAlign = 0.25f; // 1
            normalOreHeader.Top.Set(15, 0); // 2
            panel.Append(normalOreHeader);

            UIText biomeOreHeader = new("Biome specific Ores");
            biomeOreHeader.HAlign = 0.75f;
            biomeOreHeader.Top.Set(15, 0);
            panel.Append(biomeOreHeader);


            UIButtonEnterSubworld travelButtonPreBoss = new UIButtonEnterSubworld("Descend", MiningSubworldID.PreBoss);
            travelButtonPreBoss.Width.Set(120, 0);
            travelButtonPreBoss.Height.Set(40, 0);
            travelButtonPreBoss.HAlign = 0.08f;
            travelButtonPreBoss.Top.Set(0, 0.1f);
            panel.Append(travelButtonPreBoss);

            UIButtonEnterSubworld travelButtonEvilOres = new UIButtonEnterSubworld("Descend", MiningSubworldID.EvilOres);
            travelButtonEvilOres.Width.Set(120, 0);
            travelButtonEvilOres.Height.Set(40, 0);
            travelButtonEvilOres.HAlign = 0.08f;
            travelButtonEvilOres.Top.Set(0, 0.2f);
            panel.Append(travelButtonEvilOres);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (panel.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }

        // TODO: do some sort of check for the drives
    }

    public class UIButtonEnterSubworld : UIElement
    {
        // Code borrowed (stolen) from the tMod wiki
        private object _text;
        private MouseEvent _clickAction;
        private UIPanel _uiPanel;
        private UIText _uiText;
        public int SubworldToEnter;

        public string Text
        {
            get => _uiText?.Text ?? string.Empty; // Check if _uiText is null, return .Text if not null, return an empty string if .Text is null
            set => _text = value;
        }

        // Make sure to use the right constructors for the buttons
        public UIButtonEnterSubworld(object text, MouseEvent clickAction) : base()
        {
            _text = text?.ToString() ?? string.Empty; // assign text parameter to _text if not null, or an empty string if null
            _clickAction = clickAction;
            SubworldToEnter = 0;
        }

        public UIButtonEnterSubworld(object text, int subworldToEnter) : base()
        {
            _text = text?.ToString() ?? string.Empty; // assign text parameter to _text if not null, or an empty string if null
            SubworldToEnter = subworldToEnter;

            _clickAction = (UIMouseEvent evt, UIElement listeningElement) => ModContent.GetInstance<DrillUISystem>().ShowConfirmationUI(SubworldToEnter);
        }

        public override void OnInitialize()
        {
            _uiPanel = new UIPanel();
            _uiPanel.Width = StyleDimension.Fill; // Makes the panel as large as the constructed element
            _uiPanel.Height = StyleDimension.Fill;
            Append(_uiPanel);

            _uiText = new UIText("");
            _uiText.VAlign = _uiText.HAlign = 0.5f; // Align text to center of panel
            _uiPanel.Append(_uiText);

            _uiPanel.OnLeftClick += _clickAction; // Execute the action passed in during construction
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Propagate update to child elements.
            if (_text != null)
            {                    // Magic to make the text drawing "thread safe". Note to self: find out what that is
                _uiText.SetText(_text.ToString());
                _text = null;
                Recalculate();                      // Recalculating resizes the element and its children
                base.MinWidth = _uiText.MinWidth;   // You should do this if the content changes - tMod wiki
                base.MinHeight = _uiText.MinHeight;
            }
        }
    }
}

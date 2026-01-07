using FairMinesweeper.Properties;
using System.Windows.Forms;

namespace FairMinesweeper
{
    static class EventHandlers
    {
        static bool waitingForRelease = false;
        static bool disabled = false;
        
        /// <summary>
        /// Disable globaly all input handling.
        /// </summary>
        static public void Disable()
        {
            disabled = true;
        }
        /// <summary>
        /// Enable globaly all input handling
        /// </summary>
        static public void Enable()
        {
            disabled = false;
        }
        /// <summary>
        /// Handles visual responce when left click and holding buttons as well as
        /// calling RightClick function and flagging the tile when right-clicking.
        /// </summary>
        static public void OnPress(object sender, MouseEventArgs e)
        {
            Button1 button = (Button1)sender;

            if (disabled) { return; }

            if (e.Button == MouseButtons.Right &&
                ! button.Tile.Uncovered) // only allowed to right click blank od flagged tiles
            { 
                RightClick(button); 
            }
            // visual click responce:
            else if (e.Button == MouseButtons.Left &&
                     BlankTile(button)) // only visualize clicks on not-uncovered tiles
            {
                waitingForRelease = true;
                button.Image = Resources._0;
                // until mouse release, button appears clicked
            }
        }
        /// <summary>
        /// Returns button visual to original "non-pressed" state when letting go of left click.
        /// </summary>
        static public void OnRelease(object sender, MouseEventArgs e)
        {
            Button1 button = (Button1)sender;

            if (waitingForRelease)
            {
                button.Image = Resources.blank_tile; // return button to before clicked state if not clicked properly
            }
            waitingForRelease = false;
        }
        /// <summary>
        /// Uncoveres the tile, but only if left click (both press and release) happend above the tile.
        /// </summary>
        static public void OnClick(object sender, MouseEventArgs e)
        {
            Button1 button = (Button1)sender;

            if (disabled) { return; }

            if (e.Button == MouseButtons.Left && BlankTile(button)) { LeftClick(button); }
        }
        /// <summary>
        /// Tell the `button` that this tile has been right clicked. (call the ToggleFlagged method)
        /// </summary>
        static void RightClick(Button1 button) 
        {
            button.Tile.ToggleFlagged();
            waitingForRelease = false;
        }
        /// <summary>
        /// Tell the `button` that this tile has been left clicked. (call the Uncover method)
        /// </summary>
        static void LeftClick(Button1 button)
        {
            button.Tile.Uncover(true);
            waitingForRelease = false;
        }
        /// <summary>
        /// Decomposition of button state: not Uncovered and not Flagged.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        static bool BlankTile(Button1 button)
        {
            if (button.Tile.Uncovered || button.Tile.Flagged) { return false; }
            return true;
        }
    }
}

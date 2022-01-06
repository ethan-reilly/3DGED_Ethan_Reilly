using GDLibrary;
using GDLibrary.Core;
using GDLibrary.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDApp
{
    public class MyMenuManager : UIMenuManager//, UISceneManager
    {
        public MyMenuManager(Game game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
        }

        protected override void HandleMouseClicked(UIButtonObject btnObject)
        {
            switch (btnObject.Name)
            {
                case AppData.MENU_PLAY_BTN_NAME:
                    EventDispatcher.Raise(new EventData(EventCategoryType.Menu, EventActionType.OnPlay));
                    object[] backgroundMusic = { "music" };
                    EventDispatcher.Raise(new EventData(EventCategoryType.Sound,
                        EventActionType.OnPlay2D, backgroundMusic));
                    

                    break;

                case AppData.MENU_PLAY_BTN_LVL_2:
                    //SetActiveScene("level 2"); UiScene not Scene
                    //LoadScene("level 2"); // Can't inherit from 2 classes, would have to make them interfaces
                    EventDispatcher.Raise(new EventData(EventCategoryType.Menu, EventActionType.OnPlay));
                    break;

                case AppData.MENU_BACK_BTN_NAME:
                    SetActiveScene(AppData.MENU_MAIN_NAME);
                    break;

                case AppData.MENU_EXIT_BTN_NAME:
                    Game.Exit();
                    break;
            }
        }

        protected override void HandleMouseOver(UIButtonObject btnObject)
        {
            //object[] parameters = { 0 };

            //EventDispatcher.Raise(new EventData(EventCategoryType.Sound,
            //    EventActionType.OnVolumeDelta, parameters));

            // throw new System.NotImplementedException();
        }
    }
}
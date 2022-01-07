using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Core;

namespace GDApp
{
    /// <summary>
    /// To define how collisions are handled (in HandleCollision) we inherit from
    /// Collider and define how HandleCollision deals with an object it encounters
    /// </summary>
    public class MyPlayerCollider2 : Collider
    {
        /// <summary>
        /// Constructs collider which is always going to handle collisions and is never going to be a trigger
        /// </summary>
        public MyPlayerCollider2()
            : base(true, false)
        {
        }

        protected override void HandleResponse(GameObject collideeGameObject)
        {
            //    System.Diagnostics.Debug.WriteLine($"{collideeGameObject.Name}");
            //if (collideeGameObject.GameObjectType == GameObjectType.Lava)

            // Win Lose Mechanics
            if (collideeGameObject.GameObjectType == GameObjectType.Camera)
            {
                Application.UISceneManager.SetActiveScene(AppData.WIN_SCREEN);
                //EventDispatcher.Raise(new EventData(EventCategoryType.Menu, EventActionType.OnPause));
                //EventDispatcher.Raise(new EventData(EventCategoryType.Sound, EventActionType.OnPause));
                EventDispatcher.Raise(new EventData(EventCategoryType.GameState, EventActionType.OnWin));
                //Application.SceneManager.LoadScene(AppData.LOSE_SCREEN);

            }
            //if interactable then...

            //else if consumable then...

            //else if modifiable then...
        }
    }
}
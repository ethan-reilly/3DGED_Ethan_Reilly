﻿using GDLibrary;
using GDLibrary.Components;
using GDLibrary.Core;

namespace GDApp
{
    /// <summary>
    /// To define how collisions are handled (in HandleCollision) we inherit from
    /// Collider and define how HandleCollision deals with an object it encounters
    /// </summary>
    public class MyPlayerCollider : Collider
    {
        /// <summary>
        /// Constructs collider which is always going to handle collisions and is never going to be a trigger
        /// </summary>
        public MyPlayerCollider()
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
              EventDispatcher.Raise(new EventData(EventCategoryType.Menu, EventActionType.OnPause));
              EventDispatcher.Raise(new EventData(EventCategoryType.GameState, EventActionType.OnLose));
                
            }
            //if interactable then...

            //else if consumable then...

            //else if modifiable then...
        }
    }
}
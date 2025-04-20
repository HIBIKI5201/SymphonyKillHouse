using System;
using KillHouse.Runtime.System;
using SymphonyFrameWork.System;
using Unity.VisualScripting;
using UnityEngine;

namespace KillHouse.Runtime.Ingame
{ 
    public class PlayerManager : MonoBehaviour 
    {
        private void Start()
        {
            var inputBuffer = ServiceLocator.GetInstance<InputBuffer>();
            var actions = inputBuffer.PlayerInput.actions;
            actions["Attack"].started += ctx => Debug.Log("Attack");
        }
    } 
}

using Game.Scripts.Managers;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Injection
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private InputManager inputManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputManager>().FromInstance(inputManager).AsSingle();
        }
    }
}
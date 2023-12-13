using Cinemachine;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerAnimManager animManager;
    [SerializeField] private PlayerCamManager camManager;
    [SerializeField] private PlayerAmmoManager ammoManager;
    [SerializeField] private PlayerInteract interact;
    [SerializeField] private PlayerShootingManager shootingManager;

    [Header("CAMERA")]
    [SerializeField] private CinemachineImpulseSource impulseSource;
    public override void InstallBindings()
    {
        Container.Bind<PlayerController>().FromInstance(controller);
        Container.Bind<PlayerAnimManager>().FromInstance(animManager);
        Container.Bind<PlayerCamManager>().FromInstance(camManager);
        Container.Bind<PlayerAmmoManager>().FromInstance(ammoManager);
        Container.Bind<PlayerInteract>().FromInstance(interact);
        Container.Bind<PlayerShootingManager>().FromInstance(shootingManager);

        Container.Bind<CinemachineImpulseSource>().FromInstance(impulseSource);
    }
}

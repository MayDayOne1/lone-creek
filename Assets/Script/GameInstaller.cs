using Cinemachine;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("PLAYER")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerAnimManager animManager;
    [SerializeField] private PlayerAmmoManager ammoManager;
    [SerializeField] private PlayerInteract interact;
    [SerializeField] private PlayerShootingManager shootingManager;
    [SerializeField] private PlayerAudioManager audioManager;
    [SerializeField] private NotificationManager notificationManager;

    [Header("CAMERA")]
    [SerializeField] private Camera cam;
    [SerializeField] private PlayerCamManager camManager;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("ENEMY")]
    [SerializeField] private TargetForEnemy targetForEnemy;

    public override void InstallBindings()
    {
        Container.Bind<PlayerController>().FromInstance(controller);
        Container.Bind<PlayerAnimManager>().FromInstance(animManager);
        Container.Bind<PlayerAmmoManager>().FromInstance(ammoManager);
        Container.Bind<PlayerInteract>().FromInstance(interact);
        Container.Bind<PlayerShootingManager>().FromInstance(shootingManager);
        Container.Bind<PlayerAudioManager>().FromInstance(audioManager);
        Container.Bind<NotificationManager>().FromInstance(notificationManager);

        Container.Bind<Camera>().FromInstance(cam);
        Container.Bind<PlayerCamManager>().FromInstance(camManager);
        Container.Bind<CinemachineImpulseSource>().FromInstance(impulseSource);

        Container.Bind<TargetForEnemy>().FromInstance(targetForEnemy);

        Container.Bind<AnalyticsManager>().AsSingle();
    }
}

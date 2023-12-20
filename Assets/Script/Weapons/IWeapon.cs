public interface IWeapon
{
    public void AimCamSetup();
    public void CamSetup();
    public void Disable();
    public void EnableUI(bool enable);

    public void PlaySelectionSound();
    public void Select();
    public void Shoot();
    public void StartAim();
    public void StopAim();
}

using UnityEngine;

[System.Serializable]
public class Cooldown
{
    #region Variables
    private float _nextFireTime;

    #endregion

    public bool IsCoolingDown => Time.time < _nextFireTime;
    public void StartCooldown(float cooldownTime) => _nextFireTime = Time.time + cooldownTime;
}
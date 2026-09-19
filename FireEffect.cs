using UnityEngine;

public sealed class FireEffect : StatusEffect
{
	private const float RampDuration = 5f;
	private const float StartingIntensity = 0.25f;

	private float _burnTime;
	private float _damagePerSecond;

	public FireEffect() : base("Fire") { }

	public float MaxDamagePerSecond { get; set; }

	protected override void OnStarted()
	{
		_burnTime = 0f;
	}

	//burns hotter the longer it lasts
	protected override void OnTickStarted(float deltaTime)
	{
		_burnTime += deltaTime;
		_damagePerSecond = MaxDamagePerSecond * Mathf.Lerp(StartingIntensity, 1f, _burnTime / RampDuration);
	}

	protected override bool CanAffect(Zombie zombie) => base.CanAffect(zombie) && !zombie.IsWet;

	protected override float Apply(Zombie zombie, float deltaTime)
	{
		return zombie.TakeDamage(_damagePerSecond * deltaTime, this);
	}
}

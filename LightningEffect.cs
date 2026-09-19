public sealed class LightningEffect : StatusEffect
{
	private const float StrikeInterval = 1f;
	private const float WetDamageMultiplier = 2f;

	private float _timeToStrike;
	private bool _striking;

	public LightningEffect() : base("Lightning") { }

	public float DamagePerStrike { get; set; }

	protected override void OnStarted()
	{
		_timeToStrike = StrikeInterval;
	}

	//bursts on an interval instead of ticking damage every frame
	protected override void OnTickStarted(float deltaTime)
	{
		_timeToStrike -= deltaTime;
		_striking = _timeToStrike <= 0f;

		if (_striking)
		{
			_timeToStrike += StrikeInterval;
		}
	}

	//arcs over shields
	protected override bool CanAffect(Zombie zombie) => zombie.IsAlive;

	protected override float Apply(Zombie zombie, float deltaTime)
	{
		if (!_striking) return 0f;

		float damage = zombie.IsWet ? DamagePerStrike * WetDamageMultiplier : DamagePerStrike;

		return zombie.TakeDamage(damage, this);
	}
}

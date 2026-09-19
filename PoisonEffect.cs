public sealed class PoisonEffect : StatusEffect
{
	public PoisonEffect() : base("Poison") { }

	public float DamagePerSecond { get; set; }

	protected override float Apply(Zombie zombie, float deltaTime)
	{
		return zombie.TakeDamage(DamagePerSecond * deltaTime, this);
	}
}

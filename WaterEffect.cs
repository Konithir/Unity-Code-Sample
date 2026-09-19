public sealed class WaterEffect : StatusEffect
{
	private const float WetDuration = 3f;

	public WaterEffect() : base("Water") { }

	//deals no damage on its own - it soaks zombies, which keeps Fire off them and doubles Lightning
	protected override float Apply(Zombie zombie, float deltaTime)
	{
		zombie.Soak(WetDuration);

		return 0f;
	}
}

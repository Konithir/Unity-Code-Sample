using System.Collections.Generic;

public abstract class StatusEffect
{
	protected StatusEffect(string name)
	{
		Name = name;
		DeathMessage = "I died from: " + name;
	}

	public string Name { get; }
	public string DeathMessage { get; }

	public bool IsActive { get; private set; }

	public int AffectedCount { get; private set; }

	public float TotalDamageDealt { get; private set; }

	public float AverageHealth { get; private set; }

	public void Start()
	{
		IsActive = true;

		AffectedCount = 0;
		TotalDamageDealt = 0f;
		AverageHealth = 0f;

		OnStarted();
	}

	public void Stop()
	{
		IsActive = false;

		AffectedCount = 0;
		AverageHealth = 0f;
	}

	public void Tick(IReadOnlyList<Zombie> zombies, float deltaTime)
	{
		if (!IsActive) return;

		OnTickStarted(deltaTime);

		int affected = 0;
		float totalHealth = 0f;

		//backwards: a dying zombie leaves the registry mid-loop, which only shifts indices we have passed
		for (int i = zombies.Count - 1; i >= 0; i--)
		{
			//not checking for nulls on purpose, I want the code to crash instead of hiding a logic error elsewhere
			Zombie zombie = zombies[i];
			if (!CanAffect(zombie)) continue;

			TotalDamageDealt += Apply(zombie, deltaTime);

			if (!zombie.IsAlive) continue;

			affected++;
			totalHealth += zombie.Health;
		}

		AffectedCount = affected;
		AverageHealth = affected > 0 ? totalHealth / affected : 0f;
	}

	protected virtual void OnStarted() { }

	//anything the effect needs to compute once per tick instead of once per zombie
	protected virtual void OnTickStarted(float deltaTime) { }

	protected virtual bool CanAffect(Zombie zombie) => zombie.IsAlive && !zombie.ShieldActive;

	protected abstract float Apply(Zombie zombie, float deltaTime);
}

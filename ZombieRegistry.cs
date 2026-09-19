using System.Collections.Generic;
using UnityEngine;

public static class ZombieRegistry
{
	private static readonly List<Zombie> _zombies = new List<Zombie>(64);

	public static IReadOnlyList<Zombie> Zombies => _zombies;

	public static void Register(Zombie zombie) => _zombies.Add(zombie);

	public static void Unregister(Zombie zombie) => _zombies.Remove(zombie);

	//statics survive Enter Play Mode when domain reload is disabled - clear them by hand
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetState() => _zombies.Clear();
}

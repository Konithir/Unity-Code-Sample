using System;
using UnityEngine;

public class Zombie : MonoBehaviour
{
	[SerializeField] private string _name;
	[SerializeField] private float _health = 100f;
	[SerializeField] private bool _shieldActive;

	private float _wetUntil;

	public event Action<Zombie, StatusEffect> Died;

	public string Name => _name;
	public float Health => _health;
	public bool IsAlive => _health > 0f;
	public bool IsWet => Time.time < _wetUntil;

	public bool ShieldActive
	{
		get => _shieldActive;
		set => _shieldActive = value;
	}

	private void OnEnable()
	{
		_wetUntil = 0f;

		ZombieRegistry.Register(this);
	}

	private void OnDisable() => ZombieRegistry.Unregister(this);

	//returns the damage actually dealt, never more than the remaining health
	public float TakeDamage(float amount, StatusEffect source)
	{
		if (!IsAlive) return 0f;

		float applied = Mathf.Min(amount, _health);
		_health -= applied;

		if (!IsAlive)
		{
			Die(source);
		}

		return applied;
	}

	//a timestamp instead of a countdown, so wetness costs no per-frame update
	public void Soak(float duration)
	{
		_wetUntil = Mathf.Max(_wetUntil, Time.time + duration);
	}

	private void Die(StatusEffect source)
	{
		ShowDeathMessage(source.DeathMessage);
		Died?.Invoke(this, source);

		gameObject.SetActive(false); //fires OnDisable -> unregisters
	}

	public void ShowDeathMessage(string message)
	{
		Debug.Log(_name + ": " + message, this);
	}
}

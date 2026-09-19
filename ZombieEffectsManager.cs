using System;
using TMPro;
using UnityEngine;

public class ZombieEffectsManager : MonoBehaviour
{
	private const string TextFormat = "  affected: {0:0}  damage: {1:0}  AVG health: {2:1}";

	[SerializeField] private TextMeshProUGUI _text;

	private readonly PoisonEffect _poison = new PoisonEffect();
	private readonly FireEffect _fire = new FireEffect();
	private readonly WaterEffect _water = new WaterEffect();
	private readonly LightningEffect _lightning = new LightningEffect();

	private StatusEffect[] _effects;
	private string[] _textFormats;
	private int _displayed;

	private void Awake()
	{
		_effects = new StatusEffect[] { _poison, _fire, _water, _lightning };

		_textFormats = new string[_effects.Length];
		for (int i = 0; i < _effects.Length; i++)
		{
			_textFormats[i] = _effects[i].Name + TextFormat;
		}
	}

	private void OnEnable()
	{
		RefreshText();
	}

	public void StartPoisonEffect(int damagePerSecond)
	{
		_poison.DamagePerSecond = damagePerSecond;

		StartEffect(_poison);
	}

	public void StartFireEffect(int maxDamagePerSecond)
	{
		_fire.MaxDamagePerSecond = maxDamagePerSecond;

		StartEffect(_fire);
	}

	public void StartWaterEffect()
	{
		StartEffect(_water);
	}

	public void StartLightningEffect(int damagePerStrike)
	{
		_lightning.DamagePerStrike = damagePerStrike;

		StartEffect(_lightning);
	}

	public void StopPoisonEffect() => StopEffect(_poison);
	public void StopFireEffect() => StopEffect(_fire);
	public void StopWaterEffect() => StopEffect(_water);
	public void StopLightningEffect() => StopEffect(_lightning);

	private void StartEffect(StatusEffect effect)
	{
		effect.Start();

		//several effects can run at once, the HUD follows the last one started
		_displayed = Array.IndexOf(_effects, effect);
	}

	private void StopEffect(StatusEffect effect)
	{
		effect.Stop();

		RefreshText();
	}

	private void Update()
	{
		if (!AnyEffectActive()) return;

		float deltaTime = Time.deltaTime;
		var zombies = ZombieRegistry.Zombies;

		for (int i = 0; i < _effects.Length; i++)
		{
			_effects[i].Tick(zombies, deltaTime);
		}

		RefreshText();
	}

	private bool AnyEffectActive()
	{
		for (int i = 0; i < _effects.Length; i++)
		{
			if (_effects[i].IsActive) return true;
		}

		return false;
	}

	private void RefreshText()
	{
		StatusEffect effect = _effects[_displayed];

		//{n:d} is TMP's own precision specifier, not string.Format
		_text.SetText(_textFormats[_displayed], effect.AffectedCount, effect.TotalDamageDealt, effect.AverageHealth);
	}
}

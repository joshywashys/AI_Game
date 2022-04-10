using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerUIController : MonoBehaviour
{
	public RuntimePlayerSO playerStats = null;
	public Transform healthRoot = null;

	public Image heartPrefab = null;

	public UnityEvent onShowEvent = new UnityEvent();
	public UnityEvent onHideEvent = new UnityEvent();

	private List<Image> heartVisuals = new List<Image>();

    private void Start()
    {
		Initialize();
    }

    private void Update()
	{
		UpdateHealthUI();
	}

	public void Initialize()
	{
		for (int i = 0; i < playerStats.maxHealth; i++)
    		heartVisuals.Add(Instantiate(heartPrefab, healthRoot));
    }

	public void UpdateHealthUI()
    {
		for (int i = heartVisuals.Count - 1; i >= 0; i--)
		{
			float fillAmount = (i >= playerStats.currentHealth) ? 0 : playerStats.currentHealth - i / 1.0f;
			heartVisuals[i].fillAmount = fillAmount;
		}
	}
}

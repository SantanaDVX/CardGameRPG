using UnityEngine;
using System.Collections.Generic;

public class GlowObject : MonoBehaviour
{
	public Color GlowColor;
	public float LerpFactor = 10;

	public Renderer[] Renderers
	{
		get;
		private set;
	}

	public Color CurrentColor
	{
		get { return _currentColor; }
	}

    public List<Material> _materials = new List<Material>();
    public Color _currentColor;
    public Color _targetColor;

	public void Start()
	{
		Renderers = GetComponentsInChildren<Renderer>();

		foreach (var renderer in Renderers)
		{
			_materials.AddRange(renderer.materials);
		}
	}

	public void startGlow() {
        //Debug.Log("start glow: " + name);
		_targetColor = GlowColor;
		enabled = true;
	}

    public void endGlow() {
        //Debug.Log("end glow");
        _targetColor = Color.black;
		enabled = true;
	}

	/// <summary>
	/// Loop over all cached materials and update their color, disable self if we reach our target color.
	/// </summary>
	private void Update()
	{
		_currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

		for (int i = 0; i < _materials.Count; i++)
		{
			_materials[i].SetColor("_GlowColor", _currentColor);
        }

        if (_currentColor.Equals(_targetColor))
		{
			enabled = false;
		}
	}
}

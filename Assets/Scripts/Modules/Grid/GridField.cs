using DivineSkies.Modules.Game.Card;
using UnityEngine;

public class GridField : MonoBehaviour
{
    [SerializeField] private MeshRenderer _selectionRenderer;
    [SerializeField] private Transform _animalDisplay;
    [SerializeField] private bool _isWater;
    [SerializeField] private bool _isPlaceable;

    private AnimalsSpecies _animal = AnimalsSpecies.None;
    public AnimalsSpecies Animal => _animal;
    public bool WaterField => _isWater;

    private void Awake()
    {
        SetSelectionVisibility(false);
    }

    public void SetSelectionVisibility(bool value)
    {
        if(!_isPlaceable && value) //do not turn on selection if not placeable
        {
            return;
        }

        _selectionRenderer.gameObject.SetActive(value);
        _selectionRenderer.material.color = new Color(1f, 1f, 1f, 0.25f);
    }

    public void OnMouseEnter()
    {
        _selectionRenderer.material.color = _animal == AnimalsSpecies.None ? new Color(0.3f, 1f, 0.3f, 0.5f) : new Color(1f, 0.3f, 0.3f, 0.5f);
    }

    public void OnMouseExit()
    {
        _selectionRenderer.material.color = new Color(1f, 1f, 1f, 0.25f);
    }

    public void OnMouseDown()
    {
        if (_animal != AnimalsSpecies.None)
        {
            return;
        }

        CardGameController.Main.OnFieldSelected(this);
    }

    public void SetAnimal(AnimalsSpecies newAnimal)
    {
        if (_animal == newAnimal)
        {
            return;
        }
        _animal = newAnimal;
        for (int i = 0; i < _animalDisplay.childCount; i++)
        {
            _animalDisplay.GetChild(i).gameObject.SetActive(_animalDisplay.GetChild(i).name.Contains(_animal.ToString(), System.StringComparison.OrdinalIgnoreCase));
        }
    }
}

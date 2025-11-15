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
        for (int i = 0; i < _animalDisplay.childCount; i++)
        {
            _animalDisplay.GetChild(i).gameObject.SetActive(false);
        }

        SetSelectionVisibility(false);
    }

    public void SetSelectionVisibility(bool value)
    {
        if(!_isPlaceable && value) //do not turn on selection if not placeable
        {
            return;
        }

        _selectionRenderer.gameObject.SetActive(value);
        _selectionRenderer.material.color = _isWater ? Color.softBlue : Color.white;
    }

    public void OnMouseEnter()
    {
        _selectionRenderer.material.color = _animal != AnimalsSpecies.None ? Color.softRed : Color.limeGreen;
    }

    public void OnMouseExit()
    {
        _selectionRenderer.material.color = _isWater ? Color.softBlue : Color.white;
    }

    public void OnMouseDown()
    {
        if (_animal != AnimalsSpecies.None || _selectionRenderer.gameObject.activeSelf == false)
        {
            return;
        }

        AnimalCardGameController.Main.Processor.OnFieldSelected(this);
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

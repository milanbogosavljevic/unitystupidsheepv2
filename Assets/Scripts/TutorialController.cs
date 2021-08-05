
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    private int _tutorialLevel;
    private int _maxLevel;
    private bool _showPlayButtonsSelector;
    private bool _showPauseSawsButtonsSelector;
    private bool _showSheepSelectors;
    private bool _showPauseSheepIndicatorSelectors;
    private bool _showSheepSpeedIndicatorSelectors;
    private GameController _gameController;
    public Text textField;
    [SerializeField] private Text pageNumberText;
    [SerializeField] private Toggle dontShowToggle;
    [SerializeField] private Image pauseSawsButtonSelector;
    [SerializeField] private Image pauseSheepIndicatorSelector;
    [SerializeField] private Image sheepSpeedIndicatorSelector;
    [SerializeField] private List<Image> playButtonsSelectors;
    [SerializeField] private List<Image> sheepSelectors;
    [SerializeField] [TextArea(5,5)] List<String> tutorialTexts;
    void Start()
    {
        _showPlayButtonsSelector = false;
        _showPauseSawsButtonsSelector = false;
        _showSheepSelectors = false;
        _showPauseSheepIndicatorSelectors = false;
        _showSheepSpeedIndicatorSelectors = false;
        _tutorialLevel = 0;
        _maxLevel = tutorialTexts.Count - 1;
        textField.text = tutorialTexts[_tutorialLevel];
        _UpdatePageNumberText();
        _gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (_showPlayButtonsSelector)
        {
            foreach (Image selector in playButtonsSelectors)
            {
                selector.transform.Rotate(Vector3.back * 20f * Time.deltaTime);
            }
        }

        if (_showPauseSawsButtonsSelector)
        {
            pauseSawsButtonSelector.transform.Rotate(Vector3.back * 20f * Time.deltaTime);
        }

        if (_showSheepSelectors)
        {
            foreach (Image selector in sheepSelectors)
            {
                selector.transform.Rotate(Vector3.back * 20f * Time.deltaTime);
            }
        }
        
        if (_showPauseSheepIndicatorSelectors)
        {
            pauseSheepIndicatorSelector.transform.Rotate(Vector3.back * 20f * Time.deltaTime);
        }
        
        if (_showSheepSpeedIndicatorSelectors)
        {
            sheepSpeedIndicatorSelector.transform.Rotate(Vector3.back * 20f * Time.deltaTime);
        }
    }

    public void SwitchTutorialPage(bool switchToRight)
    {
        if (switchToRight)
        {
            if (_tutorialLevel < _maxLevel)
            {
                _tutorialLevel++;
                textField.text = tutorialTexts[_tutorialLevel];
            }
        }
        else
        {
            if (_tutorialLevel > 0)
            {
                _tutorialLevel--;
                textField.text = tutorialTexts[_tutorialLevel];
            }
        }
        _UpdatePageNumberText();
        _UpdateGrafix();
    }

    private void _UpdateGrafix()
    {
        _EnablePlayButtonsSelectors(false);
        _EnablePauseSawsButtonSelector(false);
        _EnableSheepSelectors(false);
        _EnablePauseSheepIndicatorSelector(false);
        _EnableSheepSpeedIndicatorSelector(false);
        switch (_tutorialLevel)
        {
            case 1:
                _EnablePlayButtonsSelectors(true);
                break;
            case 2:
                _EnablePauseSawsButtonSelector(true);
                break;
            case 3:
                _EnableSheepSelectors(true);
                break;
            case 4:
                _EnablePauseSheepIndicatorSelector(true);
                break;
            case 5:
                _EnableSheepSpeedIndicatorSelector(true);
                break;
        }
    }
    
    private void _EnablePlayButtonsSelectors(bool enableSelectors)
    {
        if (_showPlayButtonsSelector != enableSelectors)
        {
            foreach (Image selector in playButtonsSelectors)
            {
                selector.enabled = enableSelectors;
            }
            _showPlayButtonsSelector = enableSelectors;
        }
    }

    private void _EnablePauseSawsButtonSelector(bool enableSelector)
    {
        if (_showPauseSawsButtonsSelector != enableSelector)
        {
            pauseSawsButtonSelector.enabled = enableSelector;
            _showPauseSawsButtonsSelector = enableSelector;
        }
    }
    
    private void _EnableSheepSelectors(bool enableSelectors)
    {
        if (_showSheepSelectors != enableSelectors)
        {
            foreach (Image selector in sheepSelectors)
            {
                selector.enabled = enableSelectors;
            }
            _showSheepSelectors = enableSelectors;
        }
    }
    
    private void _EnablePauseSheepIndicatorSelector(bool enableSelector)
    {
        if (_showPauseSheepIndicatorSelectors != enableSelector)
        {
            pauseSheepIndicatorSelector.enabled = enableSelector;
            _showPauseSheepIndicatorSelectors = enableSelector;
        }
    }
    
    private void _EnableSheepSpeedIndicatorSelector(bool enableSelector)
    {
        if (_showSheepSpeedIndicatorSelectors != enableSelector)
        {
            sheepSpeedIndicatorSelector.enabled = enableSelector;
            _showSheepSpeedIndicatorSelectors = enableSelector;
        }
    }

    private void _UpdatePageNumberText()
    {
        pageNumberText.text = (_tutorialLevel + 1) + "/" + (_maxLevel + 1);
    }

    public void QuitTutorial()
    {
        Destroy(gameObject);
        _gameController.StartCountDown();
        if (dontShowToggle.isOn)
        {
            PlayerPrefs.SetString("ShowTutorial", "dontshow");
        }
    }
}

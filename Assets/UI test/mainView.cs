using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class mainView : MonoBehaviour
{
    VisualTreeAsset m_ListEntryTemplate;

    void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Initialize the character list controller
        //var characterListController = new CharacterListController();
        //characterListController.InitializeCharacterList(uiDocument.rootVisualElement, m_ListEntryTemplate);
    }
}

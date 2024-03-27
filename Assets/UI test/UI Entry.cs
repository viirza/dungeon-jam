using UnityEngine.UIElements;

public class UIButtonEntry
{
    public void SetButtonElement(VisualElement visualElement)
    {
        visualElement.Q<Button>().clickable.clicked += buttonClick;
    }

    private void buttonClick()
    {

    }
}

using UnityEngine;

public class PanelIntroAnimator : MonoBehaviour
{
    public RectTransform panel;

    public float initialWidth = 200f;
    public float midWidth = 600f;
    
    public float height = 400f;

    public float firstDuration = 0.5f;
    public float delayBetween = 0.5f;
    public float secondDuration = 0.7f;

    private void Start()
    {
       
        

        panel.sizeDelta = new Vector2(initialWidth, height);


        LeanTween.size(panel, new Vector2(midWidth, height), firstDuration).setEase(LeanTweenType.easeOutQuad);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Slider slider;
    // Start is called before the first frame update

    void Start()
    {
        slider = transform.GetComponent<Slider>();
        if(slider == null)
            Debug.LogError("Slider not found!");
        if (healthBarRect == null)
        {
            Debug.LogError("No health Bar");
        }
        if (healthText == null)
        {
            Debug.LogWarning("No health Text");
        }
    }
    /*
    public void SetHealth(float _cur, float _max)
    {
        float _value = (float)_cur / _max;
        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y , healthBarRect.localScale.z);
        healthBarRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, _value);
        if(healthText!=null)
            healthText.text = _cur + "/" + _max + " HP";
    }
    */
    public void setMaxHealth(float health)
    {
        if (slider == null)
            Debug.LogError("Slider not found!");
        else
            slider.maxValue = health;
            slider.value = health;
    }
    public void SetHealth(float health)
    {
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

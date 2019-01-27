using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarmSlider : MonoBehaviour
{
    public bool isWarm;
    [SerializeField]
    private float freq;

    Slider _slider;
    void Start()
    {
        // スライダーを取得する
        _slider = GameObject.Find("Thermometer").GetComponent<Slider>();
    }

    float _hp = 100;
    float count;
    void Update()
    {
        //Debug.Log(count % 3);
        count += Time.deltaTime;
        if (count > freq)
        {
            if (isWarm)
            {
                // HP上昇
                _hp += 1f;
            }
            else
            {
                // HP減少
                _hp -= 1f;
            }

            if (_hp < 0) _hp = 0;
            if (_hp > 100) _hp = 100;

            count = 0;
        }

        // HPゲージに値を設定
        _slider.value = _hp;
    }
}

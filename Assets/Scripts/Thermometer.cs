﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Thermometer : MonoBehaviour
{
    Slider _slider;
    void Awake()
    {
        // スライダーを取得する
        _slider = GetComponent<Slider>();
    }

    public void SetHPObservable( IObservable<long> hpObservable ) {
        hpObservable.Subscribe((hp)=>{
            _slider.value = hp;
        });
    }
}

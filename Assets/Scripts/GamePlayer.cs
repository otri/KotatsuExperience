using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GamePlayer : MonoBehaviour
{
    public ReactiveProperty<long> CurrentHp;
    public ReactiveProperty<bool> IsWarm;
    public ReactiveProperty<bool> Working;

    public IObservable<long> CurrentHpObservable(){
        return CurrentHp;
    }

    private const float hpTickDuration = 0.1f;
    private const float WorkDuration = 5;

    float hpTickTimer;

    float _workTime;

    public void Init() {
        CurrentHp = new ReactiveProperty<long>(100);
        IsWarm = new ReactiveProperty<bool>(false);
        Working = new ReactiveProperty<bool>(false);
    }

    public void DoWorkUpdate() {
        //Debug.Log(count % 3);
        hpTickTimer += Time.deltaTime;
        if(hpTickTimer > hpTickDuration)
        {
            long hp = CurrentHp.Value;
            if (IsWarm.Value)
            {
                // HP上昇
                hp += 1;
            }
            else
            {
                // HP減少
                hp -= 1;
            }

            if (hp < 0) hp = 0;
            if (hp > 100) hp = 100;

            hpTickTimer = 0;
            CurrentHp.Value = hp;
        }

        if(Working.Value && Time.time > _workTime ) {
            Working.Value = false;
        }
    }

    public void SetActive( bool active ) {
        gameObject.SetActive(active);
    }

    public void DoWork() {
        _workTime = Time.time + WorkDuration;
        Working.Value = true;
    }
}

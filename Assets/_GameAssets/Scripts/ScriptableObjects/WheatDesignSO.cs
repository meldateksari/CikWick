using UnityEngine;

[CreateAssetMenu(fileName = "WheatDesignSO", menuName = "ScriptableObjects/WheatDesignSO")]
public class WheatDesignSO : ScriptableObject
{
//ScriptableObject, bir nesneye ait verileri (bilgileri) 
//tek bir yerde tutmak ve bunları farklı yerlerde, 
//tekrar tekrar kullanmak için tasarlanmıştır.
    [SerializeField] private float _increaseDecreaseMultiplier;
    [SerializeField] private float _resetBoostDuration;

    public float IncreaseDecreaseMultiplier => _increaseDecreaseMultiplier;
    public float ResetBoostDuration => _resetBoostDuration;
}

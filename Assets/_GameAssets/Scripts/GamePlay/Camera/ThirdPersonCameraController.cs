using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _orientationTransform;
    [SerializeField] private Transform _playerVisualTransform;
     
    [Header("Settings")]
    [SerializeField] private float _rotationSpeed;


//movement direction= hareket yönü
//orientation = bir nesnenin dönük olduğu yön
//visual=görsel
//slerp=Bir nesnenin bir yöne bakarken,
//zamanla başka bir yöne yumuşakça dönmesini sağlar
//normalized=birim vektör haline getirmektir.
//Time.deltaTime= update içinde optimizasyon etmek. 
//Input direction = hareket etme etmeme
    private void Update()
    {
        //kameraya gore yon belirleme
        Vector3 viewDirection =
        _playerTransform.position- new Vector3(transform.position.x,_playerTransform.position.y,transform.position.z);
       
        //bakılması gereken yön
        _orientationTransform.forward=viewDirection.normalized;
       
        //oyuncu girişi
       float  horizontalInput=Input.GetAxisRaw("Horizontal");
       float  verticalInput=Input.GetAxisRaw("Vertical");
       
       //Girdi yönünü kameraya göre hesapla
       Vector3 inputDirection =
       _orientationTransform.forward*verticalInput+ _orientationTransform.right* horizontalInput;
    
          //Görsel nesneyi input yönüne döndür
        if(inputDirection !=Vector3.zero){ //hareket ediyorsa
       _playerVisualTransform.forward =
       Vector3.Slerp(_playerVisualTransform.forward, inputDirection.normalized,Time.deltaTime*_rotationSpeed);

        }
    }


}

using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientationTransform;


    [Header("Movement Settigns")]
    [SerializeField] private KeyCode _movementKey;
    [SerializeField] private float _movementSpeed;


    [Header("Jump Settigns")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    //cooldown:ıpladıktan sonra tekrar zıplayabilmek için beklemen gereken süre
    [SerializeField] private bool _canJump;

     [Header("Sliding Settigns")]
     [SerializeField] private KeyCode _slideKey;
     [SerializeField] private float _slideMultiplier;
     [SerializeField] private float _slideDrag;

     [Header("Ground Check Settigns")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;

    private float _horizontalInput,_verticalInput;
    //vertical ileri-geri hareket girdisi z ekseni
    //horizontal sağ sol hareket girdisi x ekseni
    
    private Rigidbody _playerRigidbody;

    private Vector3 _movementDirection;

    private bool _isSliding;


    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation=true;
    }
    private void Update()
    {
        SetInputs();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
    }
    private void SetInputs(){
        _horizontalInput=Input.GetAxisRaw("Horizontal");
        _verticalInput=Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(_slideKey)){
            _isSliding=true;
        }
        else if (Input.GetKeyDown(_movementKey)){
            _isSliding=false;
        }
        else if(Input.GetKey(_jumpKey)&& _canJump && IsGrounded() ){//zıplama işlemi
          _canJump=false;
          SetPlayerJumping();
          Invoke(nameof(ResetJumping),_jumpCooldown);//invoke belirli bir sure sonra calistirir

        }
    }
    private void SetPlayerMovement(){
        //oyuncu hangi yone bakıyorsa o yonde hareket ettir.
         _movementDirection= _orientationTransform.forward * _verticalInput +
         _orientationTransform.right * _horizontalInput;
         //forward ileri dogru vektor
         //right sağ dogru vektor
         if(_isSliding){
         _playerRigidbody.AddForce(_movementDirection.normalized*_movementSpeed* _slideMultiplier,ForceMode.Force);
        //normalized aynı anda iki tuşa basıldıgında o tuşlar kadar İlerletir.
         }
         else{
            _playerRigidbody.AddForce(_movementDirection.normalized*_movementSpeed,ForceMode.Force);

         }

        
        
    }
    private void SetPlayerDrag()
{
    if (_isSliding)
    {
        _playerRigidbody.linearDamping = _slideDrag;
    }
    else
    {
        _playerRigidbody.linearDamping = _groundDrag;
    }
}
  private void LimitPlayerSpeed(){
           Vector3 flatVelocity=new Vector3(_playerRigidbody.linearVelocity.x,0f,_playerRigidbody.linearVelocity.z);
            if (flatVelocity.magnitude > _movementSpeed)
{
        Vector3 limitedVelocity = flatVelocity.normalized * _movementSpeed;
          _playerRigidbody.linearVelocity = new Vector3(
          limitedVelocity.x,
          _playerRigidbody.linearVelocity.y,
           limitedVelocity.z
    );
}
          }

    private void SetPlayerJumping(){
        //zıplamadan önce y ekseninde hız sınıfrlanır 
        _playerRigidbody.linearVelocity= new Vector3(_playerRigidbody.linearVelocity.x,0f,_playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(transform.up * _jumpForce,ForceMode.Impulse);
    }

         private void ResetJumping()//yardımcı fonksiyon
    {
        _canJump=true;
    }
    
    private bool IsGrounded(){
          return Physics.Raycast(transform.position,Vector3.down, _playerHeight *0.5f+0.2f, _groundLayer);
    }
}

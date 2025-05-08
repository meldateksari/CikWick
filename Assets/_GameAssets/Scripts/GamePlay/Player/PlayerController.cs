using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJumped;
    public event Action <PlayerState> OnPlayerStateChanged;

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
     [SerializeField] private float _airMultiplier;
      [SerializeField] private float _airDrag;
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
    
    private StateController _stateController;
    private Rigidbody _playerRigidbody;

    private float _startingMovementSpeed,_startingJumpForce;

    private Vector3 _movementDirection;

    private bool _isSliding;


    private void Awake()
    {
        _stateController=GetComponent<StateController>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation=true;

         _startingMovementSpeed=_movementSpeed;
        _startingJumpForce=_jumpForce;
    }
    private void Update()
    {
        SetInputs();
        SetStates();
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
private void SetStates(){
  var movementDirection =GetMovementDirection();
  var isGrounded =IsGrounded();
  var isSliding =IsSliding();
  var currentState =_stateController.GetCurrentState();
var newState = currentState switch
{
    _ when movementDirection == Vector3.zero && isGrounded && !isSliding => PlayerState.Idle,
    _ when movementDirection != Vector3.zero && isGrounded && !isSliding => PlayerState.Move,
    _ when movementDirection != Vector3.zero && isGrounded && isSliding  => PlayerState.Slide,
    _ when movementDirection == Vector3.zero && isGrounded && isSliding  => PlayerState.SlideIdle,
    _ when !_canJump && !isGrounded => PlayerState.Jump,
    _ => currentState
};

if (newState != currentState) //eger yeni durum eski durumdan farklıysa hareketi değiştir.
{
    _stateController.ChangeState(newState);
    OnPlayerStateChanged?.Invoke(newState);
}



}

    
    private void SetPlayerMovement(){
        //oyuncu hangi yone bakıyorsa o yonde hareket ettir.
         _movementDirection= _orientationTransform.forward * _verticalInput +
         _orientationTransform.right * _horizontalInput;
         //forward ileri dogru vektor
         //right sağ dogru vektor
         float forceMultiplier = _stateController.GetCurrentState() switch
{
    PlayerState.Move => 1f,
    PlayerState.Slide => _slideMultiplier,
    PlayerState.Jump => _airMultiplier,
    _ => 1f
};


        _playerRigidbody.AddForce(_movementDirection.normalized*_movementSpeed* forceMultiplier,ForceMode.Force);
        //normalized aynı anda iki tuşa basıldıgında o tuşlar kadar İlerletir.

        
        
    }
    private void SetPlayerDrag()

{_playerRigidbody.linearDamping = _stateController.GetCurrentState() switch
{
    PlayerState.Move => _groundDrag,
    PlayerState.Slide => _slideDrag,
    PlayerState.Jump => _airDrag,
    _ => _playerRigidbody.linearDamping
};

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

        OnPlayerJumped?.Invoke();
    
        //zıplamadan önce y ekseninde hız sınıfrlanır 
        _playerRigidbody.linearVelocity= new Vector3(_playerRigidbody.linearVelocity.x,0f,_playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(transform.up * _jumpForce,ForceMode.Impulse);
    }

         private void ResetJumping()//yardımcı fonksiyon
    {
        _canJump=true;
    }

    #region Helper Functions

    private bool IsGrounded(){
          return Physics.Raycast(transform.position,Vector3.down, _playerHeight *0.5f+0.2f, _groundLayer);
    }

private Vector3 GetMovementDirection(){
    return _movementDirection.normalized;
}
private bool IsSliding(){
    return _isSliding;
}

    internal static void _OnPlayerJumped()
    {
        throw new NotImplementedException();
    }

    public void SetMovementSpeed(float speed, float duration)
{
    _movementSpeed += speed;
    Invoke(nameof(ResetMovementSpeed), duration);
}

private void ResetMovementSpeed()
{
    _movementSpeed = _startingMovementSpeed;
}

public void SetJumpForce(float force, float duration)
{
    _jumpForce += force;
    Invoke(nameof(ResetJumpForce), duration);
}

private void ResetJumpForce()
{
    _jumpForce = _startingJumpForce;
}
public Rigidbody GetPlayerRigidbody(){
    return _playerRigidbody;
}


    #endregion
}

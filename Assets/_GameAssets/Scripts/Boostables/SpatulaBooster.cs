using UnityEngine;

public class SpatulaBooster : MonoBehaviour, IBoostable
{
    [SerializeField] private Animator _spatulaAnimator;
    [SerializeField] private float _jumpForce;

private bool _isActivated;

public void Boost(PlayerController playerController)
{
    if (_isActivated) { return; }

    PlayBoostAnimation();

    Rigidbody playerRigidbody = playerController.GetPlayerRigidbody();
    playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);
    playerRigidbody.AddForce(transform.forward * _jumpForce, ForceMode.Impulse);

    _isActivated = true;
    Invoke(nameof(ResetActivation), 0.2f);
}

private void PlayBoostAnimation()
{
    _spatulaAnimator.SetTrigger(Consts.OtherAnimations.IS_SPATULA_JUMPING);
}

private void ResetActivation()
{
    _isActivated = false;//_isActivated değişkeni, bu işlemin kısa sürede tekrar edilmesini engelleyen bir kontrol mekanizmasıdır (cooldown gibi).
}

}



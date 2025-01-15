using UnityEngine;
//Bat에 자식으로 들어가있으면 localPosition으로 계속 쫓아다니기때문에 속도 측정이안됨
namespace VRTeamProject01
{
    public class BatCapsuleFollower : MonoBehaviour
    {
        private BatCapsule _batFollower;
        private Rigidbody _rigidbody;
        private Vector3 _velocity;
        private Collider _collider;

        private float _sensitivity = 50.0f;

        public void Init()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            //enabled = false;
        }

        public void FixedUpdated()
        {
            Vector3 destination = _batFollower.transform.position;
            _rigidbody.transform.rotation = transform.rotation;

            _velocity = (destination - _rigidbody.transform.position) * _sensitivity;
            _rigidbody.velocity = _velocity;

            transform.rotation = _batFollower.transform.rotation;
        }

        public void SetFollowTarget(BatCapsule batFollower)
        {
            _batFollower = batFollower;
        }

        public void Enable()
        {
            //print("en");
            //_rigidbody.velocity = Vector3.zero;
            _collider.enabled = true;
            enabled = true;
        }

        public void Disable()
        {
            //print("dis");
            //_rigidbody.velocity = Vector3.zero;
            _collider.enabled = false;
            enabled = false;
        }
    }
}
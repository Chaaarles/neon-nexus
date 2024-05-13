using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintBoost;
    public CanvasGroup textCanvasGroup;
    public TextMeshProUGUI toolTip;
    public TextMeshProUGUI timePowerHUD;
    public GameObject pickup;

    private Camera _camera;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _movement;
    private Vector2 _mousePosition;

    private bool _isSprinting;
    private float _targetTextAlpha;
    private Areas _currentArea;
    private bool _isTapeRewound;
    private bool _hasSpaceHack;
    private float _timePower;

    private enum Areas
    {
        Rewind,
        Getaway,
        SpaceHack,
        Pickup,
        Default,
    }

    private void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        toolTip.text = "Bing bong";
        _timePower = 100;
    }

    private void Update()
    {
        textCanvasGroup.alpha = Mathf.Lerp(textCanvasGroup.alpha, _targetTextAlpha, 0.1f);
        _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _isSprinting = Input.GetKey(KeyCode.LeftShift) && _timePower > 0;
        Utils.Utils.TimeMultiplier = _isSprinting ? 0.2f : 1;
        if (_isSprinting) _timePower = Mathf.Max(_timePower - Time.deltaTime * 10, 0);
        
        
        timePowerHUD.text = $"Hack time: {_timePower:F2}%\n{(_hasSpaceHack ? "Hack space: Enabled" : "")}";

        
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (_currentArea)
            {
                case Areas.Rewind:
                    _isTapeRewound = true;
                    break;
                case Areas.Getaway when _isTapeRewound:
                    GameManager.Win();
                    break;
                case Areas.SpaceHack when _hasSpaceHack:
                    transform.Translate(0, 2, 0);
                    break;
                case Areas.Pickup:
                    Destroy(pickup);
                    _hasSpaceHack = true;
                    break;
                case Areas.Default:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _movement * (moveSpeed + (_isSprinting ? sprintBoost : 0f));
        _rigidbody2D.rotation = Mathf.LerpAngle(_rigidbody2D.rotation,
            Utils.Utils.AngleBetween(transform.position, _mousePosition), 0.4f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("ViewCone"))
        {
            Vector2 currentPosition = gameObject.transform.position;
            RaycastHit2D[] rayHits =
                Physics2D.RaycastAll(currentPosition,
                    ((Vector2)other.GetComponentInParent<EnemyBehaviour>().gameObject.transform.position -
                     currentPosition) * 2);

            foreach (RaycastHit2D rayHit in rayHits)
            {
                Debug.Log(rayHit.collider.tag);
                if (rayHit.collider.CompareTag("ViewCone")) continue;
                if (rayHit.collider.CompareTag("Player")) continue;
                if (rayHit.collider.CompareTag("Goon")) other.GetComponentInParent<EnemyBehaviour>().Rush();
                else break;
            }
        }
        else if (other.CompareTag("RewindZone"))
        {
            toolTip.text = _isTapeRewound ? "Great job! Now get out of here" : "Press 'E' to rewind the tapes";
            _currentArea = Areas.Rewind;
            _targetTextAlpha = 1f;
        }
        else if (other.CompareTag("Getaway"))
        {
            toolTip.text = _isTapeRewound ? "Press 'E' to get out of here" : "I gotta rewind those tapes!";
            _currentArea = Areas.Getaway;
            _targetTextAlpha = 1f;
        }
        else if (other.CompareTag("Pickup"))
        {
            toolTip.text = "Woah! A space hacking module";
            _currentArea = Areas.Pickup;
            _targetTextAlpha = 1f;
        }
        else if (other.CompareTag("SpaceHack"))
        {
            toolTip.text = _isTapeRewound ? "Press 'E' to hack space" : "Hmm, can't get through here";
            _currentArea = Areas.SpaceHack;
            _targetTextAlpha = 1f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("RewindZone")
            || other.CompareTag("Getaway")
            || other.CompareTag("Pickup")
            || other.CompareTag("SpaceHack"))
        {
            _targetTextAlpha = 0f;
            _currentArea = Areas.Default;
        }
    }
}
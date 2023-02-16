using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerMovement : MonoBehaviour {

    private CharacterController character_Controller;

    private Vector3 move_Direction, dodge_Direction, speech_Direction;

    public float speed = 5f;
    private float gravity = 20f;

    public float jump_Force = 10f;
    private float vertical_Velocity;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    void Awake() {
        character_Controller = GetComponent<CharacterController>();

        SpeechControll();
    }

    void Update () {
        MoveThePlayer();

        Dodge();

    }

    void MoveThePlayer() {
        // vector3(x,y,z)
        move_Direction = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f,
                                     Input.GetAxis(Axis.VERTICAL));

        move_Direction = transform.TransformDirection(move_Direction);
        move_Direction *= speed * Time.deltaTime;

        ApplyGravity();

        character_Controller.Move(move_Direction);


    } // move player

    void ApplyGravity() {
        //Van toc nhay
        vertical_Velocity -= gravity * Time.deltaTime;

        // jump
        PlayerJump();
        //Toa do nhay
        move_Direction.y = vertical_Velocity * Time.deltaTime;

    } // apply gravity

    void PlayerJump() {
        //nhay = space
        if(character_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            //van toc ban dau = luc nhay
            vertical_Velocity = jump_Force;
        }

    }

    void Dodge()
    {
        if (HealthScript.isDodge == true)
        {
            dodge_Direction = new Vector3(0f, 0f, -100f);

            dodge_Direction = transform.TransformDirection(dodge_Direction);
            dodge_Direction *= speed * Time.deltaTime;
            character_Controller.Move(dodge_Direction);
            HealthScript.isDodge = false;
        }
    }

    void SpeechControll()
    {
        actions.Add("forward", SpeechForward);
        actions.Add("left", SpeechLeft);
        actions.Add("right", SpeechRight);
        actions.Add("back", SpeechBack);
        actions.Add("jump", SpeechJump);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    void SpeechForward()
    {
        speech_Direction = new Vector3(0f, 0f, 100f);
        speech_Direction = transform.TransformDirection(speech_Direction);
        speech_Direction *= speed * Time.deltaTime;
        character_Controller.Move(speech_Direction);
    }

    void SpeechBack()
    {
        speech_Direction = new Vector3(0f, 0f, -100f);
        speech_Direction = transform.TransformDirection(speech_Direction);
        speech_Direction *= speed * Time.deltaTime;
        character_Controller.Move(speech_Direction);
    }

    void SpeechLeft()
    {
        speech_Direction = new Vector3(-100f, 0f, 0f);
        speech_Direction = transform.TransformDirection(speech_Direction);
        speech_Direction *= speed * Time.deltaTime;
        character_Controller.Move(speech_Direction);
    }

    void SpeechRight()
    {
        speech_Direction = new Vector3(100f, 0f, 0f);
        speech_Direction = transform.TransformDirection(speech_Direction);
        speech_Direction *= speed * Time.deltaTime;
        character_Controller.Move(speech_Direction);
    }

    void SpeechJump()
    {
        vertical_Velocity = jump_Force;
    }

} // class



































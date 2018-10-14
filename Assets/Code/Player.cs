using System;
using Assets.Code.Structure;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Assets.Code
{
    /// <summary>
    /// Player controller class
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Player : MonoBehaviour, ISaveLoad
    {
        private static string _fireaxis;
        private Rigidbody2D _rb;
        private Gun _gun;

        // ReSharper disable once UnusedMember.Global
        internal void Start () {
            _rb = GetComponent<Rigidbody2D>();
            _gun = GetComponent<Gun>();

            _fireaxis = Platform.GetFireAxis();
        }

        // ReSharper disable once UnusedMember.Global
        internal void Update () {
            HandleInput();
        }

        /// <summary>
        /// Check the controller for player inputs and respond accordingly.
        /// </summary>
        private void HandleInput () {
            // TODO fill me in
            var axis = Input.GetAxis(_fireaxis);
            if (axis > 0.1f)
            {
                print(">");
            }
            else if (axis < -0.1f)
            {
                print("<");
            }
            print(axis);
            
        }

        private void Turn (float direction) {
            if (Mathf.Abs(direction) < 0.02f) { return; }
            _rb.AddTorque(direction * -0.05f);
        }

        private void Thrust (float intensity) {
            if (Mathf.Abs(intensity) < 0.02f) { return; }
            _rb.AddRelativeForce(Vector2.up * intensity);
        }

        private void Fire () {
            print("firing");
            _gun.Fire();
        }

        #region saveload

        public GameData OnSave () {
            PlayerGameData player = new PlayerGameData();
            player.Pos = _rb.position;
            player.Velocity = _rb.velocity;
            player.Rotation = _rb.rotation;
            player.AngularVelocity = _rb.angularVelocity;
            return player;
        }

        public void OnLoad (GameData data) {
            PlayerGameData player = data as PlayerGameData;

            _rb.position = player.Pos;
            _rb.velocity = player.Velocity;
            _rb.rotation = player.Rotation;
            _rb.angularVelocity = player.AngularVelocity * Mathf.Deg2Rad;
        }

        #endregion

        void OnGUI()
        {
            GUI.Label(new Rect(50, 50, 100, 20), Input.GetAxis(_fireaxis).ToString());
        }
    }

    public class PlayerGameData : GameData
    {
        public Vector2 Pos;
        public Vector2 Velocity;
        public float Rotation;
        public float AngularVelocity; // reaed as DEGREES but stored as RADIANS; COME ON UNITY
    }
}

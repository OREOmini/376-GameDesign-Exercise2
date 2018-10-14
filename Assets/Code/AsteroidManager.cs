using System;
using System.Collections.Generic;
using Assets.Code.Structure;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Code
{
    /// <inheritdoc><cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Manager class for spawning and tracking all of the game's asteroids
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AsteroidManager : MonoBehaviour, ISaveLoad
    {
        private const float SpawnTime = 3f;
        private const int MaxAsteroidCount = 8;
        private static Object _asteroidPrefab;
        private float _lastspawn;
        private Transform _holder;

        // ReSharper disable once UnusedMember.Global
        internal void Start () {
            _asteroidPrefab = Resources.Load("Asteroid");
            _holder = transform;
            Asteroid.Manager = this;
        }

        // ReSharper disable once UnusedMember.Global
        internal void Update () {
            if ((Time.time - _lastspawn) < SpawnTime) return;
            _lastspawn = Time.time;
            Spawn();
        }

        private void Spawn () {
            if (_holder.childCount >= MaxAsteroidCount) { return; }

            var pos = BoundsChecker.GetRandomPos();
            var vel = BoundsChecker.GetRandomVelocity();
            int size = Random.Range(2, Asteroid.AsteroidTypes); // don't spawn tinies

            ForceSpawn(pos, vel, size);
        }

        // TODO fill me in
        public void ForceSpawn (Vector2 pos, Vector2 velocity, int size, Quaternion rotation = new Quaternion()) {
            GameObject asteroidObject = Object.Instantiate(_asteroidPrefab, pos, rotation) as GameObject;
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
            asteroid.Initialize(velocity, size);

            asteroid.transform.parent = _holder;
            print("add asteroid");
        }

        #region saveload

        public GameData OnSave () {
            //throw new NotImplementedException();
            AsteroidsData asteroidsData = new AsteroidsData();

            foreach(Asteroid asteroid in _holder.GetComponents<Asteroid>()) {
                AsteroidData asteroidData = new AsteroidData();
                asteroidData.Size = asteroid.Size;
                asteroidData.Pos = asteroid.GetComponent<Rigidbody2D>().position;
                asteroidData.Velocity = 
                    asteroid.GetComponent<Rigidbody2D>().velocity;

                asteroidsData.Asteroids.Add(asteroidData);
            }

            return asteroidsData;
        }

        public void OnLoad (GameData data) {
            //throw new NotImplementedException();
            var list = _holder.GetComponents<Asteroid>();

            foreach (Asteroid asteroid in list) {
                Destroy(asteroid);
            }

            AsteroidsData asteroidsData = data as AsteroidsData;
            foreach (AsteroidData asteroid in asteroidsData.Asteroids) {
                ForceSpawn(asteroid.Pos, asteroid.Velocity, asteroid.Size);
            }
        }

        #endregion
    }

    /// <summary>
    /// The save data for all the asteroids
    /// </summary>
    public class AsteroidsData : GameData
    {
        public List<AsteroidData> Asteroids = new List<AsteroidData>();
    }

    /// <summary>
    /// The save data for one asteroid
    /// </summary>
    public class AsteroidData
    {
        public int Size;
        public Vector2 Pos;
        public Vector2 Velocity;
    }
}

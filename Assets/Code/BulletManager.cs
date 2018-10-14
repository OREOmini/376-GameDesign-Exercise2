﻿using System;
using System.Collections.Generic;
using Assets.Code.Structure;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Xml.Serialization;
using System.IO;

namespace Assets.Code
{
    /// <summary>
    /// Bullet manager for spawning and tracking all of the game's bullets
    /// </summary>
    public class BulletManager : ISaveLoad
    {
        private readonly Transform _holder;

        /// <summary>
        /// Bullet prefab. Use GameObject.Instantiate with this to make a new bullet.
        /// </summary>
        private readonly Object _bullet;

        public BulletManager (Transform holder) {
            _holder = holder;
            _bullet = Resources.Load("Bullet");
        }

        public void ForceSpawn (Vector2 pos, Quaternion rotation, Vector2 velocity, float deathtime) {
            //Bullet bulletObject = new Bullet();
            GameObject bulletObject = Object.Instantiate(_bullet, pos, rotation) as GameObject;
            Debug.Log(bulletObject.GetComponent<Bullet>());

            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.Initialize(velocity, deathtime);


            //Bullet bullet = bulletObject.GetComponent(typeof(Bullet)) as Bullet;

            //bulletObject.Initialize(velocity, deathtime);

            bulletObject.transform.parent = _holder;
        }

        #region saveload

        public GameData OnSave () {

            BulletsData BulletsData = new BulletsData();
            var bullets = Object.FindObjectsOfType(typeof(Bullet));

            //foreach (Bullet bullet in bullets) {
            //    BulletData bulletData = new BulletData();
            //    bulletData.Pos = bullet.transform.position;
            //    // TODO Quaternion rotation convert
            //    bulletData.Rotation = 0.0f;
            //    bulletData.Velocity = bullet.GetComponent<Rigidbody2D>().velocity;

            //    BulletsData.Bullets.Add(bulletData);
            //}

            foreach (Transform item in _holder) {
                //Bullet b = item as Bullet;
                BulletData bulletData = new BulletData();
                bulletData.Pos = item.position;

                // TODO Quaternion rotation convert
                bulletData.Rotation = 0.0f;

                Bullet bullet = item.GetComponent<Bullet>();
                bulletData.Velocity = bullet.GetComponent<Rigidbody2D>().velocity;

                BulletsData.Bullets.Add(bulletData);
            }


            return BulletsData;
        }


        public void OnLoad (GameData data) {
            var list = _holder.GetComponents<Bullet>();
            foreach (Bullet bullet in list) {
                Object.Destroy(bullet);
            }

            BulletsData bullets = data as BulletsData;

            foreach (BulletData bullet in bullets.Bullets) {
                ForceSpawn(bullet.Pos, Quaternion.Euler(0, 0,bullet.Rotation), bullet.Velocity, Bullet.Lifetime);
            }
        }

        #endregion

    }

    /// <summary>
    /// Save data for all bullets in game
    /// </summary>
    public class BulletsData : GameData
    {
        public List<BulletData> Bullets;
    }

    /// <summary>
    /// Save data for a single bullet
    /// </summary>
    public class BulletData
    {
        public Vector2 Pos;
        public Vector2 Velocity;
        public float Rotation;
    }
}
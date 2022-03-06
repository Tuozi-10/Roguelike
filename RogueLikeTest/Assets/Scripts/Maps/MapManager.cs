using System;
using System.Collections.Generic;
using Bonuses;
using Controller;
using DG.Tweening;
using Menus;
using Projectiles;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Random = UnityEngine.Random;

namespace Maps
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager instance;

        [SerializeField] private GameObject[] m_mapDoors;

        [SerializeField] private List<Map> m_maps = new List<Map>();
        [SerializeField] private Map m_hub;
        [SerializeField] private List<Map> m_mapsBoss = new List<Map>();
        [SerializeField] private MapShop m_shop;
        
        
        [SerializeField] private int m_mapCount = 6;
        [SerializeField] private List<int> m_mapShopId = new List<int> { 3, 7, 12};

        [SerializeField] private CanvasGroup m_bgTransition;

        public static readonly List<Bonus> shopItems = new List<Bonus>();
        
        private Map m_currentMap;

        [SerializeField] private List<int> m_bossIndex = new List<int>() { 4, 8, 13};
        private doors m_fromDoor = doors.bottom;

        private readonly Queue<Map> m_mapsQueue = new Queue<Map>();
        private readonly Queue<Map> m_mapsBossQueue = new Queue<Map>();

        public Transform transformCurrentMap => m_currentMap.transform;
        
        private int m_countMapsDone = -1;
        
        public enum doors
        {
            top = 0,
            left = 1,
            bottom = 2,
            right = 3
        }

        public static void RefreshShopTexts()
        {
            foreach (var shopItem in shopItems)
            {
                if (shopItem != null)
                {
                    shopItem.RefreshTextColor();
                }
            }
        }
        
        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
        }

        /// <summary>
        /// simple procedural generation taking a random map in a list and adding it to our queue
        /// </summary>
        public void GenerateProceduralMap()
        {
            if(m_currentMap != null) Destroy(m_currentMap.gameObject);
            m_countMapsDone = -1;
            var maps = new List<Map>(m_maps);
            var mapsBoss = new List<Map>(m_mapsBoss);
            m_mapsQueue.Clear();
            m_mapsBossQueue.Clear();
            
            var countMaps = Mathf.Min(m_mapCount, maps.Count); // just in case the gd were drunk and requested 50 maps when we only had 5

            for (int i = 0; i < countMaps; i++)
            {
                var j = Random.Range(0, maps.Count);
                m_mapsQueue.Enqueue(maps[j]);
                maps.RemoveAt(j);
            }
            
            for (int i = 0; i < m_mapsBoss.Count; i++)
            {
                var j = Random.Range(0, mapsBoss.Count);
                m_mapsBossQueue.Enqueue(mapsBoss[j]);
                mapsBoss.RemoveAt(j);
            }
            
        }

        public void CheckMapCompleted()
        {   
            var done = m_currentMap.mapDone;

            if (!done) return;
            
            for (var i = 0; i < (int) doors.right+1; i++)
            {
                if (m_fromDoor == (doors) i) continue;
                
                m_mapDoors[i].GetComponent<Animator>().Play("DoorOpen");
                ManageDoorTrigger((doors)i, true);
            }
        }

        private void ManageDoorEnabling(doors index, bool state)
        {
            m_mapDoors[(int) index].SetActive(state);
            ManageDoorTrigger(index, false);
        }

        private void ManageDoorTrigger(doors index, bool state)
        {
            m_mapDoors[(int) index].GetComponent<Collider2D>().enabled = state;
        }
        
        public void DisplayNextMap(doors fromDoor)
        {
            if (m_mapsBossQueue.Count == 0)
            {
                m_fromDoor = doors.bottom;
                MenuManager.instance.DoWin();
                return;
            }
            
            m_fromDoor = fromDoor switch
            {
                doors.top => doors.bottom,
                doors.left => doors.right,
                doors.bottom => doors.top,
                doors.right => doors.left,
                _ => m_fromDoor
            };
            
            m_bgTransition.DOFade(1, 0.25f).OnComplete(() =>
            {
                if(m_currentMap != null) Destroy(m_currentMap.gameObject);

                m_currentMap = GetMap();
                if(m_countMapsDone != 0) PlacePlayerAtDoor(m_fromDoor);
                CleanMap();
                m_bgTransition.DOFade(0, 0.25f);
            });
        }

        public void CleanMap()
        {
            BloodBathManager.instance.CleanMap();
            ProjectileManager.instance.CleanMap();
        }
        
        public void PlacePlayerAtDoor(doors door)
        {
            PlayerController.instance.ResetVelocity();
            switch (door)
            {
                case doors.top:
                    PlayerController.instance.transform.position = new Vector3(0,7.75f,0);
                    break;
                case doors.left:
                    PlayerController.instance.transform.position = new Vector3(-17.5f,-0.5f,0);
                    break;
                case doors.bottom:
                    PlayerController.instance.transform.position = new Vector3(0,-8.8f,0);
                    break;
                case doors.right:
                    PlayerController.instance.transform.position = new Vector3(17.5f,-0.5f,0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(door), door, null);
            }
            
            CameraController.instance.SetCamAtPos(PlayerController.instance.transform.position );
        }
        
        public Map GetMap()
        {
            m_countMapsDone++;

            // HUB
            if (m_countMapsDone == 0)
            {
                ManageDoorEnabling(0, true);
                m_mapDoors[0].GetComponent<Animator>().Play("DoorOpen");
                ManageDoorTrigger(0, true);
                return Instantiate(m_hub);
            }

            //SHOP
            if (m_mapShopId.Contains(m_countMapsDone))
            {
                for (var i = 0; i < (int) doors.right+1; i++)
                {
                    ManageDoorEnabling((doors)i, true);
                    m_mapDoors[i].GetComponent<Animator>().Play("DoorIdle");
                }
                MapManager.shopItems.Clear();
                return Instantiate(m_shop);;
            }

            // BOSS OR NORMAL
            for (var i = 0; i < (int) doors.right+1; i++)
            {
                ManageDoorEnabling((doors)i, true);
                m_mapDoors[i].GetComponent<Animator>().Play("DoorIdle");
            }

            if (m_bossIndex.Contains(m_countMapsDone))
            {
                AudioManager.instance.PlaySound(AudioManager.sounds.BossEntrance, 0.55f);
                return Instantiate(m_mapsBossQueue.Dequeue());
            }
            
            return Instantiate(m_mapsQueue.Dequeue());
        }
    }
}
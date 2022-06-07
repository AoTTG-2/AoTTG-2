using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Room;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Humans.Customization;
using UnityEngine;
using Assets.Scripts.UI.Camera;
using Assets.Scripts.Utility;
using Assets.Scripts.Events;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Services
{
    public class SpawnService : MonoBehaviour, ISpawnService
    {
        private IEntityService EntityService => Service.Entity;

        private readonly List<Spawner> spawners = new List<Spawner>();
        private static GamemodeBase Gamemode => FengGameManagerMKII.Gamemode;
        
        public event OnPlayerSpawn<Entity> OnPlayerSpawn;
        public void Add(Spawner spawner)
        {
            spawners.Add(spawner);
        }

        public void Remove(Spawner spawner)
        {
            spawners.Remove(spawner);
        }

        public List<T> GetAll<T>() where T : Spawner
        {
            return spawners.OfType<T>().ToList();
        }

        public T GetRandom<T>() where T : Spawner
        {
            var typedSpawners = GetAll<T>();
            if (typedSpawners.Count == 0) return null;
            return typedSpawners[Random.Range(0, typedSpawners.Count)];
        }
        
        public List<HumanSpawner> GetByType(PlayerSpawnType type)
        {
            return GetAll<HumanSpawner>().Where(x => x.Type == type).ToList();
        }

        public List<TitanSpawner> GetByType(TitanSpawnerType type)
        {
            return GetAll<TitanSpawner>().Where(x => x.Type == type).ToList();
        }

        public (Vector3 position, Quaternion rotation) GetRandomSpawnPosition()
        {
            var position = new Vector3(Random.Range(-550, 550f), 0f, Random.Range(-550f, 500f));
            if (position.x >= 0f && position.x <= 50f) position.x = 50f;
            if (position.x <= 0f && position.x >= -50f) position.x = -50f;
            if (position.z >= 0f && position.z <= 50f) position.z = 50f;
            if (position.z <= 0f && position.z >= -50f) position.z = -50f;

            var rotation = new Vector3(0f, Random.Range(0, 360f), 0f);
            return (position, Quaternion.Euler(rotation));
        }

        public void OnRestart()
        {
        }

        public T Spawn<T>() where T : Entity
        {
            var type = typeof(T);
            if (type == typeof(PlayerTitan))
            {
                return Spawn<T>(new TitanConfiguration());
            }

            throw new ArgumentException($"{type} is not implemented");
        }

        public T Spawn<T>(EntityConfiguration configuration) where T : Entity
        {
            var type = typeof(T);
            if (type == typeof(PlayerTitan))
            {
                return SpawnPlayerTitan() as T;
            }

            if (type == typeof(MindlessTitan))
            {
                return SpawnTitan("MindlessTitan", configuration as TitanConfiguration) as T;
            }

            throw new ArgumentException($"{type} is not implemented");
        }

        public T Spawn<T>(Vector3 position, Quaternion rotation, EntityConfiguration configuration) where T : Entity
        {
            var type = typeof(T);
            if (type == typeof(MindlessTitan))
            {
                return SpawnTitan("MindlessTitan", position, rotation, configuration as TitanConfiguration) as T;
            }

            if (type == typeof(FemaleTitan))
                return SpawnTitan("FemaleTitan", position, rotation, null) as T;

            if (type == typeof(ColossalTitan))
                return SpawnTitan("ColossalTitan", position, rotation, null) as T;

            if (type == typeof(ErenTitan))
                return SpawnTitan("ErenTitan", position, rotation, null) as T;

            throw new ArgumentException($"{type} is not implemented");
        }

        public T Spawn<T>(Vector3 position, Quaternion rotation, CharacterPreset preset, Faction faction = null) where T : Human
        {
            var type = typeof(T);
            if (type == typeof(Hero))
            {
                return SpawnHero("Hero", position, rotation, preset, faction) as T;
            }

            throw new ArgumentException($"{type} is not implemented");
        }

        private CharacterPreset LastUsedPreset { get; set; }
        private Faction LastUsedFaction { get; set; }
        private Hero SpawnHero(string prefab, Vector3 position, Quaternion rotation, CharacterPreset preset, Faction faction = null)
        {
            preset ??= LastUsedPreset;
            faction ??= LastUsedFaction;
            var human = PhotonNetwork.Instantiate(prefab, position, rotation, 0).GetComponent<Hero>();
            if (faction != null)
            { human.Faction = faction; }
            human.Initialize(preset);
            LastUsedPreset = preset;
            LastUsedFaction = faction;
            return human;
        }

        private TitanBase SpawnTitan(string prefab, TitanConfiguration configuration)
        {
            var spawn = GetRandom<TitanSpawner>();
            return SpawnTitan(prefab, spawn.transform.position, spawn.transform.rotation, configuration);
        }

        private TitanBase SpawnTitan(string prefab, Vector3 position, Quaternion rotation, TitanConfiguration configuration)
        {
            var titan = PhotonNetwork.Instantiate(prefab, position, rotation, 0).GetComponent<TitanBase>();
            titan.Initialize(configuration);
            return titan;
        }

        private PlayerTitan SpawnPlayerTitan()
        {
            var id = "TITAN";
            var tag = "titanRespawn";
            var location = Gamemode.GetPlayerSpawnLocation(tag);
            Vector3 position = location.transform.position;
            var titanSpawners = GetAll<TitanSpawner>().Where(x => x.Type == TitanSpawnerType.None).ToArray();
            if (titanSpawners.Length > 0) // RC Custom Map Spawns
            {
                position = titanSpawners[UnityEngine.Random.Range(0, titanSpawners.Length)].gameObject.transform.position;
            }
            PlayerTitan playerTitan = PhotonNetwork.Instantiate("PlayerTitan", position, new Quaternion(), 0).GetComponent<PlayerTitan>();
            playerTitan.Initialize(Gamemode.GetPlayerTitanConfiguration());
            Service.Player.Self = playerTitan;
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("dead", false);
            ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.isTitan, 2);
            propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            return playerTitan;
        }

        /// <summary>
        /// Slightly modified from FengGameManagerMKII. TODO refactor
        /// </summary>
        public void SpawnPlayer(string id, string tag = "playerRespawn", CharacterPreset preset = null, Faction faction = null)
        {
            if (id == null)
            {
                id = "1";
            }
            //myLastRespawnTag = tag;
            var location = Gamemode.GetPlayerSpawnLocation(/*tag*/);

            this.SpawnPlayerAt2(id, location, preset, faction);
        }

        /// <summary>
        /// Slightly modified from FengGameManagerMKII. TODO refactor
        /// </summary>
        public void SpawnPlayerAt2(string id, GameObject pos, CharacterPreset preset = null, Faction faction = null)
        {
            //if (faction != null) { Debug.Log($"SpawnPlayerAt2 called with argument Faction = {faction.Name}"); }
            // HACK
            //if (false)
            //if (!logicLoaded || !customLevelLoaded)
            //{
            //    this.NOTSpawnPlayerRC(id);
            //}
            //else
            //{
                Vector3 position = pos?.transform.position ?? new Vector3(0f, 5f, 0f);
                /*if (this.racingSpawnPointSet)
                {
                    position = this.racingSpawnPoint;
                }
                else*/
                {
                    if (FengGameManagerMKII.Level.IsCustom)
                    {
                        if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 0)
                        {
                            position = Service.Spawn.GetRandom<HumanSpawner>()?.gameObject.transform.position ?? new Vector3();
                        }
                        else if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 1)
                        {
                            var cyanSpawners = Service.Spawn.GetByType(PlayerSpawnType.Cyan);
                            if (cyanSpawners.Count > 0)
                            {
                                position = cyanSpawners[UnityEngine.Random.Range(0, cyanSpawners.Count)].gameObject.transform
                                    .position;
                            }
                        }
                        else if ((RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 2))
                        {
                            var magentaSpawners = Service.Spawn.GetByType(PlayerSpawnType.Magenta);
                            if (magentaSpawners.Count > 0)
                            {
                                position = magentaSpawners[UnityEngine.Random.Range(0, magentaSpawners.Count)].gameObject.transform
                                    .position;
                            }
                        }
                    }
                }
                IN_GAME_MAIN_CAMERA component = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>();
            //this.myLastHero = id.ToUpper();
            /*if (myLastHero == "ErenTitan")
            {
                component.SetMainObject(PhotonNetwork.Instantiate("ErenTitan", position, pos?.transform.rotation ?? new Quaternion(), 0),
                    true, false);
            }
            else
            {*/
            var hero = Spawn<Hero>(position + new Vector3(0, 5f, 0), pos?.transform.rotation ?? new Quaternion(), preset, faction);
                    component.SetMainObject(hero.transform.gameObject, true, false);
                    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add("dead", false);
                    ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
                    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                    hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.isTitan, 1);
                    propertiesToSet = hashtable;
                    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                //}

                component.enabled = true;
                SpectatorMode.Disable();
                GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
                component.gameOver = false;
                Service.Player.Self = component.main_object.GetComponent<Entity>();

            component.SetMainObject(hero.transform.gameObject, true, false);
            //}
            OnPlayerSpawn?.Invoke(hero);
        }

        private void LateUpdate()
        {

        }
    }
}

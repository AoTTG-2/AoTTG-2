﻿using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Room;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Humans.Customization;
using UnityEngine;
using Assets.Scripts.Events;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Services
{
    public class SpawnService : UnityEngine.MonoBehaviour, ISpawnService
    {
        private IEntityService EntityService => Service.Entity;
        private IMessageService MessageService => Service.Message;

        private List<Spawner> spawners;
        private static GamemodeBase Gamemode => FengGameManagerMKII.Gamemode;
        public event OnPlayerSpawn<Entity> OnPlayerSpawn;
        public event OnPlayerDespawn<Entity> OnPlayerDespawn;
        public event OnTitanKilled<Entity, Entity> OnTitanKilled;
        public void Add(Spawner spawner)
        {
            spawners.Add(spawner);
        }

        public void Remove(Spawner spawner)
        {
            spawners.Remove(spawner);
        }

        public void RemoveAllSpawners()
        {
            foreach (Spawner spawner in spawners)
            {
                spawners.Remove(spawner);
                Destroy(spawner);
            }
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

        #region Spawn overloads
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

        public T Spawn<T>(Vector3 position, Quaternion rotation, CharacterPreset preset) where T : Human
        {
            var type = typeof(T);
            if (type == typeof(Hero))
            {
                return SpawnHero("Hero", position, rotation, preset) as T;
            }

            throw new ArgumentException($"{type} is not implemented");
        }
        #endregion

        /// <summary>
        /// Last CharacterPreset used by the player.
        /// </summary>
        public CharacterPreset LastUsedPreset { get; set; }

        /// <summary>
        /// Last used titan configuration used by the player.
        /// </summary>
        private TitanConfiguration LastUsedTitanConfiguration { get; set; }
        private Hero SpawnHero(string prefab, Vector3 position, Quaternion rotation, CharacterPreset preset)
        {
            preset ??= LastUsedPreset;
            var human = PhotonNetwork.Instantiate(prefab, position, rotation, 0).GetComponent<Hero>();
            human.Initialize(preset);
            LastUsedPreset = preset;
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

        public Spawner RespawnSpawner { get; set; }
        public Faction LastUsedFaction { get; set; }
        public Hero SpawnPlayer(HumanSpawner spawner = null, CharacterPreset preset = null, Faction faction = null)
        {
            if (Service.Player.Self != null)
            {
                //despawns the current character before spawning a new one.
                GameObject.Destroy(Service.Player.Self.gameObject);
            }
            if (faction == null)
            {
                if (LastUsedFaction != null)
                { faction = LastUsedFaction; }
                else { faction = Service.Faction.GetHumanity(); }
            }
            Hero hero;

            //If a spawner isn't given, first it tries to spawn at the last used spawner (RespawnSpawner)
            //If RespawnSpawner is null, then it tries a random spawner. If that is null, then it tries using the legacy tag system.
            spawner ??= (RespawnSpawner as HumanSpawner ?? GetRandom<HumanSpawner>());

            if (spawner != null)
            {
                hero = SpawnHero("Hero", spawner.transform.position, spawner.transform.rotation, preset);
            }
            else
            {
                //legacy spawning system
                var spawnLocations = GameObject.FindGameObjectsWithTag("playerRespawn");
                if (!spawnLocations.Any())
                {
                    Debug.LogError("No valid spawn locations are available.");
                    return null;
                }
                var spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Count())];
                hero = SpawnHero("Hero", spawnLocation.transform.position, spawnLocation.transform.rotation, preset);
            }
            Service.Player.Self = hero;
            Service.Player.SetFaction(faction);
            OnPlayerSpawn?.Invoke(hero);

            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("dead", false);
            ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.isTitan, 1);
            propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);

            RespawnSpawner = spawner;
            LastUsedFaction = faction;
            return hero;
        }

        private PlayerTitan SpawnPlayerTitan(TitanSpawner spawner = null, TitanConfiguration titanConfiguration = null, Faction faction = null)
        {
            if (Service.Player.Self != null)
            {
                //Despawns the current player before spawning a new one.
                GameObject.Destroy(Service.Player.Self.gameObject);
            }
            if (faction == null)
            {
                if (LastUsedFaction != null)
                { faction = LastUsedFaction; }
                else { faction = Service.Faction.GetTitanity(); }
            }
            spawner ??= (RespawnSpawner as TitanSpawner ?? GetRandom<TitanSpawner>());

            PlayerTitan playerTitan;

            if (spawner != null)
            {
                var spawnTransform = spawner.gameObject.transform;
                playerTitan = PhotonNetwork.Instantiate("PlayerTitan", spawnTransform.position, spawnTransform.rotation, 0).GetComponent<PlayerTitan>();
            }
            else
            {
                //legacy spawning system
                var spawnLocations = GameObject.FindGameObjectsWithTag("titanRespawn");
                if (!spawnLocations.Any())
                {
                    Debug.LogError("No valid spawn locations are available.");
                    return null;
                }
                var spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Count())];
                var spawnTransform = spawnLocation.gameObject.transform;
                playerTitan = PhotonNetwork.Instantiate("PlayerTitan", spawnTransform.position, spawnTransform.rotation, 0).GetComponent<PlayerTitan>();
            }
            titanConfiguration ??= (LastUsedTitanConfiguration ?? Gamemode.GetPlayerTitanConfiguration());
            playerTitan.Initialize(titanConfiguration);

            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("dead", false);
            ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.isTitan, 2);
            propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);

            Service.Player.Self = playerTitan;
            Service.Player.SetFaction(faction);
            OnPlayerSpawn?.Invoke(playerTitan);

            RespawnSpawner = spawner;
            LastUsedFaction = faction;
            LastUsedTitanConfiguration = titanConfiguration;
            return playerTitan;
        }

        public void InvokeOnPlayerDespawn(Entity entity)
        {
            OnPlayerDespawn?.Invoke(entity);
        }
        [PunRPC]
        public void RespawnRpc(PhotonMessageInfo info)
        {
            if (!info.sender.IsMasterClient) return;
            Respawn(PhotonNetwork.player);
        }

        public void Respawn(PhotonPlayer player)
        {
            //if the player is living, then delete the old player first.
            if (player.CustomProperties[PhotonPlayerProperty.dead] == null
                || !RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead])
                || !GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
            {
                if (Service.Player.Self == null)
                {
                    Debug.LogError("Attempted to respawn a living player, but Service.Player.Self is not set.");
                    return;
                }
                GameObject.Destroy(Service.Player.Self.gameObject);
            }

            var isPlayerTitan = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2;
            if (RespawnSpawner == null
                || (LastUsedPreset == null && !isPlayerTitan)
                || (LastUsedTitanConfiguration == null && isPlayerTitan))
            {
                Debug.LogError("Cannot respawn a player that hasn't spawned yet, or no valid respawn spawner exists.");
            }
            else if (isPlayerTitan)
            {
                Service.Spawn.Spawn<PlayerTitan>();
            }
            else
            {
                Service.Spawn.SpawnPlayer();
            }
            Service.Message.Local("<color=#FFCC00>You have been revived by the master client.</color>", UI.DebugLevel.Default);
        }

        private IEnumerator respawnAllDeadPlayers(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (((player.CustomProperties[PhotonPlayerProperty.RCteam] == null)
                    && RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead])))
                {
                    Service.Photon.GetPhotonView().RPC(nameof(RespawnRpc), player, new object[0]);
                }
            }
        }
        public bool IsRespawning()
        {
            return isRespawning;
        }
        private bool isRespawning = false;
        public IEnumerator WaitAndRespawn(float time)
        {
            isRespawning = true;
            yield return new WaitForSeconds(time);
            SpawnPlayer();
            isRespawning = false;
        }
        public IEnumerator WaitAndRespawnAt(float time, Spawner spawner)
        {
            isRespawning = true;
            yield return new WaitForSeconds(time);
            SpawnPlayer(spawner as HumanSpawner);
            isRespawning = false;
        }
        public void NOTSpawnPlayer(string id = "2")
        {
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("dead", true);
            ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.isTitan, 1);
            propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null, true, false);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(true);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }

        private void SpawnService_OnLevelLoaded(int scene, Level level)
        {
            isRespawning = false;
        }

        public void InvokeOnTitanKilled(Entity human, Entity killedTitan, int damage)
        {
            OnTitanKilled?.Invoke(human, killedTitan, damage);
        }

        private void LateUpdate()
        {

        }
        private void Awake()
        {
            spawners = new List<Spawner>();
            Service.Level.OnLevelLoaded += SpawnService_OnLevelLoaded;
        }
    }
}

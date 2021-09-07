using Assets.Scripts.Characters.Humans;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Gamemode.Racing
{
    public class RacingObjective : MonoBehaviour
    {
        private RacingGamemode Gamemode { get; } = FengGameManagerMKII.Gamemode as RacingGamemode;
        public enum ObjectiveState { Queue, Taken, Current, Next }

        private ObjectiveState _state;
        public ObjectiveState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnStateChanged(value);
            }
        }
        private Hero Hero { get; set; }
        [SerializeField] private AudioSource audioSource;

        private void OnStateChanged(ObjectiveState state)
        {
            switch (state)
            {
                case ObjectiveState.Queue:
                    SetColor(Color.red, Color.gray);
                    break;
                case ObjectiveState.Taken:
                    SetColor(Color.gray, Color.gray);
                    break;
                case ObjectiveState.Current:
                    SetColor(Color.blue, Color.yellow);
                    if (NextObjective != null)
                    {
                        NextObjective.State = ObjectiveState.Next;
                    }
                    break;
                case ObjectiveState.Next:
                    SetColor(new Color(1f, 0.39f, 0f), Color.yellow);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void SetColor(Color evenColor, Color unevenColor)
        {
            var renderers = gameObject.transform.parent.GetComponentsInChildren<Renderer>();
            for (var i = 0; i < renderers.Length; i++)
            {
                var materialBlock = new MaterialPropertyBlock();
                renderers[i].GetPropertyBlock(materialBlock);
                materialBlock.SetColor("_BaseColor", i % 2 == 0 ? evenColor : unevenColor);
                renderers[i].SetPropertyBlock(materialBlock);
            }
        }

        public RacingObjective NextObjective { get; set; }
        public int Order;

        public void Current()
        {
            State = ObjectiveState.Current;
        }

        public void Queue()
        {
            State = ObjectiveState.Queue;
        }

        private void Start()
        {
            OnStateChanged(_state);
            if (Gamemode == null)
            {
                Destroy(gameObject.transform.parent.gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Gamemode != null && Gamemode.Objectives.Any(x => this))
                Gamemode.Objectives.Remove(this);
        }

        private float hintTimer = 0f;
        private GameObject hint;
        private void Update()
        {
            this.hintTimer += Time.deltaTime;
            if (this.hint != null)
            {
                if (this.hintTimer < 5f)
                {
                    this.hint.transform.position = Hero.transform.position + Vector3.up * 0.5f;
                    Vector3 vector = NextObjective.transform.position - this.hint.transform.position;
                    float num = Mathf.Atan2(-vector.z, vector.x) * Mathf.Rad2Deg;
                    this.hint.transform.rotation = Quaternion.Euler(-90f, num + 180f, 0f);
                }
                else if (this.hint != null)
                {
                    UnityEngine.Object.Destroy(this.hint);
                }
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer != 8 || State != ObjectiveState.Current) return;
            var hero = collider.gameObject.GetComponent<Hero>();
            if (!hero.photonView.isMine) return;
            State = ObjectiveState.Taken;
            Hero = hero;
	        if (audioSource) audioSource.Play();          

            FengGameManagerMKII.instance.racingSpawnPoint = gameObject.transform.parent.position;
            hero.FillGas();

            if (NextObjective != null)
            {
                NextObjective.State = ObjectiveState.Current;
                hintTimer = 0f;
                this.hint = (GameObject) Instantiate(Resources.Load("Gamemode/RacingObjectiveHint"));
                this.hint.transform.parent = Hero.transform;
                this.hint.transform.position = Hero.transform.position + Vector3.up * 0.5f;
                Vector3 vector = base.transform.position - this.hint.transform.position;
                float num = Mathf.Atan2(-vector.z, vector.x) * Mathf.Rad2Deg;
                this.hint.transform.rotation = Quaternion.Euler(-90f, num + 180f, 0f);
            }
            else
            {
                Gamemode.OnRacingFinished();
            }
        }
    }
}

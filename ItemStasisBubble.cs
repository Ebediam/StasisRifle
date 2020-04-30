using BS;
using UnityEngine;


namespace StasisRifle
{

    public class ItemStasisBubble : MonoBehaviour
    {
        protected Item item;
        public ParticleSystem particleVFX;
        public GameObject forceFieldVFX;
        public Rigidbody rb;

        public ItemStasisRifle stasisRifle;

        public AudioSource stasisStartSFX;
        public AudioSource stasisLoopSFX;

        public bool hasCollided = false;
        public float duration = 5f;

        public float defaultLocomotionSpeed = 9999f;

        protected void Awake()
        {
            item = this.GetComponent<Item>();
            rb = item.GetComponent<Rigidbody>();

            particleVFX = item.transform.Find("ParticleVFX").GetComponent<ParticleSystem>();
            forceFieldVFX = item.transform.Find("ForceFieldVFX").gameObject;

            item.OnCollisionEvent += OnBubbleCollisionEvent;

            stasisStartSFX = transform.Find("StasisStartSFX").GetComponent<AudioSource>();
            stasisLoopSFX = transform.Find("StasisLoopSFX").GetComponent<AudioSource>();

        }

        
       public void PlayLoopSFX()
        {
            stasisLoopSFX.Play();
        }
        

        void OnBubbleCollisionEvent(ref CollisionStruct collisionInstance)
        {
            if (hasCollided)
            {
                return;
            }

            stasisStartSFX.Play();
            Invoke("PlayLoopSFX", stasisStartSFX.clip.length);

            hasCollided = true;
            particleVFX.Stop();
            forceFieldVFX.SetActive(true);
            rb.isKinematic = true;

            FreezeEnemies();
            Invoke("UnfreezeEnemies", duration);

        }


        void FreezeEnemies()
        {
            foreach(Creature creature in Creature.list)
            {
                if(creature == Creature.player)
                {
                    continue;
                }

                if(Vector3.Distance(creature.transform.position, transform.position) < 2f)
                {
                    creature.animator.speed = 0f;
                    creature.navigation.StopNavigation();
                    creature.navigation.enabled = false;
                    
                }

            }
        }

        public void UnfreezeEnemies()
        {
            foreach(Creature creature in Creature.list)
            {
                if(creature == Creature.player)
                {
                    continue;
                }

                creature.animator.speed = 1f;
                creature.navigation.enabled = true;
                
            }



            item.Despawn();
        }

        void FixedUpdate()
        {
            

        }

       

    }
}
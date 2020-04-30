using BS;
using UnityEngine;


namespace StasisRifle
{

    public class ItemStasisRifle : MonoBehaviour
    {
       protected Item item;
        public ItemModuleStasisRifle module;
        
        public ItemShooter.ItemShooter shootController;
        public Animator animator;

        public ParticleSystem lightBallVFX;
        public ParticleSystem chargeSparksVFX;
        public AudioSource chargeSFX;
        public ItemStasisBubble stasisBubble;

        public float timer = 0f;

        public bool triggerHeld;

        protected void Awake()
        {
            item = this.GetComponent<Item>();
            module = item.data.GetModule<ItemModuleStasisRifle>();


            item.OnHeldActionEvent += OnHeldAction;

            animator = gameObject.GetComponent<Animator>();

            if (!animator)
            {
                Debug.LogError("No animator found");
            }

            if (item.GetComponent<ItemShooter.ItemShooter>())
            {
                shootController = item.GetComponent<ItemShooter.ItemShooter>();
                shootController.isShootingAllowed = false;
            }
            else
            {
                Debug.Log("ItemShooter not found");
            }

            lightBallVFX = transform.Find("LightBallFX").GetComponent<ParticleSystem>();
            chargeSparksVFX = transform.Find("SparksFX").GetComponent<ParticleSystem>();
            chargeSFX = transform.Find("ChargeSFX").GetComponent<AudioSource>();

        }

        
        public void OnHeldAction(Interactor interactor, Handle handle, Interactable.Action action)
        {
            if(handle != item.mainHandleRight)
            {
                return;
            }


            if (action == Interactable.Action.UseStart)
            {

                StartFX();
                triggerHeld = true;
                
            }

            if(action == Interactable.Action.UseStop)
            {
                if(timer > module.minCharge)
                {
                    if (triggerHeld)
                    {
                        float charge = (timer / module.fullCharge) * (module.maxDuration - module.minDuration) + module.minDuration;

                        ShootBubble(charge);
                    }
                }
                else
                {
                    timer = 0f;
                    StopFX();
                }

                


                triggerHeld = false;




            }

        }


        public void StartFX()
        {
            lightBallVFX.Play();
            chargeSparksVFX.Play();
            chargeSFX.Play();
            animator.SetBool("isCharging", true);
        }

        public void StopFX()
        {
            lightBallVFX.Stop();
            chargeSparksVFX.Stop();
            chargeSFX.Stop();
            animator.SetBool("isCharging", false);
        }


        void Update()
        {
            if (triggerHeld)
            {
                timer += Time.deltaTime;

                if(timer > module.fullCharge)
                {
                    
                    ShootBubble(module.maxDuration);

                }
            }

        }

      
        public void ShootBubble(float duration)
        {
            timer = 0f;
            Item bullet = shootController.Shoot();
            if (stasisBubble)
            {
                stasisBubble.UnfreezeEnemies();
                stasisBubble = null;
            }

            stasisBubble = bullet.GetComponent<ItemStasisBubble>();

            stasisBubble.stasisRifle = this;

            stasisBubble.duration = duration;
            StopFX();
            triggerHeld = false;
        }


        

    }
}
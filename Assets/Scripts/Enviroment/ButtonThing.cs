using UnityEngine;

    public class ButtonThing : InteractableThing
    {
        public SpriteRenderer sr;
        public Sprite activatedSprite;
        public Sprite deactivatedSprite;
        public GameObject[] boxes;
        public Laser[] lasers;
        public Door[] doors;
        
        public AudioSource audioSource;
        public AudioClip activateSound;
        public AudioClip deactivateSound;
        
        private void Start()
        {
            sr.sprite = isActivated ? activatedSprite : deactivatedSprite;
            
        }
        
        public override void Interact()
        {
            if (!isInteractable) return;
            if (isSwitch) isActivated = !isActivated;
            else isActivated = true;
            
            audioSource.clip = isActivated ? activateSound : deactivateSound;
            audioSource.Play();
            
            sr.sprite = isActivated ? activatedSprite : deactivatedSprite;
            
            foreach (var switchBox in boxes)
            {
                switchBox.SetActive(!switchBox.activeSelf);
            }

            foreach (var laser in lasers)
            {
                laser.SetActive(!laser.isActivated);
            }
            
            foreach (var door in doors)
            {
                door.SetActive(door.isLocked);
            }
            
        }
        
    }

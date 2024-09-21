namespace Quantum
{
    using Photon.Deterministic;
    using Spine.Unity;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Animations;

    public enum AnimationName
    {
        Idle, Walk, Attack, Dead, Buff
    }

    public unsafe  class PlayerView : QuantumEntityViewComponent
    {
        public const string AMIN_IDLE = "Idle";
        public const string AMIN_WALK = "Walk";
        public const string AMIN_ATTACK = "Attack";
        public const string AMIN_DEAD = "Dead";
        public const string AMIN_BUFF = "Buff";
        public const string LV_UP = "LvUp";
        public Quaternion rotationLeft;
        public Quaternion rotationRight;

        private Animator animator;
        private PhysicsBody2D body;
        private AnimationName currentAnim;
        private PlayerInfo playerInfo;
        private HealthIfno healthIfno;
        private BulletInfo bulletInfo;
        private HealthBar healthBar;
        private TextMeshPro txtName;
        private TextMeshProUGUI txtQuantity;
        private TextMeshProUGUI txtDamagePlayer;
        private TextMeshProUGUI txtHealthPlayer;
        private TextMeshProUGUI txtLV;
        public bool spawnBullet = false;
        public bool TestHP = false;
        public AudioSource source;


        private void Awake()
        {
            source = GetComponent<AudioSource>();
            animator = GetComponentInChildren<Animator>();
            healthBar = GetComponentInChildren<HealthBar>();
            txtName = GetComponentInChildren<TextMeshPro>();
            txtName.text = "";
            

            QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
        }


        private void Update()
        {
            currentAnim = GetCurrentAnimaton();
            body = VerifiedFrame.Get<PhysicsBody2D>(_entityView.EntityRef);
            if(body.Velocity != FPVector2.Zero && currentAnim != AnimationName.Attack && currentAnim != AnimationName.Dead)
            {
                animator.Play(AMIN_WALK);
            }

            playerInfo = VerifiedFrame.Get<PlayerInfo>(_entityView.EntityRef);
            // sl bình máu


            if (txtName.text == "")
            {
                var playerData = VerifiedFrame.GetPlayerData(playerInfo.PlayerRef);
                txtName.text = playerData.PlayerNickname;
            }

            if(playerInfo.CurrentHealth <=0 )
            {
                animator.Play(AMIN_DEAD);
                healthBar.gameObject.SetActive(false);
                return;
            }
            var input = VerifiedFrame.GetPlayerInput(playerInfo.PlayerRef);
            if (input->Attack.WasPressed)
            {
                animator.Play(AMIN_ATTACK);
                source.Play();
            }
            if (input->Eat.WasPressed)
            {

                //Debug.Log(playerInfo.QuantityHP+"số lượng bình máu")"
                    animator.Play(AMIN_BUFF);
                
            }


            if (body.Velocity.X > 0)
            {
                animator.transform.rotation = rotationRight;
            }
            if (body.Velocity.X < 0)
            {
                animator.transform.rotation = rotationLeft;
            }

            // mau
            if(playerInfo.Health == 0)
            {
                healthBar.SetValue(0);
            }
            else
            {
                healthBar.SetValue((playerInfo.CurrentHealth/playerInfo.Health).AsFloat);
            }

            // camera
            //if (QuantumRunner.DefaultGame.PlayerIsLocal(playerInfo.PlayerRef) == false)
            //{
            //    Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            //}
            if (QuantumRunner.DefaultGame.PlayerIsLocal(playerInfo.PlayerRef) == true)
            {
                Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);

                GameObject player = GameObject.Find("TXTCanvas");

                if (player != null)
                {
                    // Lấy component Text từ đối tượng
                    Transform quantity = player.transform.Find("txtQuantity");

                    if (quantity != null)
                    {
                        txtQuantity = quantity.GetComponent<TextMeshProUGUI>();
                        if (txtQuantity != null)
                        {
                            txtQuantity.text = playerInfo.QuantityHP.ToString();
                        }
                    }
                    Transform damage = player.transform.Find("txtDamage");
                    if (damage != null)
                    {
                        txtDamagePlayer = damage.GetComponent<TextMeshProUGUI>();
                        if (txtDamagePlayer != null)
                        {
                            txtDamagePlayer.text = playerInfo.Damage.ToString();
                        }
                    }
                    Transform health = player.transform.Find("txtHealth");

                    if (health != null)
                    {
                        txtHealthPlayer = health.GetComponent<TextMeshProUGUI>();
                        if (txtHealthPlayer != null)
                        {
                            txtHealthPlayer.text = playerInfo.CurrentHealth+"/"+ playerInfo.Health.ToString();
                        }
                    }
                    Transform lv = player.transform.Find("txtLV");

                    if (lv != null)
                    {
                        txtLV = lv.GetComponent<TextMeshProUGUI>();
                        if (txtLV != null)
                        {
                            txtLV.text = "LV: " + playerInfo.LV.ToString();
                        }
                    }
                }
                
            }
        }



        private AnimationName GetCurrentAnimaton()
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(AMIN_ATTACK)) return AnimationName.Attack;
            else if(stateInfo.IsName(AMIN_WALK)) return AnimationName.Walk;
            else if (stateInfo.IsName(AMIN_DEAD)) return AnimationName.Dead;
            else if (stateInfo.IsName(AMIN_BUFF)) return AnimationName.Buff;
            return AnimationName.Idle;
        }
        public void PollInput(CallbackPollInput callback)
        {
            if (QuantumRunner.DefaultGame.PlayerIsLocal(playerInfo.PlayerRef) == false)
            {
                return;
            }
            Quantum.Input input = new Quantum.Input();
            FP x, y;
            if(currentAnim != AnimationName.Dead)
            {
                GetPlayerInputDirection(out x, out y);
                input.Direction = new FPVector2(x, y);
            }

            input.Attack = UnityEngine.Input.GetKey(KeyCode.Mouse0);
            input.SpawnBullet = spawnBullet;
            if(spawnBullet == true) spawnBullet = false;
            //
            if ((playerInfo.QuantityHP>0) && (playerInfo.QuantityHP != playerInfo.Health))
            {
                input.Eat = UnityEngine.Input.GetKeyUp(KeyCode.Space);
            }
            

            callback.SetInput(input, DeterministicInputFlags.Repeatable);
        }

        private void GetPlayerInputDirection(out FP x, out FP y)
        {
            x = 0;
            y = 0;
            // nếu đang tấn công thì đứng yên
            //if (currentAnim == AnimationName.Attack) return;
            if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                x = 1;
            }
            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                x = -1;
            }
            if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
            {
                y = 1;
            }
            if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
            {
                y = -1;
            }
            // dieu khien bang wasd
            if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                x = 1;
            }
            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                x = -1;
            }
            if (UnityEngine.Input.GetKey(KeyCode.W))
            {
                y = 1;
            }
            if (UnityEngine.Input.GetKey(KeyCode.S))
            {
                y = -1;
            }
            
        }

    }
}

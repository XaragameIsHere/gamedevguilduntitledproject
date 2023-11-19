using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Slider = UnityEngine.UI.Slider;

public class playerMovement : MonoBehaviour
{
	[SerializeField] Vector2 velocity = new Vector2(1, 0);
	[SerializeField] LayerMask groundLayers;
	[SerializeField] LayerMask wallLayers;
    [SerializeField] LayerMask enemyLayers;
	[SerializeField] float friction = 2;
	[SerializeField] float walkSpeed = 5;
	//[SerializeField] enemyScripting enemyScript;
	//[SerializeField] Sprite badheart;
	//[SerializeField] List<Image> hearts = new List<Image>();
	//[SerializeField] List<Image> badHearts = new List<Image>();
	[SerializeField] GameObject trigger;
	//[SerializeField] ParticleSystem parryParticles;
	[SerializeField] Canvas deathScreen;
	//[SerializeField] GameObject parryBack;

	//public CircleCollider2D parryCollider;
	public int playerLives = 4;
    public Transform tweenPos;
    public Camera playerCamera;
	public bool invincibility = false;
	public Vector2 velocityUp;
	public Vector2 playerSpeed;
	//private playerUIController uIController;
	//private ParticleSystem.EmissionModule parryEmitter;
	private bool isGrounded;
	private ParticleSystem rocketBoots;
	public bool inFight = false;
	public bool inDialogue = false;
	public bool alive = true;
	public bool parrying = false;
    public Rigidbody2D rBody;
    Collider2D collisionBaybe;
	Collider2D[] hits;
	//ParticleSystem.MainModule psMain;
	//ParticleSystem.ShapeModule shape;
	//Animator playerAnimator;
	SpriteRenderer playerSprite;
	private bool objectAttained = false;
	private float startDistance;
	private playerMovement theScript;
	//private Dictionary<bool, string> dialogueType = new Dictionary<bool, string>();
    //[ColorUsage(true, true)]
    //public Color someHDRColor;


    // Start is called before the first frame update
    void Start()
	{
		//dialogueType.Add(true, "SelectionStartOBJECT1");
        //dialogueType.Add(false, "SelectionStartNOOBJECT1");
        //startDistance = Vector3.Distance(transform.position, trigger.transform.position);
		playerSprite = GetComponent<SpriteRenderer>();
		//playerAnimator = GetComponent<Animator>();
		//rocketBoots = GetComponent<ParticleSystem>();
		//psMain = rocketBoots.main;
		//shape = rocketBoots.shape;
		collisionBaybe = GetComponent<PolygonCollider2D>();
		//uIController = GetComponent<playerUIController>();
		theScript = this;

	}

	IEnumerator waitforit()
	{
		yield return new WaitForSeconds(3);
		invincibility = false;
        collisionBaybe.excludeLayers = 0;
    }


    
    void hurt()
	{
		
		if ( alive && !invincibility)
		{
			invincibility = true;
			//collisionBaybe.excludeLayers = enemyLayers;
			StartCoroutine(waitforit());
			//badHearts.Add(hearts[0]);
			//hearts.RemoveAt(0);
		}
		
	}

	

   

    

    IEnumerator unParry()
    {
        yield return new WaitForSeconds(.5f);
        parrying = false;
        //parryCollider.enabled = false;
    }


	// Update is called once per frame
	bool killed = false;
	bool playerCanRestart = false;
    IEnumerator waitToStart() { yield return new WaitForSeconds(2); deathScreen.transform.GetChild(1).gameObject.SetActive(true); playerCanRestart = true;}
	public void killPlayer()
	{
		alive = false;
		deathScreen.gameObject.SetActive(true);
		deathScreen.transform.GetChild(2).DOLocalMoveY(190, .75f);
		deathScreen.transform.GetChild(0).GetComponent<Image>().DOColor(new Color(0, 0, 0, 255), 1);

        StartCoroutine(waitToStart());
        
    }

    void FixedUpdate()
	{
		isGrounded = rBody.IsTouchingLayers(groundLayers); //checks if the player is on a platform
		//calculates vertical velocity

		/*
		if (alive)
		{
			foreach (Image heart in badHearts)
			{
				heart.sprite = badheart;
			}
		}
		
		if (hearts.Count <= 0) 
		{
			alive = false;
		}*/

		//character controls
		if (inFight)
		{
			rBody.gravityScale = 0;
		}
		else
		{
			rBody.gravityScale = 50f;
		}

		if (Input.GetAxis("walk") > .1)
			playerSprite.flipX = false;
		else if (Input.GetAxis("walk") < -.1)
			playerSprite.flipX = true;
		

		//playerAnimator.SetBool("Flying", !isGrounded);

		if (Input.GetButton("Jump") && alive && !inDialogue && inFight)
			rBody.velocity = new Vector2(rBody.velocity.x, walkSpeed * Input.GetAxis("Jump"));
		else if (!Input.GetButton("Jump") && Mathf.Abs(rBody.velocity.y) > 1 && inFight)

			rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Lerp(rBody.velocity.y, 0, friction * Time.deltaTime));
		else
			rBody.velocity = new Vector2(rBody.velocity.x, 0);

		//playerAnimator.SetFloat("playerSpeed", Mathf.Abs(Input.GetAxis("walk")));
        //playerAnimator.SetBool("walking", Input.GetButton("walk"));

        if (Input.GetButton("walk") && alive && !inDialogue)
			rBody.velocity = new Vector2(walkSpeed * Input.GetAxis("walk"), rBody.velocity.y);
		else if (!Input.GetButton("walk") && Mathf.Abs(rBody.velocity.x) > 1)
		
			rBody.velocity = new Vector2(Mathf.Lerp(rBody.velocity.x, 0, friction * Time.deltaTime), rBody.velocity.y);
		else
			rBody.velocity = new Vector2(0, rBody.velocity.y);

		//shape.rotation = new Vector3(90+(45 * Input.GetAxis("walk")), 90, 0);

		/*
		if (isGrounded)
			rocketBoots.Stop();
		else
			rocketBoots.Play();
		*/
        
       
		/*
        if (Input.GetButton("parry") && !parryCollider.enabled && inFight && !inDialogue)
        {
			playerAnimator.SetTrigger("parry");
			sfxSource.clip = parrySound;
			sfxSource.Play();
            
            parryParticles.Emit( 50);
			parrying = true;
            parryCollider.enabled = true;
			StartCoroutine(unParry());
        }

        if (!inFight && !inDialogue)
		{
			fuelMeter.value = Mathf.Abs(Vector3.Distance(transform.position, trigger.transform.position) - startDistance)/ startDistance;
		}
        else
		{
			fuelMeter.value = 1;
			playerCamera.transform.position = tweenPos.position;
		}*/

        if (collisionBaybe.IsTouchingLayers(wallLayers))
		{
			//walk = false;
			rBody.AddForce(new Vector2(0, 10000));
		}

		if (!alive && !killed)
        {
			killed = true;
            killPlayer();
            
        }
		
		if (Input.anyKeyDown && playerCanRestart)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}


		//actually executes moving the player forward

	
	
}

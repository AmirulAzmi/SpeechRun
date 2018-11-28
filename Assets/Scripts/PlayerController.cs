using UnityEngine;

public enum State
{
    Normal = 0,
    Magnet,
    Multiplier
}

public class PlayerController : MonoBehaviour {

	public static PlayerController Instance;
	public static float speed = 4f;
	public bool isPause = false;
    public bool canMove = true;
    bool gameover = false;
	public State state = State.Normal;


	Vector3 targetPos;

    static int life = 3;

	int laneNumber = 0;
	float jumpForce = 6.0f;
    float time;
	float gravity = 12.0f;
	float vertical;
    float starttimer = 0.0f;
	bool jumping = false;
	bool sliding = false;

	//Rigidbody rb;
	Animator anim;
	CharacterController controller;
	CapsuleCollider box;

	void Start()
	{
		Instance = this;
		anim = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
		//rb = GetComponent<Rigidbody> ();
	}
    void Update()
    {
        if (starttimer > 4)
        {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) { Jump(); }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) { MoveLeft(); }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) { MoveRight(); }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) { Slide(); }
            #endif
            if (Input.GetKeyDown(KeyCode.Escape)) { Pause(); }
        }
    }
    void FixedUpdate()
    {
        starttimer += Time.fixedDeltaTime;
        
        if (canMove) {
            Move();
        }
    }

    public void SpeechHyp(string text)
    {
        switch (text)
        {
            case "left":
                MoveLeft();
                break;

            case "right":
                MoveRight();
                break;

            case "jump":
                Jump();
                break;

            case "slide":
                Slide();
                break;

            case "pause":
                if (!gameover && !MenuManager.Instance.isbumped)
                    Pause();
                break;

            case "resume":
                if (isPause)
                    Resume();
                break;

            case "retry":
                if (gameover)
                    Retry();
                break;

            case "continue":
                if (isPause && MenuManager.Instance.isbumped)
                    MenuManager.Instance.ReloadScene();
                break;

            case "home":
                if (gameover || isPause)
                    Home();
                break;

            default:
                break;
        }
    }

	void Move() {
		targetPos = transform.position.z * Vector3.forward;

        if (laneNumber == -1) {
            targetPos += Vector3.left;
        }
        else if (laneNumber == 1) {
            targetPos += Vector3.right;
        }
		Vector3 moveVector = Vector3.zero;
		moveVector.x = (targetPos - transform.position).normalized.x * 10;

		bool grounded = isGrounded ();
		anim.SetBool ("isGrounded", grounded);

		if(grounded) {
			vertical = 0.0f;

			if (jumping) {
				anim.SetTrigger ("isJumping");
				vertical = jumpForce;
				jumping = false;
			} 
			else if (sliding) {
				StartSliding ();
				Invoke ("StopSliding", 1.0f);
			}
		}
		else {
			vertical -= (gravity * Time.deltaTime);
		}

		moveVector.y = vertical;
		moveVector.z = speed;

		controller.Move(moveVector * Time.deltaTime);
	}
    
    public void Dead()
    {
        isPause = true;
        canMove = false;
        anim.speed = 0;
        life -= 1;
        if (life < 1)
        {
            PlayerPrefs.SetInt("coins", 0);
            MenuManager.Instance.DisplayGameOver();
            ScoreManager.Instance.SaveScore();
            GameOver();
        }
        else
        {
            speed = 4;
            PlayerPrefs.SetInt("coins", ScoreManager.Instance.coins);
            MenuManager.Instance.Continue();
        }
    }
    void GameOver() {
        gameover = true;
        life = 3; //life is static, reset life back if player choose 'retry'
    }
    // -- Character Movements ---
	void MoveLeft()
	{
		if (!canMove)
			return;

		if (laneNumber > -1) {
			laneNumber -= 1;
			//transform.position = new Vector3 (pointX, transform.position.y, transform.position.z);
		}
	}

	void MoveRight()
	{
		if (!canMove)
			return;

		if (laneNumber < 1) {
			laneNumber += 1;
			//transform.position = new Vector3 (pointX, transform.position.y, transform.position.z);
		}
	}

	void Jump()
	{
		if (isGrounded ())
			jumping = true;
	}

	void Slide()
	{
		sliding = true;
	}

    // -- End Character Movements ---
	
    // -- Button's Action -----------
    public void Pause()
	{
		isPause = true;
		canMove = false;
		anim.speed = 0;
		MenuManager.Instance.Pause ();
	}

    public void Resume()
    {
        isPause = false;
        canMove = true;
        anim.speed = 1;
        MenuManager.Instance.Resume();
    }

    //public void Quit()
    //{
    //    ScoreManager.Instance.ResetScore();
    //    #if UNITY_EDITOR
    //        UnityEditor.EditorApplication.isPlaying = false;
    //    #else
    //        Application.Quit();
    //    #endif
    //}
    public void Home()
    {
        MenuManager.Instance.Home();
    }

    public void Retry() {
        MenuManager.Instance.Retry();
    }

    // -- End Button's Action ---------

    // -- Animation's Use ----------
	void StartSliding()
	{
		anim.SetBool ("isSliding", sliding);
		controller.radius = 0.2f;
		controller.height = 0.2f;
		controller.center = new Vector3 (0, 0.15f, 0);
	}

	void StopSliding()
	{
		controller.radius = 0.5f;
		controller.height = 2.1f;
		controller.center = new Vector3 (0, 1, 0);
		sliding = false;
		anim.SetBool ("isSliding", sliding);
	}

	bool isGrounded()
	{
		Ray groundRay = new Ray (new Vector3(controller.bounds.center.x, 
			(controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,
			controller.bounds.center.z), Vector3.down);
		Debug.DrawRay (groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

		return Physics.Raycast (groundRay, 0.3f );
	}

    // -- End Animation's Use ----------
}

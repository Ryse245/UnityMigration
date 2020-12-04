using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    const float MAX_X_POS = 31.0f;
    const float MIN_X_POS = 0.0f;
    const float Y_POS = 9.0f;
    const string SCORESTRING = "Score: ";

    public GameObject target;
    public int score = 0;
    public Text ScoreText;
    public List<Particle2D> particleArray;

    bool isScoring = false;

    private void Awake()
    {
        if(!instance)
            instance = this;
    }

    private void Start()
    {
        if(target)
        {
            BuoyancyForceGenerator buoyancy = new BuoyancyForceGenerator(target, target.transform.localScale.y / 2.0f, 1.0f, WaterObject.waterTopPos.y, 15.0f);
            ForceManager.instance.AddForceGen(buoyancy);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        // Check for colliding particles
        for (int i = 0; i < particleArray.Count; i++)
        {
            /*
            for(int j = i + 1; j < particleArray.Count; j++)
            {
                if (CollisionDetector.CheckForCollision(particleArray[i], particleArray[j])&&(particleArray[i].getParticleInstantiated()&&particleArray[j].getParticleInstantiated()))
                {
                    // destroy both particles if colliding
                    Destroy(particleArray[i].gameObject);
                    Destroy(particleArray[j].gameObject);
                }
            }
            */
        }
        /*
        if(target)
        {
            if (CheckForHit())
            {
                ChangePos();
            }
            if (isScoring)   //Was here to mitigate errors, but I tihnk the project just didn't compile correctly since it's working fine now
            {
                ScoreText.text = SCORESTRING + score.ToString();
            }
        }
        */
    }
    /*
    bool CheckForHit()
    {
        foreach (Particle2D particle in particleArray)
        {
            if(particle != target.GetComponent<Particle2D>())
            {
                float distance = Vector3.Distance(target.transform.position, particle.transform.position);
                if (distance <= 1.0f)
                {
                    Destroy(particle.gameObject);
                    return true;
                }
            }
        }
        return false;
    }

    void ChangePos()
    {
        Vector3 randomPos = new Vector3(Random.Range(MIN_X_POS, MAX_X_POS), Y_POS, 0.0f);

        target.transform.position = randomPos;
        target.GetComponent<Particle2D>().setVelocity(Vector3.zero);

        score++;
    }
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingProjectile : baseProjectile
{
    #region vars
    // cache vars
    GameObject playerObject;

    // homing vars
    [SerializeField]
    float homingSpeed;
    [SerializeField]
    float homingTime;
    [SerializeField]
    float fullyHomeThreshold;

    float homingTimeLeft;
    bool hasFullyHomedIn = false;
    #endregion
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        homingTimeLeft = homingTime;
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // highers the angle that the projectile follows at
    private void homeUp()
    {
        setAngle(getAngle() + homingSpeed * Time.deltaTime);
        if (getAngle() > 360)
        {
            setAngle(0);
        }
    }

    // lowers the angle that the projectile follows at
    private void homeDown()
    {
        setAngle(getAngle() - homingSpeed * Time.deltaTime);
        if (getAngle() < 0)
        {
            setAngle(360);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (homingTimeLeft >= 0 && !hasFullyHomedIn)
        {
            homingTimeLeft -= Time.deltaTime;
            Vector2 diff = ((Vector2)playerObject.transform.position) - ((Vector2)gameObject.transform.position);
            float angleTowardsPlayer = Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) + 180) % 360;

            // checks if angle is already pointing towards player
            if (Mathf.Abs(Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) + 360) % 360 - getAngle()) % 360 < fullyHomeThreshold)
            {
                hasFullyHomedIn = true;
            }

            // checks whether homing up or homing down would be optimal to angle towards the player
            if(getAngle() > angleTowardsPlayer)
            {
                if (360 - getAngle() + angleTowardsPlayer > getAngle() - angleTowardsPlayer)
                {
                    homeUp();
                }
                else
                {
                    homeDown();
                }
            }
            else {
                if (360 - angleTowardsPlayer + getAngle() > angleTowardsPlayer - getAngle())
                {
                    homeDown();
                }
                else
                {
                    homeUp();
                }
            }
        }
    }
}

using UnityEngine;

// This script is attached to the `DaughterNPC` object in the Hierarchy.
// Kusi-Rose is the daughter of the family patriarch.
public class DaughterNPC : FriendlyNPC
{
    protected override void Start()
    {
        base.Start();

        var playerObject = GameObject.FindGameObjectWithTag("Player");
        Profile.Relationships.Add(playerObject, new RelationshipTraits
        {
            TrustToward = 0.95f,    // high trust in father
            Love = 0.92f,           // strong familial love
            FearOf = 0.15f          // less fear than son
        });

        stateMachine.SetState(new NPCIdleState(this));
    }

    private void onGunFired()
    {
        this.Profile.MentalState.Comfort -= (this.Profile.MentalState.Comfort * 0.12f);
        this.Profile.MentalState.Comfort = Mathf.Clamp(this.Profile.MentalState.Comfort, -1f, 1f);
    }
}

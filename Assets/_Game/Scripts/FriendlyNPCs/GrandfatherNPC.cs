using UnityEngine;

// This script is attached to the `GrandfatherNPC` object in the Hierarchy.
// Tupac is the grandfather of the family.
public class GrandfatherNPC : FriendlyNPC
{
    protected override void Start()
    {
        base.Start();

        // Grandfather moves slower due to age
        moveSpeed = 2.0f;

        var playerObject = GameObject.FindGameObjectWithTag("Player");
        Profile.Relationships.Add(playerObject, new RelationshipTraits
        {
            TrustToward = 0.85f,    // trusts but more independent
            Love = 0.90f,           // strong familial love
            FearOf = 0.10f          // less fearful due to life experience
        });

        stateMachine.SetState(new NPCIdleState(this));
    }

    private void onGunFired()
    {
        // Grandfather is more stoic, less affected by gunfire
        this.Profile.MentalState.Comfort -= (this.Profile.MentalState.Comfort * 0.08f);
        this.Profile.MentalState.Comfort = Mathf.Clamp(this.Profile.MentalState.Comfort, -1f, 1f);
    }
}

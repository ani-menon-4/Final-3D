using System.Collections.Generic;
using UnityEngine;

public class Katamari : MonoBehaviour
{
    [SerializeField] private AudioSource _equipAudioSource;
    [SerializeField] private AudioSource _bonkAudioSource;
    [SerializeField] private AudioSource _unequipAudioSource;
    [SerializeField] private float _radiusGrowAmount = 1.0f;
    [SerializeField] private Collider _katamariCollider;
    [SerializeField] private Transform _sphereTransform;

    // Add a member variable to represent a LIST of all of the Collectable 
    //      objects we've collected named _katamariCollection.
    // Then, uncomment the code in SetSpherePosition().
    private List<Collectable> _katamariCollection;

    // ------------------------------------------------------------------------
    // There's probably a member variable you need to declare in a built-in
    //      Unity method that runs once, before Update starts...
    
    public void Start()
    {
        _katamariCollection = new List<Collectable>();
    }

    // ------------------------------------------------------------------------
    /*
    POTENTIAL CHANGE ADDED THAT MIGHT DEVIATE FROM RUBRIC:
    I would like to try and implement the ability for the katamari to pick up anything so long as it is bigger than it. Therefore, I have added the logic handling obstacles into this method at lines 63-82 in order to take advantage of this method's access to comparing sizes between Collectables and the Katamari. I don't think this drastically changes any logic of the game since the collision parameter in OnCollisionEnter wasn't doing anything too crucial besides serving as a way to reference the tag of the GameObject (at least to my understanding).
    */
    private void OnTriggerEnter(Collider otherCollider)
    {
        // Find the Collectable Component on the object we collided with.
        // Modify this if statement to ALSO check and make sure the Collectable
        //      actually exists, since we might collide with other kinds of objects too.
        Collectable itemCollectable = otherCollider.gameObject.GetComponent<Collectable>();
        if (itemCollectable != null)
        {
            if (otherCollider.bounds.extents.magnitude <= _katamariCollider.bounds.extents.magnitude) {
                // Grow the SPHERE OBJECT, which should be a CHILD of the Katamari object,
                //      by _radiusGrowAmount.
                // AFTER growing the sphere, call SetSpherePosition() to update the sphere's position.
                _sphereTransform.localScale += (Vector3.one * _radiusGrowAmount);
                SetSpherePosition();

                // Add the new Collectable to our list of hit objects.
                _katamariCollection.Add(itemCollectable);

                // Tell the Collectable that we want to collect it.
                itemCollectable.Collect(transform);

                // If the object has an NPC Component, let it know about being collected
                //      by calling PickUp on the NPC.
                NPC itemNPC = otherCollider.gameObject.GetComponent<NPC>();
                if (itemNPC != null)
                {
                    itemNPC.PickUp();
                }

                _equipAudioSource.Play();
            }
            else if (otherCollider.gameObject.tag.Equals("Obstacle") && _katamariCollection.Count != 0)
            {
                // SHRINK the katamari SPHERE by _radiusGrowAmount.
                // AFTER changing its size, call SetSpherePosition() to fix its position.
                _sphereTransform.localScale -= (Vector3.one * _radiusGrowAmount);
                SetSpherePosition();

                // Grab a RANDOM member of our katamari collection.
                // Generate a position to drop it at- preferably by launching it far above the katamari sphere.
                // Then, DROP that Collectable.
                int kCollectionInd = Random.Range(0, _katamariCollection.Count - 1);
                _katamariCollection[kCollectionInd].Drop(
                    transform,
                    transform.position + new Vector3(0, _sphereTransform.localScale.y + 1.0f, 0)
                );

                // Finally, remove the entry from the _katamariCollection.
                _katamariCollection.RemoveAt(kCollectionInd);
                _unequipAudioSource.Play();
            }
        }
    }

    // ------------------------------------------------------------------------
    /*
    POTENTIAL CHANGE ADDED THAT MIGHT DEVIATE FROM RUBRIC:
    I would like to try and implement the ability for the katamari to pick up anything so long as it is bigger than it. Therefore, this method has been commented out and code moved to OnTriggerEnter at lines 63 - 82.
    */
    // private void OnCollisionEnter (Collision collision)
    // {
    //     // Modify this if statement to ALSO check if we have any entries in our
    //     //  katamari collection.
        
    //     if(collision.gameObject.tag.Equals("Obstacle") && _katamariCollection.Count != 0)
    //     {
    //         // SHRINK the katamari SPHERE by _radiusGrowAmount.
    //         // AFTER changing its size, call SetSpherePosition() to fix its position.
    //         _sphereTransform.localScale -= (Vector3.one * _radiusGrowAmount);
    //         SetSpherePosition();

    //         // Grab a RANDOM member of our katamari collection.
    //         // Generate a position to drop it at- preferably by launching it far above the katamari sphere.
    //         // Then, DROP that Collectable.
    //         int kCollectionInd = Random.Range(0, _katamariCollection.Count - 1);
    //         _katamariCollection[kCollectionInd].Drop(
    //             transform,
    //             transform.position + new Vector3(0, transform.lossyScale.y + 5.0f, 0)
    //         );

    //         // Finally, remove the entry from the _katamariCollection.
    //         _katamariCollection.RemoveAt(kCollectionInd);
    //         _unequipAudioSource.Play();
    //     }
    // }

    // ------------------------------------------------------------------------
    // Uncomment this method by removing the /* and */ AFTER you've added the
    //      _katamariCollection member variable.
    // You don't need to code anything else in this method :)
    // This method sets up the sphere to be in the corret position in front
    //      of the player, and ensures all of the objects inside the katamari
    //      remain in its center.
    private void SetSpherePosition ()
    {
        // place the sphere in front of the player
        _sphereTransform.position = transform.parent.position +
                transform.parent.up * _sphereTransform.lossyScale.x/2.0f +
                transform.parent.forward * (_sphereTransform.lossyScale.x/2.0f + 1.0f);

        // move all of the katamari objects to the new katamari center
        foreach (Collectable collectable in _katamariCollection)
        {
            if (collectable != null)
            {
                collectable.transform.position = _sphereTransform.position;   
            }
            else
            {
                Debug.LogError("null item in katamari collectables list");
            }
        }
    }
}

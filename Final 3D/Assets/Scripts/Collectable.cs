using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// Add an enum to represent an item's rarity.
public enum Rarity
{
    common, uncommon, rare
}

public class Collectable : MonoBehaviour
{
    // Add a member varible to represent each collectable's rarity. Make sure
    //      it can be tuned from the Inspector.
    [SerializeField] private Rarity _rarity;
    private Collider[] _colliders;
    private Rigidbody _rigidbody;
    private GameController _gameController;

    // ------------------------------------------------------------------------
    private void Start()
    {
        _colliders = GetComponents<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _gameController = FindFirstObjectByType<GameController>();
    }

    // ------------------------------------------------------------------------
    // You might want to add parameters to this method.
    public void Collect(Transform katamariTrans)
    {
        // Set the new parent and position of the Collectable.
        // Disable all of this object's colliders.
        transform.parent = katamariTrans;
        transform.position = katamariTrans.position;
        foreach (Collider collider in _colliders)
        {
            collider.enabled = false;
        }

        /*
        POTENTIAL CHANGE ADDED THAT MIGHT DEVIATE FROM THE RUBRIC:
        Due to the Katamari being able to pick up Collectable GameObjects that may have NavMeshObstacle Components attached to it (buildings for example), considerations need to be made to ensure that the NavMeshSurface will update appropriately when the GameObject is collected into the Katamari. Specifically, the NavMesh will already update when the object is collected thanks to the carve when stationary feature, but the carve may be reintroduced if the player holds the Katamari still, which is not ideal for NPC pathing. This could be solved by simply disabling the NavMeshObstacle Component when the Katamari is picked up, but if this Component is disabled before the carve is removed, that could lead to the NavMeshSurface not updating until the Collectable is ejected and acquires a stationary state, which is not ideal.
        
        Therefore, the following conditional will check the Collectable for a NavMeshObstacle, and if so, will then disable the NavMeshObstacle Component. Additionally, if the carve has not already been disabled, the carve will automatically be disabled in the conditional body before the Component is disabled.
        */
        NavMeshObstacle navObstacle = gameObject.GetComponent<NavMeshObstacle>();
        if (navObstacle != null)
        {
            navObstacle.carving = false;
            navObstacle.enabled = false;
        }

        // This line sets the rigidbody to kinematic so it'll stop using physics.
        // (You don't need to change this.)
        _rigidbody.isKinematic = true;

        // Tell the GameController that we collected this item.
        _gameController.CollectItem(_rarity);
    }

    // ------------------------------------------------------------------------
    // You might want to add parameters to this method.
    public void Drop(Transform katamariTrans, Vector3 dropPos)
    {
        // Reset this object's parent so it's no longer attached to the Katamari,
        //  and give it a new position ABOVE the Katamari sphere.
        transform.parent = null;
        transform.position = dropPos;

        // Enable all of this object's colliders.
        foreach (Collider collider in _colliders)
        {
            collider.enabled = true;
        }

        /*
        POTENTIAL CHANGE ADDED THAT MIGHT DEVIATE FROM THE RUBRIC:
        Due to the Katamari being able to pick up Collectable GameObjects that may have NavMeshObstacle Components attached to it (buildings for example), considerations need to be made to ensure that the NavMeshSurface will update appropriately when the GameObject is ejected from the Katamari. Specifically, the NavMeshObstacle Component needs to be re-enabled when dropped from the Katamari, so that when the object settles into a stationary state, it will be able to reapply a carve into the NavMeshSurface.
        
        Therefore, the following conditional will check the Collectable for a NavMeshObstacle, and if present, will re-enable the NavMeshObstacle Component.
        */
        NavMeshObstacle navObstacle = gameObject.GetComponent<NavMeshObstacle>();
        if (navObstacle != null)
        {
            navObstacle.enabled = true;
        }

        // This line sets the rigidbody to kinematic so it'll start using physics again.
        // (You don't need to change this.)
        _rigidbody.isKinematic = false;

        // Add a force to the rigidbody to make it fly upwards.
        _rigidbody.AddForce(
            new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(5.0f, 10.0f), Random.Range(-2.0f, 2.0f)), 
            ForceMode.Impulse
        );

        // Tell the UI that we removed this item.
        _gameController.RemoveItem(_rarity);
    }
}
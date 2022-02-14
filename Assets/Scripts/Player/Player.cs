using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Player : Entity
{
    [SerializeField] private GearCounter gearsCounter;
    [SerializeField] private PointCounter pointCounter;
    private int gearsCount = 0;
    
    [SerializeField] private float radiationLevel = 0f;
    [SerializeField] private float maxRadiationLevel = 20f;
    [SerializeField] private float timeForNullRadiation = 5;
    
    private Tilemap tilemap;
    private GameObject tilemapGameObject;
    
    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile openChestTile;
    private string gearTileName = GlobalFields.gearTileName;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag(GlobalFields.tilemapTag).Length > 0)
        {
            tilemapGameObject = GameObject.FindGameObjectsWithTag(GlobalFields.tilemapTag)[0];
            tilemap = tilemapGameObject.GetComponent<Tilemap>();
        }
        uiControl = GameObject.FindGameObjectsWithTag(GlobalFields.canvasTag)[0].GetComponent<UIControl>();
    }

    public override void Start()
    {
        base.Start();
        new Points();
        Points.Counter = pointCounter;
    }

    public void AddRadiation(float radLevel)
    {
        radiationLevel += radLevel;
        uiControl.radiationIndicatorWidth = radiationLevel / maxRadiationLevel;
        if (radiationLevel >= maxRadiationLevel)
        {
            Debug.Log(gameObject.name + " Dead by radiation");
            gameObject.SendMessage("Death by radiation", null, SendMessageOptions.DontRequireReceiver);
            Death();
        }
    }
    
    public IEnumerator NullRadiationAfterSomeSeconds()
    {
        yield return new WaitForSeconds(timeForNullRadiation);
        if (!isInRadiation)
        {
            this.radiationLevel = 0f;
            uiControl.radiationIndicatorWidth = 0f;
        }
    }
    
    protected override void AddHealth(float _delta)
    {
        if (_delta < 0f)
        {
            ReduceHealth(_delta);
            return;
        }
        base.AddHealth(_delta); 
        uiControl.healthIndicatorWidth = health / startHealth;
        SoundManager soundManager = SoundManager.getInstance();
        soundManager.PlaySound(soundManager.powerUpClip, 0.8f);
    }
    
    protected override void ReduceHealth(float _delta)
    {
        base.ReduceHealth(_delta);
        uiControl.healthIndicatorWidth = health / startHealth;
        SoundManager soundManager = SoundManager.getInstance();
        soundManager.PlaySound(soundManager.damageClip, 0.3f);
    }

    protected override void Death()
    {
        uiControl.OpenGameOverMenu();
        Time.timeScale = 0f;
        SoundManager soundManager = SoundManager.getInstance();
        soundManager.PlaySound(soundManager.deathClip, 1f);
        Destroy(gameObject);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        if (tilemap != null && tilemapGameObject == collision.gameObject)
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                Vector3Int vector = tilemap.WorldToCell(hitPosition);
                TileBase tile = tilemap.GetTile(vector);

                if (tile != null && tile.name == gearTileName)
                {
                    SoundManager soundManager = SoundManager.getInstance();
                    soundManager.PlaySound(soundManager.gearPickUpClip, 0.7f);
                    tilemap.SetTile(vector, floorTile);
                    gearsCount++;
                    gearsCounter.SetCount(gearsCount);
                    Points.getCurrentInstance().pointCounter += Points.pointsForGear * gearsCount;
                    Points.Counter.SetCount();
                    //Debug.Log($"{Points.getCurrentInstance().pointCounter} points");
                }

                if (tile != null && tile.name == GlobalFields.chestTileName)
                {
                    tilemap.SetTile(vector, openChestTile);
                    GetBonus();
                }
            }
        }
    }
    
    private void GetBonus()
    {
        int bonusType = Random.Range(0, 4);
        Debug.Log($"Chest: {bonusType}");
        SoundManager soundManager = SoundManager.getInstance();
        switch (bonusType)
        {
            case 0:
                float health = Random.Range(-1, 3);
                AddHealth(health);
                break;
            case 1:
                gearsCount++;
                gearsCounter.SetCount(gearsCount);
                soundManager.PlaySound(soundManager.gearPickUpClip, 0.7f);
                break;
            case 2:
                Points.getCurrentInstance().pointCounter += Points.pointsFromChest;
                Points.Counter.SetCount();
                break;
            case 3:
                radiationLevel = 0;
                soundManager.PlaySound(soundManager.powerUpClip, 0.8f);
                break;
        }
    }
}

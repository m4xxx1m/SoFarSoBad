using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Создаём новый пункт меню для CustomTile
[CreateAssetMenu(fileName = "new CustomTile", menuName = "Tiles/CustomTile")]
public class CustomTile : Tile
{
    // Оставляем конструктор пустым - сегодня он нам не пригодится
    public CustomTile()
    {

    }
}
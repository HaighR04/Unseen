using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class FogController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerAbilities abilities;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap fogTilemap;
    [SerializeField] private TileBase fogTile;

    [Header("Settings")]
    [SerializeField] private int revealRadius = 3;
    [SerializeField] private int fogPadding = 2;


    private readonly HashSet<Vector3Int> visibleNow = new HashSet<Vector3Int>();
    private readonly HashSet<Vector3Int> newVisible = new HashSet<Vector3Int>();

    private Vector3Int lastCell;
    private bool hasLastCell;

    private void Start() {
        FillFog();
    }

    private void Update() {
        Vector3Int cell = fogTilemap.WorldToCell(player.position);
        if (hasLastCell && cell == lastCell) return; // still in the same cell

        lastCell = cell;
        hasLastCell = true;
        RefreshVisibility(cell);
    }


    private void FillFog() {
        floorTilemap.CompressBounds();
        BoundsInt b = floorTilemap.cellBounds;

        for (int x = b.xMin - fogPadding; x < b.xMax + fogPadding; x++) {
            for (int y = b.yMin - fogPadding; y < b.yMax + fogPadding; y++) {
                fogTilemap.SetTile(new Vector3Int(x, y, 0), fogTile);
            }
        }
    }


    private void RefreshVisibility(Vector3Int center) {
        newVisible.Clear();
        int r2 = revealRadius * revealRadius;
        for (int dx = -revealRadius; dx <= revealRadius; dx++) {
            for (int dy = -revealRadius; dy <= revealRadius; dy++) {
                if (dx * dx + dy * dy <= r2)
                    newVisible.Add(new Vector3Int(center.x + dx, center.y + dy, 0));
            }
        }

        // Cells that left the view will bring the fog back unless the character has eyes
        if (!abilities.HasEyes) {
            foreach (Vector3Int c in visibleNow) {
                if (!newVisible.Contains(c))
                    fogTilemap.SetTile(c, fogTile);
            }
        }

        foreach (Vector3Int c in newVisible) {
            fogTilemap.SetTile(c, null);
        }

        visibleNow.Clear();
        foreach (Vector3Int c in newVisible) visibleNow.Add(c);
    }
}

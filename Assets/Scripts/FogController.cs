using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

/* Covers the entire map with fog and only reveals a soft edge circle surrounding the player
Before eyes: fog fades back over the cells after the player leaves those tiles
After eyes: fog doesn't fade back, stays clear
*/

// This one was the hardest to figure out, lots of math and handling for the transitions
public class FogController : MonoBehaviour
{
    // These are all editable fields within Unity for various aspects, such as checking if the player has eyes, or modifying the settings of the fog
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerAbilities abilities;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap fogTilemap;
    [SerializeField] private TileBase fogTile;

    [Header("Visibility Settings")]
    [SerializeField] private int revealRadius = 3;
    [SerializeField] private int fogPadding = 2;
    [SerializeField] private float edgeSoftness = 1.5f;
    [SerializeField, Range(0f, 1f)] private float rimAlpha = 0.7f;

    // Alpha 1 = full fog, alpha 0 = full clear
    // FadeIn controls how fast the player's view clears the fog, FadeOut controls how fast the fog returns
    [Header("Fading, alpha per second")]
    [SerializeField] private float fadeInSpeed = 5f;
    [SerializeField] private float fadeOutSpeed = 0.8f;

    // visibleTarget keeps the alpha that each cell in the player's view is heading towards
    // alpha keeeps the current fog alpha for every cell which isn't fully fog
    // keys are a reusable copy of alpha's keys since a Dict can't be changed while looping
    private readonly Dictionary<Vector3Int, float> visibleTarget = new Dictionary<Vector3Int, float>();
    private readonly Dictionary<Vector3Int, float> alpha = new Dictionary<Vector3Int, float>();
    private readonly List<Vector3Int> keys = new List<Vector3Int>();
    private Vector3Int lastCell;
    private bool hasLastCell;

    private void Start() {
        FillFog();
    }

    // checks the tiles around the player on each update and calls the RefreshVisibility function
    private void Update() {
        Vector3Int cell = fogTilemap.WorldToCell(player.position);
        if (!hasLastCell || cell != lastCell) {
            lastCell = cell;
            hasLastCell = true;
            RefreshVisibility(cell);
        }; 
        Fade();
        // still in the same cell, doesn't recalculate the fog
    }

    // Fills the area outside of the fog padding with fog tiles, floor tiles have to exist in the entire map or the cells won't be covered in fog (still black anyways so it works lol)
    private void FillFog() {
        floorTilemap.CompressBounds();
        BoundsInt b = floorTilemap.cellBounds;

        for (int x = b.xMin - fogPadding; x < b.xMax + fogPadding; x++) {
            for (int y = b.yMin - fogPadding; y < b.yMax + fogPadding; y++) {
                Vector3Int pos = new Vector3Int(x, y, 0);
                fogTilemap.SetTile(pos, fogTile);
                fogTilemap.SetTileFlags(pos, TileFlags.None); // Lets each cell have its own color and alpha
            }
        }
    }

    // Figures out which cells are in the view and the alpha it should fade to
    private void RefreshVisibility(Vector3Int center) {
        visibleTarget.Clear();
        float r = revealRadius;
        for (int dx = -revealRadius; dx <= revealRadius; dx++) 
            { 
                for (int dy = -revealRadius; dy <= revealRadius; dy++) 
                { 
                    float d = Mathf.Sqrt(dx * dx + dy * dy);
                    if (d > r) continue;

                    float t = Mathf.InverseLerp(r - edgeSoftness, r, d); // Ranges from 0 in the middle to 1 at the rim
                    Vector3Int c = new Vector3Int(center.x + dx, center.y + dy, 0);
                    visibleTarget[c] = t * rimAlpha;
                    // New cells start as full fog and fade toward target
                    if (!alpha.ContainsKey(c)) alpha[c] = 1f;
            }
        }
    }

    // Moves each tracked cell alpha one step toward the target value
    private void Fade() {
        float dt = Time.deltaTime;
        keys.Clear();
        keys.AddRange(alpha.Keys);

        foreach (Vector3Int c in keys) {
            float a = alpha[c];
            float target;

            // With Eyes, a cell never gets darker than it already is
            if (visibleTarget.TryGetValue(c, out float vt))
            {
                target = (abilities.HasEyes && vt > a) ? a : vt;
            }
            else
            { // Out of view, fog returns before eyes, but with eyes fades fully clear and stays that way
                target = abilities.HasEyes ? 0f : 1f;
            }

            float speed = target < a ? fadeInSpeed : fadeOutSpeed;
            float next = Mathf.MoveTowards(a, target, speed * dt);

            if (next == a) continue; // no changes, skip the tilemap update
            fogTilemap.SetColor(c, new Color(0f, 0f, 0f, next));

            // Fully fogged cells are dropped from tracking to keep the Dict smaller
            if (next >= 1f) alpha.Remove(c);
            else alpha[c] = next;
        }
    }
}
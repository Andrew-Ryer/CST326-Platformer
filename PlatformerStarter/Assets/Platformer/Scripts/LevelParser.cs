using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * This script is responsible for reading a level layout from a text file and constructing the level
 * in a Unity scene by instantiating block GameObjects. The level file should be placed in the
 * Resources folder, and each line in the file represents a row of blocks.
 *
 * WHAT YOU NEED TO DO:
 * 1. In the for loop that iterates over each character (i.e. letter) in the current row, determine
 *    which type of block to create based on the letter (e.g., use 'R' for rock, 'B' for brick, etc.).
 *
 * 2. Instantiate the correct prefab (rockPrefab, brickPrefab, questionBoxPrefab, stonePrefab) corresponding
 *    to the letter.
 *
 * 3. Calculate the position for the new block GameObject using the current row and column index.
 *    - You will likely need to maintain a separate column counter as you iterate through the characters.
 *
 * 4. Set the instantiated block’s parent to 'environmentRoot' to keep the hierarchy organized.
 *
 * ADDITIONAL NOTES:
 * - The level reloads when the player presses the 'R' key, which clears all blocks under levelRoot
 *   and then re-parses the level file.
 * - Ensure that the level file's name (without the extension) matches the 'filename' variable.
 *
 * By completing these TODOs, you will enable the level parser to dynamically create and position
 * the blocks based on the level file data.
 */


public class LevelParser : MonoBehaviour
{
    public TextAsset levelFile;
    public Transform levelRoot;

    [Header("Prefabs")]
    public GameObject dirtPrefab;
    public GameObject brickPrefab;
    public GameObject questionBoxPrefab;
    public GameObject stonePrefab;
    public GameObject waterPrefab;
    public GameObject goalPrefab;
    public GameObject coinPrefab;

    void Start()
    {
        LoadLevel();
    }

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
            ReloadLevel();
    }

    void LoadLevel()
    {
        // Push lines onto a stack so we can pop bottom-up rows. This is easy to reason
        //  about, but an index-based loop over the string array is faster.
        Stack<string> levelRows = new Stack<string>();

        foreach (string line in levelFile.text.Split('\n'))
            levelRows.Push(line);

        int row = 0;
        while (levelRows.Count > 0)
        {
            string rowString = levelRows.Pop();
            char[] rowChars = rowString.ToCharArray();
            
            for (var columnIndex = 0; columnIndex < rowChars.Length; columnIndex++)
            {
                var currentChar = rowChars[columnIndex];

                // Todo - Instantiate a new GameObject that matches the type specified by the character
                // Todo - Position the new GameObject at the appropriate location by using row and column
                // Todo - Parent the new GameObject under levelRoot

                // Dirt
                if (currentChar == 'x')
                {
                    Vector3 newPostition = new Vector3(columnIndex+0.5f, row+0.5f, -0.5f);
                    Transform dirtInstance = Instantiate(dirtPrefab, levelRoot).transform;
                    dirtInstance.position = newPostition;
                }
                // Question Block
                if (currentChar == '?')
                {
                    Vector3 newPostition = new Vector3(columnIndex+0.5f, row+0.5f, -0.5f);
                    Transform questionInstance = Instantiate(questionBoxPrefab, levelRoot).transform;
                    questionInstance.position = newPostition;
                    
                    // Add animator component (if not already on the prefab)
                    var anim = questionInstance.GetComponent<QuestionTileAnimatorMPB>();
                    if (anim == null)
                    {
                        anim = questionInstance.gameObject.AddComponent<QuestionTileAnimatorMPB>();
                    }

                    anim.tilesY = 5;                 // 1/5 Y tiling
                }
                // Brick
                if (currentChar == 'b')
                {
                    Vector3 newPostition = new Vector3(columnIndex+0.5f, row+0.5f, -0.5f);
                    Transform brickInstance = Instantiate(brickPrefab, levelRoot).transform;
                    brickInstance.position = newPostition;
                }
                // Stone
                if (currentChar == 's')
                {
                    Vector3 newPostition = new Vector3(columnIndex+0.5f, row+0.5f, -0.5f);
                    Transform StoneInstance = Instantiate(stonePrefab, levelRoot).transform;
                    StoneInstance.position = newPostition;
                }
                //PT2
                // Water (hazard)
                if (currentChar == 'w')
                {
                    Vector3 newPostition = new Vector3(columnIndex + 0.5f, row + 0.5f, -0.5f);
                    Transform waterInstance = Instantiate(waterPrefab, levelRoot).transform;
                    waterInstance.position = newPostition;
                }
                //PT2
                // Goal (finish)
                if (currentChar == 'g')
                {
                    Vector3 newPostition = new Vector3(columnIndex + 0.5f, row + 0.5f, -0.5f);
                    Transform goalInstance = Instantiate(goalPrefab, levelRoot).transform;
                    goalInstance.position = newPostition;
                }
                //PT2
                // Coins
                if (currentChar == 'c')
                {
                    Vector3 newPostition = new Vector3(columnIndex + 0.5f, row + 0.5f, -0.5f);
                    Transform goalInstance = Instantiate(coinPrefab, levelRoot).transform;
                    goalInstance.position = newPostition;
                }
            }

            row++;
        }
    }

    // --------------------------------------------------------------------------
    public void ReloadLevel()
    {
        foreach (Transform child in levelRoot)
           Destroy(child.gameObject);
        
        LoadLevel();
        
        //PT2
        // Reset player position
        GameObject playerRoot = GameObject.FindGameObjectWithTag("Player");
        if (playerRoot == null)
        {
            Debug.LogWarning("ReloadLevel: Player not found (tag Player).");
            return;
        }

        CharacterController cc = playerRoot.GetComponentInChildren<CharacterController>();
        Transform t = (cc != null) ? cc.transform : playerRoot.transform;

        if (cc != null)
        {
            cc.enabled = false;
        }
        t.position = new Vector3(13f, 3f, -0.5f);
        if (cc != null)
        {
            cc.enabled = true;
        }
    }
}




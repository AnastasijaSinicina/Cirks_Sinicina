using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] tiles;
    public GameObject spawnPoint;
    public DiceRollScript diceScript;
    public GameObject winPanel;
    public TextMeshProUGUI winText;
    public Button closeButton;
    public Button restartButton;

    private int characterIndex;
    private int index;
    private GameObject mainCharacter;
    private int currentTileIndex = -1;
    private bool isMoving = false;
    private int[] otherPlayers;
    private const string textFileName = "playerNames";
    
    private float startTime;
    private float elapsedTime;

    void Start()
    {
        startTime = Time.time;
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        mainCharacter = Instantiate(playerPrefabs[characterIndex],
            spawnPoint.transform.position, Quaternion.identity);
        mainCharacter.GetComponent<NameScript>().SetPlayerName(
            PlayerPrefs.GetString("PlayerName"));

        otherPlayers = new int[PlayerPrefs.GetInt("PlayerCount")];
        string[] nameArray = ReadLinesFromFile(textFileName);

        for (int i = 0; i < otherPlayers.Length; i++)
        {
            spawnPoint.transform.position += new Vector3(0.2f, 0, 0.08f);
            index = Random.Range(0, playerPrefabs.Length);
            GameObject character = Instantiate(playerPrefabs[index],
                spawnPoint.transform.position, Quaternion.identity);
            character.GetComponent<NameScript>().SetPlayerName(
                nameArray[Random.Range(0, nameArray.Length)]);
        }
        
        winPanel.SetActive(false);
    }

    void Update()
    {
        if (diceScript.isLanded && !diceScript.hasBeenUsed && !isMoving)
        {
            int diceResult;
            if (int.TryParse(diceScript.diceFaceNum, out diceResult))
            {
                diceScript.hasBeenUsed = true;
                StartCoroutine(MovePlayer(diceResult));
            }
        }
    }

    IEnumerator MovePlayer(int steps)
    {
        isMoving = true;
        float elevation = 0.7f;
        int remaining = (tiles.Length - 1) - currentTileIndex;
        int totalSteps = steps;

        if (totalSteps <= remaining)
        {
            for (int i = 0; i < totalSteps; i++)
            {
                int nextTileIndex = (currentTileIndex == -1) ? 0 : currentTileIndex + 1;
                currentTileIndex = nextTileIndex;
                yield return StartCoroutine(MoveToTile(currentTileIndex, elevation));
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            int forwardSteps = remaining;
            int backwardSteps = totalSteps - remaining;

            for (int i = 0; i < forwardSteps; i++)
            {
                int nextTileIndex = (currentTileIndex == -1) ? 0 : currentTileIndex + 1;
                currentTileIndex = nextTileIndex;
                yield return StartCoroutine(MoveToTile(currentTileIndex, elevation));
                yield return new WaitForSeconds(0.1f);
            }

            for (int i = 0; i < backwardSteps; i++)
            {
                int nextTileIndex = currentTileIndex - 1;
                if (nextTileIndex < 0)
                    nextTileIndex = 0;

                currentTileIndex = nextTileIndex;
                yield return StartCoroutine(MoveToTile(currentTileIndex, elevation));
                yield return new WaitForSeconds(0.1f);
            }
        }

        CheckForTeleport();
        isMoving = false;

        if (currentTileIndex == 15)
        {
            ShowWinPanel();
        }
    }

    IEnumerator MoveToTile(int tileIndex, float elevation)
    {
        Vector3 startPos = mainCharacter.transform.position;
        Vector3 endPos = tiles[tileIndex].position + new Vector3(0, elevation, 0);
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            mainCharacter.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mainCharacter.transform.position = endPos;
    }

    void CheckForTeleport()
    {
     
        if (currentTileIndex == 4)
        {
            StartCoroutine(TeleportToTile(12));
        }
        else if (currentTileIndex == 10)
        {
            StartCoroutine(TeleportToTile(1));
        }
        else if (currentTileIndex == 14)
        {
            StartCoroutine(TeleportToTile(9));
        }
    }

    IEnumerator TeleportToTile(int targetTileIndex)
    {
        yield return new WaitForSeconds(0.3f);
        float elevation = 0.7f;
        currentTileIndex = targetTileIndex;

        Vector3 startPos = mainCharacter.transform.position;
        Vector3 endPos = tiles[targetTileIndex].position + new Vector3(0, elevation, 0);
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            mainCharacter.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mainCharacter.transform.position = endPos;
    }

    void ShowWinPanel()
    {
        elapsedTime = Time.time - startTime;
        winPanel.SetActive(true);
        winText.text = $"Congratulations!\nYou completed the game in  {elapsedTime:F2} seconds";

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseGame);

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(RestartGame);
    }

    public void CloseGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Останавливает Play Mode в редакторе
        #else
        Application.Quit(); // Закрывает игру в сборке
        #endif
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    string[] ReadLinesFromFile(string fName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fName);
        if (textAsset != null)
            return textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        else
        {
            Debug.LogError("File not found " + fName);
            return new string[0];
        }
    }
}

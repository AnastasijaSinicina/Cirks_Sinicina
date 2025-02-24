using UnityEngine;
using UnityEngine.SceneManagement; // ��� ����� �����

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // ������ �����
    public GameObject[] objectsToHide; // �������, ������� ���������� ��� �����
    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false); // ��������, ��� ������ ����� ������ ��� ������
    }

    // ����� ��� ������������ ����� (������������ ������� �����)
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0; // ���������� �����
            pausePanel.SetActive(true);

            foreach (GameObject obj in objectsToHide)
            {
                obj.SetActive(false); // �������� ������� �������
            }
        }
        else
        {
            ResumeGame(); // ���� ����� �����������, ������ ������������ ����
        }
    }

    // ����� ��� ������ "����������"
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // ��������� �����
        pausePanel.SetActive(false); // ������ ���� �����

        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(true); // ������� ��� ������� �������
        }
    }

    // ����� ��� ������ "����� � ����"
    public void ExitToMainMenu()
    {
        Time.timeScale = 1; // ����������� ������������ ����� ����� ������ �����
        SceneManager.LoadScene("Main Menu"); // ��������� ������� �����
    }
}

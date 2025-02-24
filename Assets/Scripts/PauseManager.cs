using UnityEngine;
using UnityEngine.SceneManagement; // Для смены сцены

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Панель паузы
    public GameObject[] objectsToHide; // Объекты, которые скрываются при паузе
    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false); // Убедимся, что панель паузы скрыта при старте
    }

    // Метод для переключения паузы (используется кнопкой паузы)
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0; // Остановить время
            pausePanel.SetActive(true);

            foreach (GameObject obj in objectsToHide)
            {
                obj.SetActive(false); // Скрываем игровые объекты
            }
        }
        else
        {
            ResumeGame(); // Если пауза выключается, просто возобновляем игру
        }
    }

    // Метод для кнопки "Продолжить"
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // Запустить время
        pausePanel.SetActive(false); // Скрыть меню паузы

        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(true); // Вернуть все скрытые объекты
        }
    }

    // Метод для кнопки "Выйти в меню"
    public void ExitToMainMenu()
    {
        Time.timeScale = 1; // Обязательно восстановить время перед сменой сцены
        SceneManager.LoadScene("Main Menu"); // Загружаем главную сцену
    }
}

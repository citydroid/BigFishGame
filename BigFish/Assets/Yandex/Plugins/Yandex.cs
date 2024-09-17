using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Yandex : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void GiveMePlayerData();

    [DllImport("__Internal")]
    private static extern void SaveExtern(string date);

    [DllImport("__Internal")]
    private static extern void LoadExtern();

    [DllImport("__Internal")]
    private static extern void ShowAdv();

    [DllImport("__Internal")]
    private static extern void RewardedAdv();

    [SerializeField] TextMeshProUGUI _nameText;
    //[SerializeField] RawImage _photo;

    public static Yandex Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    /// <summary>
    /// Авторизация и получение имени игрока
    /// </summary> *********************************
    public void PlayerData()
    {
        //Отправляет запрос на получение имени и возвращает имя в SetName(string _name)
        GiveMePlayerData();
    }
    public void SetName(string _name)
    {
        _nameText.text = _name;
    }
    //**********************************************

    public void LoadParam()
    {
        LoadExtern();
    }
    public void GetPlayerParam(string value)
    {
        Progress.Instance.SetPlayerInfo(value);
    }

    public void SaveParam(string jsonString)
    {
        SaveExtern(jsonString);
    }



        //Для показа банера
        public void ShowAdvMethod()
    {
        ShowAdv();
    }

    /// <summary>
    /// Реклама с вознаграждением
    /// </summary>-------------------------------------------
    public void RevardAdv()
    {
        RewardedAdv();
    }
    //Запуск после вознаграждения
    public void AfterAdvAction()
    {
        SceneManager.LoadScene(1);
    }
    //-------------------------------------------------------
}

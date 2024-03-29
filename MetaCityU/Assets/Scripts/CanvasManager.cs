using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Text _tokenBalanceText;

    [SerializeField] private Button _exitCampusButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _tokenBalanceText.text = "Token Balance: " + (PlayerData.Instance == null ? "null" : PlayerData.Instance.LocalBalance.ToString());
    }

    public void EnterCampus()
    {
        _exitCampusButton.gameObject.SetActive(true);
    }

    public void ExitCampus()
    {
        _exitCampusButton.gameObject.SetActive(false);
    }
}

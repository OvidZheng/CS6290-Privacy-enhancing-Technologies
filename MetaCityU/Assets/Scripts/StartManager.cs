using System.Threading.Tasks;
using UnityEngine;
using Thirdweb;
using UnityEngine.SceneManagement;


public class StartManager : MonoBehaviour
{

    [SerializeField] private GameObject connectButton;

    [SerializeField] private GameObject connectedButton;

    [SerializeField] private GameObject enterButton;

    [SerializeField] private TMPro.TextMeshProUGUI addressTxt;

    [SerializeField] private TMPro.TextMeshProUGUI balanceTxt;

    public async void ConnectToWallet()
    {
        #if !UNITY_EDITOR
        //连接钱包
        string address = await SDKManager.Instance.SDK.wallet.Connect(new WalletConnection()
        {
            provider = WalletProvider.CoinbaseWallet,
            chainId = 5 // Switch the wallet Goerli on connection
        });
        #else
        
        string address = "Local Address";
        
        #endif
        
        //更新UI界面
        addressTxt.text = address;
        connectButton.SetActive(false);
        connectedButton.SetActive(true);

        //获取余额
        await CheckBalance();
        
        //准备进入下一场景
        enterButton.SetActive(true);
    }

    public async Task CheckBalance()
    {
        #if !UNITY_EDITOR
        
        //获取智能合同
        balanceTxt.text = "Start Fetching Balance";
        Contract contract = SDKManager.Instance.SDK.GetContract("0xcF157113bD328Ab070DBb1eD74747225a7BDD67d");
        CurrencyValue balance = await contract.ERC20.Balance();
        
        //保存余额到本地
        PlayerData.Instance.LocalBalance = float.Parse(balance.displayValue);
        balanceTxt.text = balance.displayValue;
        
        #else
        
        PlayerData.Instance.LocalBalance = 999;
        balanceTxt.text = "999";
        
        #endif
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("Entrance");
    }
}

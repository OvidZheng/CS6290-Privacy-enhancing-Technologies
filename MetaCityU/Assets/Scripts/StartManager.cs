using System.Threading.Tasks;
using UnityEngine;
using Thirdweb;


public class StartManager : MonoBehaviour
{

    [SerializeField] private GameObject connectButton;

    [SerializeField] private GameObject connectedButton;

    [SerializeField] private TMPro.TextMeshProUGUI addressTxt;

    [SerializeField] private TMPro.TextMeshProUGUI balanceTxt;

    public async void ConnectToWallet()
    {
        // Connect to the user's wallet via CoinbaseWallet
        string address = await SDKManager.Instance.SDK.wallet.Connect(new WalletConnection()
        {
            provider = WalletProvider.CoinbaseWallet,
            chainId = 5 // Switch the wallet Goerli on connection
        });
        
        addressTxt.text = address;
        connectButton.SetActive(false);
        connectedButton.SetActive(true);

        await CheckBalance();
    }

    public async Task CheckBalance()
    {
        balanceTxt.text = "Start Fetching Balance";
        Contract contract = SDKManager.Instance.SDK.GetContract("0xcF157113bD328Ab070DBb1eD74747225a7BDD67d");
        CurrencyValue balance = await contract.ERC20.Balance();
        balanceTxt.text = balance.displayValue;
    }
}

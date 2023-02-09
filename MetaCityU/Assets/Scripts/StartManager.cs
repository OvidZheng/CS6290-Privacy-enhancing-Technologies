using UnityEngine;
using Thirdweb;

public class StartManager : MonoBehaviour
{

    [SerializeField] private GameObject connectButton;

    [SerializeField] private GameObject connectedButton;

    [SerializeField] private TMPro.TextMeshProUGUI addressTxt;

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
    }
}

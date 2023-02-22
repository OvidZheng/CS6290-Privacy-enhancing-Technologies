using System.Threading.Tasks;
using UnityEngine;
using Thirdweb;
using UnityEngine.Serialization;

public class SDKManager : MonoBehaviour
{
    /// <summary>
    /// Manager单例
    /// </summary>
    public static SDKManager Instance;
    
    public ThirdwebSDK SDK;
    public string _tokenDropContractAddress = "0xcF157113bD328Ab070DBb1eD74747225a7BDD67d";
    private int _userTokenNum;
    private void Awake()
    {
        //单例已经实现则销毁冗余
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public async Task<string> Connect()
    {
        string addr =
            await SDK
                .wallet
                .Connect(new WalletConnection()
                {
                    provider = WalletProvider.MetaMask,
                    chainId = 5 // Switch the wallet Optimism Goerli network on connection
                });
        return addr;
    }

    public Contract GetTokenDropContract()
    {
        return SDK
            .GetContract(_tokenDropContractAddress);
    }

    public async Task<TransactionResult> Claim(int num)
    {
        await Connect();
        var contract = GetTokenDropContract();
        return await contract.ERC20.Claim(num.ToString());
    }

    public async Task CheckBalance()
    {
        await Connect();
        
        var contract = GetTokenDropContract();
        CurrencyValue balance = await contract.ERC20.Balance();
        
        //保存余额到本地
        PlayerData.Instance.LocalBalance = float.Parse(balance.displayValue);
    }

    void Start()
    {
        #if UNITY_EDITOR
        return;
        #endif
        
        SDK = new ThirdwebSDK("goerli");
    }
}
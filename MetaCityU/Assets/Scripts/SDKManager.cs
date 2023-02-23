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
    public string _marketPlaceContractAddress = "0x2C3B134B94Fe39b15814db54591C242a6C0EC74B";
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
    
    /// <summary>
    /// 链接到钱包
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// 获取掉落token的contract
    /// </summary>
    /// <returns></returns>
    public Contract GetTokenDropContract()
    {
        return SDK
            .GetContract(_tokenDropContractAddress);
    }
    
    /// <summary>
    /// 为自己增加token
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public async Task<TransactionResult> Claim(int num)
    {
        await Connect();
        var contract = GetTokenDropContract();
        return await contract.ERC20.Claim(num.ToString());
    }
    
    
    /// <summary>
    /// 检查token余额
    /// </summary>
    public async Task CheckBalance()
    {
        await Connect();
        
        var contract = GetTokenDropContract();
        CurrencyValue balance = await contract.ERC20.Balance();
        
        //保存余额到本地
        PlayerData.Instance.LocalBalance = float.Parse(balance.displayValue);
    }
    
    /// <summary>
    /// 获取商店contract地址
    /// </summary>
    /// <returns></returns>
    public Marketplace GetMarketplaceContract()
    {
        return SDK
            .GetContract(_marketPlaceContractAddress)
            .marketplace;
    }
    
    /// <summary>
    /// 购买物品
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public async Task<TransactionResult> BuyItem(string itemId)
    {
        await Connect();
        return await GetMarketplaceContract().BuyListing(itemId, 1);
    }

    void Start()
    {
        #if UNITY_EDITOR
        return;
        #endif
        
        SDK = new ThirdwebSDK("goerli");
    }
}
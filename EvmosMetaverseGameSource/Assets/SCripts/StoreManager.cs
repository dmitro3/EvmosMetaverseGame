using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager insta;
    [SerializeField] GameObject itemPanelUI;
    [SerializeField] GameObject itemPurchaseUI;

    //item panel stuff
    //[SerializeField] Button[] itemButtons;


    //purcahse panel stuff
    [SerializeField] RawImage purchaseItemImg;
    [SerializeField] TMP_Text purchaseItemText;
    [SerializeField] TMP_Text purchaseItemCostText;

    int currentSelectedItem = -1;

    [SerializeField] Transform itemParent;
    //item panel stuff
    [SerializeField] GameObject itemButtonPrefab;

    private void Awake()
    {
        insta = this;
    }



    private void OnEnable()
    {
        ClosePurchasePanel();

      /*  if (CovalentManager.loadingData) {
            MessaeBox.insta.showMsg("Loading Data",true);
            CloseItemPanel();
            return;
        }*/


        foreach (Transform child in itemParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < SingletonDataManager.metanftlocalData.Count; i++)
        {
            bool check = false;
            bool unlocked = false;
            for (int j = 0; j < SingletonDataManager.myNFTData.Count; j++)
            {
                //Debug.Log("checkID " + SingletonDataManager.myNFTData[i].itemid);
                if (SingletonDataManager.myNFTData[j].itemid == i)
                {
                    check = true;
                    //break;
                    //continue;
                }
                if (SingletonDataManager.myNFTData[j].itemid == 0) unlocked = true;
            }

            //Debug.Log("check " + check + " | " + i);
            if (!check)
            {
                var temp = Instantiate(itemButtonPrefab, itemParent);
                temp.GetComponent<RawImage>().texture = SingletonDataManager.metanftlocalData[i].imageTexture;
                temp.transform.GetChild(0).GetComponent<TMP_Text>().text = SingletonDataManager.metanftlocalData[i].cost.ToString();
                var tempNo = i;
                var tempTexture = SingletonDataManager.metanftlocalData[i].imageTexture;
                temp.GetComponent<Button>().onClick.AddListener(() => SelectItem(tempNo, tempTexture));

                if (!unlocked && i != 0) temp.GetComponent<Button>().interactable = false;
            }
        }
        //SingletonDataManager.insta.LoadPurchasedItems();
    }


    public void SelectItem(int _no, Texture _texture)
    {
        Debug.Log("Selected item " + _no);
        currentSelectedItem = _no;
        itemPanelUI.SetActive(false);
        itemPurchaseUI.SetActive(true);
        purchaseItemImg.texture = _texture;// itemButtons[_no].GetComponent<RawImage>().texture;
        purchaseItemText.text = SingletonDataManager.metanftlocalData[_no].description;
        purchaseItemCostText.text = SingletonDataManager.metanftlocalData[_no].cost.ToString();


    }

    public void purchaseItem()
    {
        Debug.Log("purchaseItem");
        MetadataNFT meta = new MetadataNFT();
        if (SingletonDataManager.userData.score >= SingletonDataManager.metanftlocalData[currentSelectedItem].cost)
        {
            meta.itemid = SingletonDataManager.metanftlocalData[currentSelectedItem].itemid;
            meta.name = SingletonDataManager.metanftlocalData[currentSelectedItem].name;
            meta.description = SingletonDataManager.metanftlocalData[currentSelectedItem].description;
            meta.image = SingletonDataManager.metanftlocalData[currentSelectedItem].imageurl;
            //meta.itemid = SingletonDataManager.metanftlocalData[currentSelectedItem].

           // NFTPurchaser.insta.StartCoroutine(NFTPurchaser.insta.UploadNFTMetadata(Newtonsoft.Json.JsonConvert.SerializeObject(meta), SingletonDataManager.metanftlocalData[currentSelectedItem].cost, SingletonDataManager.metanftlocalData[currentSelectedItem].itemid));
        }
        else
        {
            Debug.Log("not enough money");
            MessaeBox.insta.showMsg("No enough coins\nFight to earn coins", true);
        }
    }

    public void ClosePurchasePanel()
    {
        itemPanelUI.SetActive(true);
        itemPurchaseUI.SetActive(false);
        
    }

    public void CloseItemPanel()
    {
        itemPanelUI.SetActive(false);
        itemPurchaseUI.SetActive(false);
        Debug.Log("close");
        ///if (!CovalentManager.loadingData) CovalentManager.insta.GetNFTUserBalance();
        foreach (Transform child in itemParent)
        {
            Destroy(child.gameObject);
        }
        gameObject.SetActive(false);
    }
}

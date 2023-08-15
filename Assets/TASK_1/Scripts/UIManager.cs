using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    #region singleton Seup
    // setting up singleton //
    public static UIManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        //DontDestroyOnLoad(gameObject);
    }

    #endregion

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas detailsInfoCanvas;
    
    [SerializeField] private TMP_Text detailInfoText;
    [SerializeField] private TMP_Dropdown _dropDownMenu;

    [SerializeField] private GameObject detailsCanvasBody;
    [SerializeField] private float _shakeDuration = 1f;
    [SerializeField] private float _shakeStrength = 10f;
    
    [SerializeField] private Bar barPrefab;
    [SerializeField] private Transform barTransform;

    private ClientAuth _currentClientAuth = ClientAuth.All;
    private ClientDataType _clientData = new ClientDataType();
    private List<Bar> Bars = new List<Bar>();


    private void Start()
    {
        _dropDownMenu.onValueChanged.AddListener( delegate{dropDownValueChanges(_dropDownMenu);});
    }

    /// <summary>
    /// Sets up client data recieved from server and parsed from JSON
    /// </summary>
    /// <param name="_clientData"></param>
    public void SetupClientData(ClientDataType _clientData)
    {
        this._clientData = _clientData;

        foreach(var _data in _clientData.clients)
        {
            var _bar = Instantiate(barPrefab, barTransform);
            Bars.Add(_bar);

            DetailsData _detailsData = _clientData.DataDictionary.ContainsKey(_data.id.ToString())? _clientData.DataDictionary[_data.id.ToString()] : new DetailsData();
            _bar.SetupData(_data, _detailsData);
        }
        SetupDropDownMenu();
    }

    /// <summary>
    /// Sets up DropDown Menu's option from the ClientAuth Enum
    /// </summary>
    private void SetupDropDownMenu()
    {
       foreach(var _val in System.Enum.GetValues(typeof(ClientAuth)))
             _dropDownMenu.options[(int)_val].text = _val.ToString();

        _dropDownMenu.captionText.text = ((ClientAuth)0).ToString();
    }

    /// <summary>
    /// Opens up details of a client
    /// </summary>
    /// <param name="_details"></param>
    public void OpenDetails(DetailsData _details)
    {
        detailsInfoCanvas.gameObject.SetActive(true);
        string _dText = string.Format("Name : {0}\n Points : {1}\n Address : {2}\n",
                                    _details.name,_details.points, _details.address);

        detailInfoText.text = _dText;
        ShakeObj(detailsCanvasBody);
        mainCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Dotween shake effect for simple transition animation
    /// </summary>
    /// <param name="go"></param>
    private void ShakeObj(GameObject go)
    {
        go.transform.DOShakePosition(_shakeDuration, _shakeStrength);
        go.transform.DOShakeRotation(_shakeDuration, _shakeStrength);
        go.transform.DOShakeScale(_shakeDuration, _shakeStrength);
    }

    /// <summary>
    /// Closes already opneded detailsInfo canvas
    /// </summary>
    public void CloseDetails()
    {
        detailInfoText.text = "";
        detailsInfoCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// Updates client info list according to 
    /// current Client Authtype (All, ManagerOnly, Non Managers)
    /// </summary>
    private void UpdateBarList()
    {
        foreach(var bar in Bars)
        {
            if (_currentClientAuth == ClientAuth.All)
            {
                bar.gameObject.SetActive(true);
                continue;
            }
            if(_currentClientAuth == ClientAuth.ManagerOnly)
            {
                bar.gameObject.SetActive(bar.IsManager());
                continue;
            }
            if(_currentClientAuth == ClientAuth.NonManagers)
            {
                bar.gameObject.SetActive(!bar.IsManager());
                continue;
            }  
        }
    }

    /// <summary>
    /// subscribed delegate for any changes in dropdown menu
    /// </summary>
    /// <param name="change"></param>
    private void dropDownValueChanges(TMP_Dropdown change)
    {
        _currentClientAuth = (ClientAuth)change.value;
        UpdateBarList();
        ShakeObj(barTransform.gameObject);
    }
}

/// <summary>
/// auth access of client
/// </summary>
public enum ClientAuth
{
    All = 0,
    ManagerOnly = 1,
    NonManagers = 2
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bar : MonoBehaviour
{
    [SerializeField] private ClientData _cData;
    [SerializeField] private DetailsData _details;
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _pointText;

    //sets up data of a client on the bar
    public void SetupData(ClientData _cData, DetailsData _details)
    {
        this._cData = _cData;
        this._details = _details;
        
        _labelText.text = _cData.label;
        _pointText.text = _details.points;
    }

    //returns this clients auth type
    public bool IsManager()
    {
        switch (_cData.isManager)
        {
            case "true":
                return true;
            case "false":
                return false;
        }
        return true;
    }

    //Calls when this Bar is clicked
    public void OnThisBarClicked()
    {
        UIManager.instance.OpenDetails(_details);
    }
}

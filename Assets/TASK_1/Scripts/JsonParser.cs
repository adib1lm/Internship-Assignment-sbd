using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using UnityEngine.Networking;

public class JsonParser : MonoBehaviour
{
    private string JSON_STRING = "";

    public const string URL = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";
    public ClientDataType _retrievedData = new ClientDataType();    

    void Start()
    {
        StartCoroutine(GetDataFromServer());
    }

    private IEnumerator GetDataFromServer()
    {
        using(UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                JSON_STRING = request.downloadHandler.text;
                //Debug.Log(JSON_STRING);
                ParseJSON();
            }
        }
    }

    private void ParseJSON()
    {
        _retrievedData = JsonUtility.FromJson<ClientDataType>(JSON_STRING);
        BruteForceData();
        UIManager.instance.SetupClientData(_retrievedData);
    }

    /// <summary>
    /// The "data" node of the json is most likely serialized from a dictionary or key value pair List
    /// Because of the limitations with Unity's build in Json utility,
    /// it is not possible to directly retrieve data  from Json string to dictionary or KVP list
    /// as I am not allowed to use any external Json Packages (i.e: Newtonsoft.Json, simpleJSON),
    /// A least time consuming and Optimal wayout that I could think of for this task is:
    /// Optimized bruteforce on the "data" node to retrive the data to Dictionary
    /// </summary>
    
    ///Identifies DetailsData Type's start index in the JSON for a given id
    private void BruteForceData()
    {
        foreach(var client in _retrievedData.clients)
        {
            string _id = client.id;
            string _identifier = "\""+_id+"\":";
            int index = JSON_STRING.LastIndexOf(_identifier);
          //  Debug.Log(index);
            if(index != -1)
                SeparateData(_id, index + 4);
        }
    }

    /// <summary>
    /// Separates DetailsData type of value from JSON
    /// creates a json string with the appropiate structure for DetailsDataType
    /// retrieves data from json and adds them to Dictionary<string, DetailsData>
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_index"></param>

    private void SeparateData(string _id, int _index)
    {
        string _separatedJson = "{\"dataNode\" : ";
        char _currentChar = '.';
        while(_currentChar != '}' && _index < JSON_STRING.Length)
        {
            _separatedJson = _separatedJson + JSON_STRING[_index];
            _currentChar = JSON_STRING[_index];
            _index++;
        }
        _separatedJson = _separatedJson + "}";
        //Debug.Log(_separatedJson);
        var _dat = JsonUtility.FromJson<DetailsDataNode>(_separatedJson);
        _retrievedData.DataDictionary.Add(_id, _dat.dataNode);
    }

    #region FOR DEBUG
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            debugData(_testData.Data["1"]);
            debugData(_testData.Data["2"]);
            debugData(_testData.Data["3"]);
        }
    }

    private void debugData(DetailsData _dat)
    {   
        Debug.Log("address : " + _dat.address + "\n");
        Debug.Log("points : " + _dat.points + "\n");
        Debug.Log("name : " + _dat.name + "\n");
    }
    */
    #endregion
}

/// <summary>
/// holds "clients" node data, label data and creates a dictionary for "data" nodes
/// </summary>
[System.Serializable]
public class ClientDataType
{
    public List<ClientData> clients;

    public string label;

    public Dictionary<string, DetailsData> DataDictionary = new Dictionary<string, DetailsData>();
}

[System.Serializable]
public class ClientData
{
    public string isManager;
    public string id;
    public string label;
}

/// <summary>
/// needed this for the bruteforce on the "data" Node
/// </summary>
[System.Serializable]
public class DetailsDataNode
{
    public DetailsData dataNode;
}

/// <summary>
/// "data" : {"1" : DetailsData, "2" : DetailsData,....}
/// same structure as the value of key value Pair of data node
/// </summary>
[System.Serializable]
public struct DetailsData
{
    public string address;
    public string name;
    public string points;
}

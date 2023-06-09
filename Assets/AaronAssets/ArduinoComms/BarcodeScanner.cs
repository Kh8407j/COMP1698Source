using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

public class BarcodeScanner : MonoBehaviour
{
    

    void Start()
    {
        StartCoroutine(GetProductInfo());
    }

    IEnumerator GetProductInfo()
    {
        string url = "https://world.openfoodfacts.org/api/v0/product/" + ManagerVar.Instance.barValue + ".json";

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("JSON Response: " + json); // Log the JSON response for debugging purposes

            // Use Json.NET to read the JSON response as a stream
            using (JsonTextReader reader = new JsonTextReader(new System.IO.StringReader(json)))
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "nova_groups")
{
    reader.Read();
    if (int.TryParse((string)reader.Value, out int novaGroups))
    {
        ManagerVar.Instance.enemyDamage = novaGroups;
        Debug.Log("Nova Group: " + ManagerVar.Instance.enemyDamage);
    }
}
else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "nutrition_grades")
{
    reader.Read();
    char nutritionGradeChar = ((string)reader.Value)[0];
    ManagerVar.Instance.nutritionGrade = char.ToUpper(nutritionGradeChar) - 64;
    Debug.Log("Numeric Nutrition Grade: " + ManagerVar.Instance.nutritionGrade);
}
                }
            }
        }
        else
        {
            Debug.Log("Error retrieving product info: " + request.error);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropCurrency
{
    public string name;
    public GameObject item;
    public int dropRarity;
}

public class LootController : MonoBehaviour
{
    public List<DropCurrency> lootTable = new List<DropCurrency>();
    public ReportController reportController;

    void Start()
    {
        reportController = reportController.GetComponent<ReportController>();
    }

    //Calculo de quais itens dropar com base no rate
    public void CalculateLoot(GameObject other)
    {
        int dropRate = Random.Range(0, 101);

        for (int i = 0; i < lootTable.Count; i++)
        {
            if (dropRate <= lootTable[i].dropRarity)
            {
                Vector3 boxPos = new Vector3(other.transform.position.x, other.transform.position.y + 0.1f, other.gameObject.transform.position.z);
                GameObject item = (GameObject)Instantiate(lootTable[i].item, boxPos, Quaternion.identity);
                item.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-10f, 10f), Random.Range(0f,15f), 0f), ForceMode2D.Impulse);

                Destroy(item, 10f);
            }
        }
        Destroy(other);
    }
}

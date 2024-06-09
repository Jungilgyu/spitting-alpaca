using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Inventory>();
            }

            return m_instance;
        }
    }

    private static Inventory m_instance;
    private string gooditem = "";

    void Start()
    {
       
    }

    public void AddItem(GameObject item)
    {
        gooditem = item.name;
        Debug.Log("������ ȹ��: " + item.name);
    }
    //�¾����ۻ��
    public string useGooditem()
    {
        Debug.Log("�����ۻ��");
        string realgooditem = gooditem;
        gooditem = "";
        return realgooditem;
    }
}

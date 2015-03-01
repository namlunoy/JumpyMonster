using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/**
API : 2d4dfa4b3add90b8ac1bcf1540f53895811bc07c8afc6833e06af458bb512f64
Secret : 3a296b9be61e021af1efb18de756c7f25a60f67cfc0485848bf938e9f71952a8
 * Name : JumpyJumpy
 * 
 * 
 * 
 * */
public class ListviewRunners : MonoBehaviour
{

    //Các thuộc tính của người chơi cần quan tâm
    public static int GetSelectedIndex { get { return selectedIndex; } }
    private static int selectedIndex = 0;
    private float tam_x = 0;
    private float width;
    private float heigh;
    //Lấy level mà người chơi
    private int NumberMontersOpened
    {
        get
        {
            if (PlayerPrefs.GetInt("NumberMontersOpened", 0) == 0)
                PlayerPrefs.SetInt("NumberMontersOpened", 1);
            return PlayerPrefs.GetInt("NumberMontersOpened");
        }
        set { PlayerPrefs.SetInt("NumberMontersOpened", value); }
    }
    //Ảnh con monster chưa mở được
    public Sprite unopenMonster;
    //Danh sách ảnh các con monster
    public Sprite[] MonsterImages;
    //Item Model
    public RectTransform itemModel;
    //Items Panel
    public RectTransform itemsPanel;


    void Start()
    {

        width = itemModel.offsetMax.x - itemModel.offsetMin.x;
        heigh = itemModel.offsetMax.y - itemModel.offsetMin.y;

        print(width + " - " + heigh);

        //Định dạng kich thước của Panel
        itemsPanel.offsetMin = new Vector2(0, -heigh);
        itemsPanel.offsetMax = new Vector2(width * MonsterImages.Length, 0);

        //Định dạng kích thước của từng item
        for (int i = 0; i < MonsterImages.Length; i++)
        {
            RectTransform item = ((GameObject)GameObject.Instantiate(itemModel.gameObject)).GetComponent<RectTransform>();
            //Cho làm con của panel
            item.transform.parent = itemsPanel.transform;

            //Thay ảnh
            Image image = item.GetComponent<Image>();
            if (i < NumberMontersOpened)
                image.sprite = MonsterImages[i];
            else image.sprite = unopenMonster;

            //Định vị trí
            item.offsetMin = new Vector2(i * width, -heigh);
            item.offsetMax = new Vector2((i + 1) * width, 0);

            //Đẩm bảo đúng scale
            item.localScale = new Vector3(1, 1, 1);
        }

        //Đầu tiên tính tọa độ tâm tương ứng tại vị trí đầu tiên
        tam_x = MonsterImages.Length * width / 2 - width / 2;

        SetIndex(NumberMontersOpened - 1);
    }


    void Update()
    {
        //  print(itemsPanel.gameObject.transform.position);
    }
    public void Click_Left()
    {
        if ((selectedIndex - 1) >= 0)
        {
            audio.Play();
            selectedIndex--;
            iTween.MoveTo(itemsPanel.gameObject, iTween.Hash("x", tam_x - selectedIndex * width, "islocal", true, "time", 0.2f, "easetype", iTween.EaseType.linear));
        }
    }

    public void Click_Right()
    {
        if (selectedIndex + 1 < MonsterImages.Length)
        {
            audio.Play();
            selectedIndex++;
            iTween.MoveTo(itemsPanel.gameObject, iTween.Hash("x", tam_x - selectedIndex * width, "islocal", true, "time", 0.2f, "easetype", iTween.EaseType.linear));
        }
    }

    public void SetIndex(int index)
    {
        if (index >= 0 && index < MonsterImages.Length)
        {
            selectedIndex = index;
            iTween.MoveTo(itemsPanel.gameObject, iTween.Hash("x", tam_x - selectedIndex * width, "islocal", true, "time", 0.2f, "easetype", iTween.EaseType.linear));
        }
    }


    public void UserScroll(Vector2 pos)
    {

    }
}
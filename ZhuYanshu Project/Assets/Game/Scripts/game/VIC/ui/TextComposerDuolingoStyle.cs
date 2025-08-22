using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui
{
    public class TextComposerDuolingoStyle : MonoBehaviour
    {
        public SlotDuolingoStyle[] slots;
        public string[] itemWords;

        public ItemDuolingoStyle prefab_ItemDuolingoStyle;
        public Vector2 item_startAnchoredPos;
        public float item_offsetX;
        public float item_offsetY;
        List<ItemDuolingoStyle> crtItems = new List<ItemDuolingoStyle>();

        void Start()
        {
            int i = 0;
            foreach (var iw in itemWords)
            {
                CreateItem(iw, i);
                i++;
            }
        }

        void CreateItem(string s, int i)
        {
            var newItem = Instantiate(prefab_ItemDuolingoStyle, prefab_ItemDuolingoStyle.transform.parent);
            int line = 0;
            int row = i;

            if (i >= 8)
            {
                line = 2;
                row -= 8;
            }
            else if (i >= 4)
            {
                line = 1;
                row -= 4;
            }
            var anchoredPos = item_startAnchoredPos + new Vector2(item_offsetX * row, item_offsetY * line);
            newItem.Init(anchoredPos, s);
            newItem.gameObject.SetActive(true);

            crtItems.Add(newItem);
        }

        public bool TryMoveMe(ItemDuolingoStyle item)
        {
            float moveTime = 0.6f;
            if (item.state == ItemDuolingoStyle.State.Pending)
            {
                foreach (var s in slots)
                {
                    if (s.crtItem == null)
                    {
                        item.GetComponent<RectTransform>().DOAnchorPos(s.GetComponent<RectTransform>().anchoredPosition, moveTime);
                        item.state = ItemDuolingoStyle.State.Done;
                        s.crtItem = item;
                        return true;
                    }
                }
            }
            else if (item.state == ItemDuolingoStyle.State.Done)
            {
                item.GetComponent<RectTransform>().DOAnchorPos(item.startPos, moveTime);
                item.state = ItemDuolingoStyle.State.Pending;

                foreach (var s in slots)
                {
                    if (s.crtItem == item)
                    {
                        s.crtItem = null;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
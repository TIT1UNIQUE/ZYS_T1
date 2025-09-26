using Assets.Game.Scripts.game.VIC.ui.Message.MessageComposer;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

namespace Assets.Game.Scripts.game.VIC.ui
{
    public class TextComposerDuolingoStyle : MonoBehaviour
    {
        public ItemDuolingoStyle prefab_item;
        public SlotDuolingoStyle prefab_slot;

        public Vector2 item_startAnchoredPos;
        public float item_offsetX;
        public float item_offsetY;


        public CanvasGroup cg;
        List<SlotDuolingoStyle> slots = new List<SlotDuolingoStyle>();
        List<ItemDuolingoStyle> items = new List<ItemDuolingoStyle>();

        public TextMeshProUGUI bodyText;
        MessageComposerPrototype currentMcp;

        public GameObject submitButton;

        private void Start()
        {
            submitButton.SetActive(false);
        }

        void CreateItem(string s, int i)
        {
            var newItem = Instantiate(prefab_item, prefab_item.transform.parent);
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

            items.Add(newItem);
        }
        void CreateBlank(MessageBlank b, int blankIndex)
        {
            var newSlot = Instantiate(prefab_slot, prefab_slot.transform.parent);
            newSlot.Init(b.anchorPos, blankIndex, b.correctAnswerIndex);
            newSlot.gameObject.SetActive(true);

            slots.Add(newSlot);
        }
        public bool TryMoveMe(ItemDuolingoStyle item)
        {
            float moveTime = 0.5f;
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

        public void UpdateSubmitState()
        {
            var allFilled = true;
            foreach (var s in slots)
            {
                if (s.crtItem == null)
                {
                    allFilled = false;
                }
            }

            submitButton.SetActive(allFilled);
            //todo evaluation score
            //currentMcp;
        }

        void ClearSlots()
        {
            foreach (var s in slots)
            {
                Destroy(s.gameObject);
            }
            slots.Clear();
        }
        void ClearOptions()
        {
            foreach (var s in items)
            {
                Destroy(s.gameObject);
            }
            items.Clear();
        }

        public void Setup(MessageComposerPrototype mcp)
        {
            //Debug.Log("TextComposerDuolingoStyle Setup");
            //Debug.Log(mcp);
            currentMcp = mcp;
            submitButton.SetActive(false);

            if (mcp == null)
            {
                cg.alpha = 0;
                cg.blocksRaycasts = false;
            }
            else
            {
                ClearSlots();
                ClearOptions();
                cg.alpha = 1;
                cg.blocksRaycasts = true;

                bodyText.text = mcp.rawText;

                int i = 0;
                foreach (var b in mcp.blanks)
                {
                    CreateBlank(b, i);
                    i++;
                }

                i = 0;
                foreach (var iw in mcp.options)
                {
                    CreateItem(iw, i);
                    i++;
                }
            }
        }
    }
}
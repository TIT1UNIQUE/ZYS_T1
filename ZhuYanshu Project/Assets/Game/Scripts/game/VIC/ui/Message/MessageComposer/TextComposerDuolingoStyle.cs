using Assets.Game.Scripts.game.VIC.ui.Message.MessageComposer;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;
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
        public MessageComposerPrototype currentMcp { get; private set; }

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
            Debug.Log("TextComposerDuolingoStyle Setup");
            Debug.Log(mcp);
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

        public List<string> GetOptionsString()
        {
            List<string> res = new List<string>();
            foreach (var s in slots)
            {
                res.Add(s.crtItem.txt.text);
            }
            return res;
        }

        public (int, int) GetScoreOfTotalScore()
        {
            var res = (0, 0);
            return res;
        }

        public string GetFinalString()
        {
            var s = currentMcp.rawText;
            //Debug.Log("GetFinalString rawText: " + s);
            var options = GetOptionsString();
            s = UnifyUnderscores(s);
            var res = FillBlanks(s, options);
            Debug.Log("finalString: " + res);
            return res;
        }

        //given a string， how to replace all more-than-4 underscores such as "_________" to "____"
        public static string UnifyUnderscores(string s)
        {
            string output = Regex.Replace(s, @"_{5,}", "____");
            return output;
        }

        public static string FillBlanks(string s, List<string> options)
        {
            if (string.IsNullOrEmpty(s) || options == null || options.Count == 0)
                return s;

            const string blank = "____";          // 4 underscores
            int idx = 0;
            var sb = new System.Text.StringBuilder(s.Length);

            for (int i = 0; i < s.Length;)
            {
                // quick check: enough characters left for a match?
                if (i + blank.Length <= s.Length &&
                    s.AsSpan(i, blank.Length).SequenceEqual(blank))
                {
                    // found a blank – replace if we still have options
                    sb.Append(idx < options.Count ? options[idx++] : blank);
                    i += blank.Length;
                }
                else
                {
                    // ordinary character
                    sb.Append(s[i++]);
                }
            }
            return sb.ToString();
        }
    }
}
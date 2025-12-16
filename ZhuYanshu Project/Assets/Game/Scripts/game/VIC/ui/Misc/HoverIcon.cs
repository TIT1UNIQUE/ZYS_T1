using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Assets.Game.Scripts.game.VIC.ui.Misc
{
    public class HoverIcon : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject hover;
        public ArrowRingSpawner ars;
        public void OnPointerEnter(PointerEventData eventData)
        {
            hover.SetActive(true);
            if (ars!=null)
            {
                ars.SpawnAllWaves();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hover.SetActive(false);
        }
    }
}
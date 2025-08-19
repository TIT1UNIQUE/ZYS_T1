using com;
using DG.Tweening;
using Rescue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Game.Scripts.game.RescueGame
{
    public class MedicineBottle : DragDropTarget
    {
        [SerializeField] DragDropContainer mouthContainer;
        [SerializeField] DragDropContainer fromContainer;
        public override void OnEndDrag(PointerEventData eventData)
        {
            if (freeDrag)
            {
                GetComponent<Image>().raycastTarget = true;
                return;
            }

            DragDropContainer endContainer = null;
            foreach (var ddc in containers)
            {
                if (ddc.inside)
                {
                    //Debug.Log("OnEndDrag " + ddc);
                    endContainer = ddc;
                    break;
                }
            }

            if (endContainer == null)
            {
                SetToDragDropContrainer(_startDDC);
            }
            else
            {
                SetToDragDropContrainer(endContainer);
                if (mouthContainer == endContainer)
                {
                    EatPills();
                }
            }

            foreach (var ddc in containers)
            {
                if (ddc != endContainer)
                    ddc.inside = false;
            }
        }


        void EatPills()
        {
            StartCoroutine(EatPillCoroutine());
        }

        IEnumerator EatPillCoroutine()
        {
            GetComponent<Image>().raycastTarget = false;
            SoundSystem.instance.Play("pill");
            transform.DORotate(new Vector3(0, 0, 90), 2.0f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(1.0f);
            SoundSystem.instance.Play("eat");
            yield return new WaitForSeconds(1.0f);
            SoundSystem.instance.Play("eat");

            transform.DORotate(new Vector3(0, 0, 0), 2.0f).SetEase(Ease.InOutCubic);
            yield return new WaitForSeconds(2.0f);
            transform.DOMove(fromContainer.goodPlaceRef.position, 1.2f).SetEase(Ease.InOutCubic);
            yield return new WaitForSeconds(1.2f);

            GetComponent<Image>().raycastTarget = true;
            RescueSystem.instance.OnEatPill();

            _startDDC = fromContainer;
            SetToDragDropContrainer(_startDDC);
            fromContainer.inside = true;
            mouthContainer.inside = false;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            SoundSystem.instance.Play("drag pill start");
        }
    }
}
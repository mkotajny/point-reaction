using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Afrenchguy
{
    public class TextsListAnimation : MonoBehaviour
    {
        /**********************************************************************/

        private static float M_POS_OUT      = -130f;
        private static float M_DELAY_TEXT   = 0.25f;
        private static float M_TIME_SHOW    = 0.5f;
        private static float M_TIME_WAIT    = 2.5f;
        private static float M_TIME_HIDE    = 0.5f;
        private static float M_DELAY        = 0.25f;
        private static bool AlreadyShowed = false;

        /**********************************************************************/

        /**
         * The text
         */
        public Text TxtMsg = null;

        /**
         * The Icon image
         */
        public Image Icon = null;

        /**
         * 
         */
        public HorizontalLayoutGroup HorizontalLayoutGroup = null;

        /**********************************************************************/

        private Tween m_tween               = null; // the tween
        private List<TextListItem> m_items  = null; // the list of all text item
        private Vector3 m_vector3           = Vector3.zero;
        private int m_itemIndexSelected     = -1;
        private AnimationStatus m_status    = AnimationStatus.NONE;
        private float m_startPosYInfo       = 0f;
        private float m_startPosYTxt        = 0f;

        /**********************************************************************/

        /**
         * Use this for initialization
         */
        public void Start()
        {

        }

        /**
         * Update is called once per frame
         */
        public void Update()
        {
            if (this.m_tween != null && !this.m_tween.IsFinished)
            {
                this.m_tween.Update();

                if (this.m_status == AnimationStatus.SET_NEXT)
                    return;

                float currValIcon = this.m_tween.CurrVal.x;
                float currValText = this.m_tween.CurrVal.x - TextsListAnimation.M_DELAY_TEXT;

                // manage icon
                if (currValIcon <= 1f)
                {
                    this.m_vector3 = this.Icon.transform.localPosition;
                    if (this.m_status == AnimationStatus.HIDE)
                        this.m_vector3.y = Mathf.Lerp(this.m_startPosYInfo, TextsListAnimation.M_POS_OUT, currValIcon);
                    else
                        this.m_vector3.y = Mathf.Lerp(this.m_startPosYInfo, 0f, currValIcon);


                    this.Icon.transform.localPosition = this.m_vector3;
                }

                // manage text
                if (currValText > TextsListAnimation.M_DELAY_TEXT)
                {
                    this.m_vector3 = this.TxtMsg.transform.localPosition;
                    if (this.m_status == AnimationStatus.HIDE)
                        this.m_vector3.y = Mathf.Lerp(this.m_startPosYTxt, TextsListAnimation.M_POS_OUT, currValText);
                    else
                        this.m_vector3.y = Mathf.Lerp(this.m_startPosYTxt, 0f, currValText);

                    this.TxtMsg.transform.localPosition = this.m_vector3;
                }
            }
        }

        /**
         * The destroy method
         */
        public void OnDestroy()
        {
            // remove signal listener
            if (this.m_tween == null)
                this.m_tween.SignalOnFinished -= this._onTweenFinished;

            // remove references
            this.TxtMsg     = null;
            this.Icon       = null;
            this.m_tween    = null;
            this.m_items    = null;
        }

        /**
         * Init
         * @param items The list of text list item
         */
        public void Init(List<TextListItem> items)
        {
            AlreadyShowed = false;
            this.m_items = items;

            this.gameObject.SetActive(true);

            this._setNext();
        }

        /**********************************************************************/

        // position it
        private void _setNext()
        {
            this.m_status = AnimationStatus.SET_NEXT;

            // enable the horizontal layout group to position text and icon perfectly
            this.HorizontalLayoutGroup.enabled = true;

            // set position of horizontal layout
            this._resetPosition(this.HorizontalLayoutGroup.transform, TextsListAnimation.M_POS_OUT);

            // set the next item
            this._setNextItem();

            if (this.m_tween == null)
            {
                this.m_tween                    = new Tween();
                this.m_tween.SignalOnFinished   -= this._onTweenFinished;
                this.m_tween.SignalOnFinished   += this._onTweenFinished;
            }

            // Add a delay to position
            this.m_tween.TweenFloat(0f, 1f, Ease.Linear, TextsListAnimation.M_DELAY, 0f);
        }

        // show it
        private void _show()
        {
            if (AlreadyShowed)
            { 
                this.m_status = AnimationStatus.NONE;
                this.gameObject.SetActive(false);
                return;
            }
            AlreadyShowed = true;
            // disable the horizontal layout group
            this.HorizontalLayoutGroup.enabled = false;

            // set position of img information
            this._resetPosition(this.Icon.transform, TextsListAnimation.M_POS_OUT);

            // set position of txt info
            this._resetPosition(this.TxtMsg.transform, TextsListAnimation.M_POS_OUT);

            // set position of horizontal layout
            this._resetPosition(this.HorizontalLayoutGroup.transform, 0f);

            this.m_status = AnimationStatus.SHOW;
            if (this.m_tween == null)
            {
                this.m_tween                    = new Tween();
                this.m_tween.SignalOnFinished   -= this._onTweenFinished;
                this.m_tween.SignalOnFinished   += this._onTweenFinished;
            }

            // set start pos
            this.m_startPosYInfo    = this.Icon.transform.localPosition.y;
            this.m_startPosYTxt     = this.TxtMsg.transform.localPosition.y;
            this.m_tween.TweenFloat(0f, 1f + TextsListAnimation.M_DELAY_TEXT, Ease.EaseOutCubic, TextsListAnimation.M_TIME_SHOW);
        }

        // wait to show next msg
        private void _wait()
        {
            this.m_status = AnimationStatus.WAIT;
            if (this.m_tween == null)
            {
                this.m_tween                    = new Tween();
                this.m_tween.SignalOnFinished   -= this._onTweenFinished;
                this.m_tween.SignalOnFinished   += this._onTweenFinished;
            }

            // set start pos
            this.m_startPosYInfo    = this.Icon.transform.localPosition.y;
            this.m_startPosYTxt     = this.TxtMsg.transform.localPosition.y;
            this.m_tween.TweenFloat(0f, 1f + TextsListAnimation.M_DELAY_TEXT, Ease.Linear, TextsListAnimation.M_TIME_WAIT);
        }

        // wait to show next msg
        private void _hide()
        {
            // reset position of img information
            this._resetPosition(this.Icon.transform, 0f);

            // reset position of txt info
            this._resetPosition(this.TxtMsg.transform, 0f);

            this.m_status = AnimationStatus.HIDE;
            if (this.m_tween == null)
            {
                this.m_tween = new Tween();
                this.m_tween.SignalOnFinished -= this._onTweenFinished;
                this.m_tween.SignalOnFinished += this._onTweenFinished;
            }

            // set start pos
            this.m_startPosYInfo    = this.Icon.transform.localPosition.y;
            this.m_startPosYTxt     = this.TxtMsg.transform.localPosition.y;
            this.m_tween.TweenFloat(0f, 1f + TextsListAnimation.M_DELAY_TEXT, Ease.EaseInQuad, TextsListAnimation.M_TIME_HIDE);
}

        // called when the tween is finished
        private void _onTweenFinished(Tween tween)
        {
            if (this.m_status == AnimationStatus.SET_NEXT)
                this._show();
            else if (this.m_status == AnimationStatus.SHOW)
                this._wait();
            else if (this.m_status == AnimationStatus.WAIT)
                this._hide();
            else if (this.m_status == AnimationStatus.HIDE)
                this._setNext();
        }

        // set the next item
        private void _setNextItem()
        {
            this.m_itemIndexSelected++;
            this.m_itemIndexSelected = (this.m_itemIndexSelected >= this.m_items.Count) ? 0 : this.m_itemIndexSelected;

            TextListItem item = this.m_items[this.m_itemIndexSelected];

            // manage the text
            if (item.Text != null)
            {
                this.TxtMsg.text = item.Text;
                this.TxtMsg.gameObject.SetActive(true);

            }
            else
                this.TxtMsg.gameObject.SetActive(false);

            // manage icon
            if (item.Sprite != null)
            {
                this.Icon.sprite = item.Sprite;
                this.Icon.gameObject.SetActive(true);
            }
            else
                this.Icon.gameObject.SetActive(false);
        }

        // reset position
        private void _resetPosition(Transform tr, float posY)
        {
            this.m_vector3      = tr.localPosition;
            this.m_vector3.y    = posY;
            tr.localPosition    = this.m_vector3;
        }

        private enum AnimationStatus
        {
            NONE,
            WAIT,
            SET_NEXT,
            SHOW,
            HIDE
        }
    }
}

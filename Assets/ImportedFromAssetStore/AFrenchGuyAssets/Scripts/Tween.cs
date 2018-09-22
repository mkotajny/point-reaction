using UnityEngine;
namespace Afrenchguy
{
    public class Tween
    {
        /********************************************************************************/

        /**
            * The signal dispatched we've finished our tween
            */
        public delegate void SignalOnFinishedHandler(Tween tween);
        public event SignalOnFinishedHandler SignalOnFinished = null;

        /**
            * Are we finished tweening?
            */
        public bool IsFinished = false;

        /**
            * The current value, if we're tweening a Vector3
            */
        public Vector3 CurrVal { get; private set; }

        /********************************************************************************/

        private Ease m_transition   = Ease.Linear;      // the transition that we're using
        private Vector3 m_from      = Vector3.zero;     // the value that we're tweening from
        private Vector3 m_to        = Vector3.zero;     // the value that we're tweening to
        private float m_currTime    = 0.0f;             // our current tween time
        private float m_maxTime     = 0.0f;             // our max tween time
        private float m_delayTime   = 0.0f;             // the time to delay

        /********************************************************************************/

        /**
         * Destroy
         */
        public void Destroy()
        {
            // clear our signals
            this.SignalOnFinished = null;

            // null our properties
            this.IsFinished = true;
        }

        /**
        * Tween a float. The value can be read in the CurrValF property of the Tween
        * @param from The from value
        * @param to The to value
        * @param transition The transition to use for this tween
        * @param time The time to take to finish the tween
        * @param delayTime The optional time to delay before starting the tween
        */
        public void TweenFloat(float from, float to, Ease transition, float time, float delayTime = 0.0f)
        {
            this.m_transition   = transition;
            this.m_from.x       = from;
            this.m_to.x         = to;
            this.m_currTime     = 0.0f;
            this.m_maxTime      = time;
            this.m_delayTime    = delayTime;
            this.IsFinished     = false;
            this.CurrVal        = Vector3.zero;
        }

        /**
         * Updates the Tween
         */
        public void Update()
        {
            if (this.IsFinished)
                return;

            // if we're delayed
            if (this.m_delayTime > 0.0f)
            {
                this.m_delayTime -= Time.deltaTime;
                if (this.m_delayTime > 0.0f)
                    return;
            }

            // update our time
            this.m_currTime += Time.deltaTime;
            if (this.m_currTime > this.m_maxTime)
                this.m_currTime = this.m_maxTime;

            // set the curr value
            this.CurrVal = Transitions.ChangeVector(this.m_currTime, this.m_from, this.m_to, this.m_maxTime, this.m_transition);

            // check if it's finished
            if (this.m_currTime == this.m_maxTime)
                this._finish();
        }

        /********************************************************************************/

        // called when the tween is finished
        private void _finish()
        {
            // set our final value
            this.CurrVal = this.m_to;

            // dispatch the on finished signal
            this.IsFinished = true;
            if (this.SignalOnFinished != null)
                this.SignalOnFinished(this);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;

namespace DesignPatterns
{
    public class State
    {
        public class Mobile
        {
            public enum State { Off, Normal, Charge, FullCharged }

            private State state = State.Normal;
            private bool charging = false;
            private float battery = 50f;

            private void Update()
            {
                switch (state)
                {
                    case State.Off:
                        OffUpdate();
                        break;
                    case State.Normal:
                        NormalUpdate();
                        break;
                    case State.Charge:
                        ChargeUpdate();
                        break;
                    case State.FullCharged:
                        FullChargedUpdate();
                        break;
                }
            }

            private void OffUpdate()
            {
                if (charging)
                {
                    state = State.Charge;
                }
            }
            private void NormalUpdate()
            {
                battery -= 1.5f * Time.deltaTime;

                if (charging)
                {
                    state = State.Charge;
                }
                else if (battery <= 0)
                {
                    state = State.Off;
                }
            }

            private void ChargeUpdate()
            {
                battery += 2.5f * Time.deltaTime;
                if (!charging)
                {
                    state = State.Normal;
                }
                else if (battery >= 100)
                {
                    state = State.FullCharged;
                }
            }

            private void FullChargedUpdate()
            {
                if (!charging)
                {
                    state = State.Normal;
                }
            }

        }
    }

}

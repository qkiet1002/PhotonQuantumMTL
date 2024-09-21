﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quantum
{
    public class HealthBar : MonoBehaviour
    {
        public Transform transformValue;
        private Vector3 scale;

        public void Start()
        {
            scale = transformValue.localScale;  
        }

        public void SetValue(float value)
        {
            if(value< 0) value = 0;
            scale.x = value;
            transformValue.localScale = scale; 
        }

        public void AddHealth(float quantity)
        {
            quantity += 1;
        }
    }
}

using System.Collections.Generic;
using Model.Runtime.Projectiles;
using UnityEngine;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;
        
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;
            //a. Проверка перегрева оружия
            if (GetTemperature() >= overheatTemperature)
            {
                return;
            }
            
            //2.a Увеличение снарядов
            int MissileCount = GetTemperature(); // кол-во снарядов = текущей температуре
            for (int i = 0; i < MissileCount; i++) ;
            {
                var projectile = CreateProjectile(forTarget);
                AddProjectileToList(projectile, intoList);
            }
            //b. Вызов метода private void IncreaseTemperature()
            IncreaseTemperature();
        }

        public override Vector2Int GetNextStep()
        {
            return base.GetNextStep();
        }

        protected override List<Vector2Int> SelectTargets()

        {

            List<Vector2Int> result = GetReachableTargets();
            if (result.Count == 0)
                return result;

            Vector2Int target = result[0]; // Изначальная цель
            var minDistance = DistanceToOwnBase(target); // с float не пробовал, но думаю разницы нет

            for (int i = 1; i < result.Count; i++)
            {
                var distance = DistanceToOwnBase(result[i]); // Ближайшая цель на начало расчета
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = result[i];
                }
            }
            result.Clear(); //очистить
            result.Add(target); //добавить
            return result;
        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {              
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}
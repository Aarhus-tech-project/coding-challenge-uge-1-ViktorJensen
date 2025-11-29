using System;
using System.Linq.Expressions;
namespace BoxingGame
{
    public class Boxer
    {
        public string Name { get; }
        public int X { get; set; }
        public int Y { get; }
        public bool IsFacingLeft { get; private set; }
        private int health = 100;
        private int stamina = 100;
        private int regainStaminaMs = 1500;
        private int staminaTimerMs = 0;
        public int Stamina { get => stamina; set => stamina = Math.Max(0, Math.Min(100, value)); }
        public int Health { get => health; set => health = Math.Max(0, Math.Min(100, value)); }
        public bool IsPunching { get; private set; }
        public bool PunchHit { get; set; }
        public bool IsBlocking { get; private set; }
        private int punchDurationMs = 300;
        private int coolDownMs = 500;
        private int blockDurationMs = 300;
        private int timerMs = 0;
        private int cooldownTimerMs = 0;
        public Boxer(string name, int startX, int startY, bool leftFacing)
        {
            Name = name;
            X = startX;
            Y = startY;
            IsFacingLeft = leftFacing;
        }
        public void MoveLeft(int minX)
        {
            IsFacingLeft = true;
            if (X > minX) X -= 1;
        }
        public void MoveRight(int maxX)
        {
            IsFacingLeft = false;
            if (X < maxX) X += 1;
        }
        public void RegainStamina(int deltaMs)
        {
            if (Stamina >= 100)
            {
                staminaTimerMs = 0;
                return;
            }
            staminaTimerMs += deltaMs;
            if (staminaTimerMs >= regainStaminaMs)
            {
                int increments = staminaTimerMs / regainStaminaMs;
                staminaTimerMs %= regainStaminaMs;
                Stamina += 10 * increments;
            }
        }
        public void Punch()
        {
            if (Stamina >= 10)
                if (cooldownTimerMs <= 0 && !IsPunching)
                {
                    IsPunching = true;
                    timerMs = punchDurationMs;
                    cooldownTimerMs = coolDownMs;
                    PunchHit = false;
                }
            else
                {
                    return;
                }
        }
        public void Block()
        {
            if (Stamina >= 10)
                if (cooldownTimerMs <= 0)
                {
                    IsBlocking = true;
                    timerMs = blockDurationMs;
                    cooldownTimerMs = coolDownMs;
                }
            else
                {
                    return;
                }
        }
        public void Tick(int deltaMs)
        {
            if (cooldownTimerMs > 0) cooldownTimerMs -= deltaMs;

            if (IsPunching || IsBlocking)
            {
                timerMs -= deltaMs;
                if (timerMs <= 0)
                {
                    if (IsPunching)
                    {
                        IsPunching = false;
                        PunchHit = false;
                    }
                    if (IsBlocking)
                    {
                        IsBlocking = false;
                    }
                }
            }
            RegainStamina(deltaMs);
        }
    }
}
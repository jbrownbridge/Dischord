using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dischord.Engine
{
    public class Animation
    {
        private float frameLength;
        private float totalElapsedTime;

        private string currentBehaviour;
        private int currentFrame;
        private int totalFrames;

        private AnimationManager animationManager;

        public Animation(AnimationManager animationManager, int framesPerSecond)
        {
            this.animationManager = animationManager;
            frameLength = 1.0f / framesPerSecond;
        }

        public AnimationManager AnimationManager
        {
            get { return animationManager; }
            set { animationManager = value; }
        }

        public string Behaviour
        {
            get { return currentBehaviour; }
            set
            {
                currentBehaviour = value;
                currentFrame = 0;
                totalFrames = animationManager.BehaviourList[currentBehaviour].Count;
            }
        }

        public Frame Frame
        {
            get { return animationManager.BehaviourList[currentBehaviour][currentFrame]; }
        }

        public void UpdateFrame(float elapsedTime)
        {
            totalElapsedTime += elapsedTime;
            if (totalElapsedTime >= frameLength)
            {
                currentFrame++;
                currentFrame %= totalFrames;
                totalElapsedTime %= frameLength;
            }
        }
    }
}

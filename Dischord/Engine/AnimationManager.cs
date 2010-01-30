using System;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dischord.Engine
{
    public class AnimationManager
    {
        private int columns;
        private int rows;
        private int frameWidth;
        private int frameHeight;
        private Vector2 origin;

        private Dictionary<string, List<Frame>> behaviourList;

        public AnimationManager(
            Texture2D spriteImage, int columns, int rows, string behaviourPath)
        {
            this.columns = columns;
            this.rows = rows;
            frameWidth = spriteImage.Width / columns;
            frameHeight = spriteImage.Height / rows;
            origin = new Vector2(frameWidth / 2, frameHeight / 2);

            behaviourList = new Dictionary<string, List<Frame>>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(behaviourPath);

            XmlNodeList xmlBehaviours = xmlDoc.SelectNodes("Behaviours/Behaviour");
            foreach (XmlNode behaviour in xmlBehaviours)
            {
                List<Frame> frameList = new List<Frame>();
                behaviourList.Add(behaviour.FirstChild.InnerText, frameList);

                XmlNodeList frames = behaviour.SelectNodes("Frames/Frame");
                foreach (XmlNode frame in frames)
                {
                    frameList.Add(
                        new Frame(
                            Frame(int.Parse(frame.Attributes["Position"].Value)),
                            int.Parse(frame.Attributes["Rotation"].Value),
                            (SpriteEffects)Enum.Parse(typeof(SpriteEffects),
                            ((frame.Attributes["Flip"].Value == "None") ? "" : "Flip")
                            + frame.Attributes["Flip"].Value +
                            ((frame.Attributes["Flip"].Value == "None") ? "" : "ly"), true)
                    ));
                }
            }
        }

        public Dictionary<string, List<Frame>> BehaviourList
        {
            get { return behaviourList; }
        }

        public Vector2 Origin
        {
            get { return origin; }
        }

        public int FrameWidth
        {
            get { return frameWidth; }
        }

        public int FrameHeight
        {
            get { return frameHeight; }
        }

        private Rectangle Frame(int position)
        {
            return new Rectangle(
                ((position - 1) % columns) * frameWidth,
                ((position - 1) / columns) * frameHeight,
                frameWidth,
                frameHeight
                );
        }
    }
}
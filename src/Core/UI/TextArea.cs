using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cr_mono.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cr_mono.Core.UI
{
    internal class TextArea 
    {
        private List<string> lines;
        private int blockWidth;
        private static SpriteFont Font 
        {
            get { return ResourceManager.FontSmall; }
        }

        internal TextArea(string text, int blockWidth)
        {
            this.blockWidth = blockWidth;
            this.lines = [];

            Vector2 measuredString = Font.MeasureString(text);
  
            CalculateLines(text);
        }

        private void CalculateLines(string fullText)
        {
            string[] words = fullText.Split(" ");
            StringBuilder currentLine = new StringBuilder();

            foreach (string word in words)
            {
                string line = currentLine.Length == 0 ? word : $"{currentLine} {word}";
                Vector2 size = Font.MeasureString(line);

                if (size.X <= blockWidth)
                {
                    if (currentLine.Length == 0)
                    {
                        currentLine.Append(word);
                    }
                    else
                    {
                        currentLine.Append($" {word}");
                    }
                }
                else 
                {
                    if (currentLine.Length > 0)
                    {
                        lines.Add(currentLine.ToString());
                    }
                    currentLine.Clear();
                    currentLine.Append(word);
                }
            }

            if (currentLine.Length > 0) 
            {
                lines.Add(currentLine.ToString());
            }
        }

        internal void Draw(SpriteBatch spriteBatch, Vector2 position) 
        {
            for(int i = 0; i < lines.Count; i++)
            {
                spriteBatch.DrawString(Font, lines[i], new Vector2(position.X, position.Y + i * 8), Color.Black);
            }
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puissance_4
{
    class Bouton
    {
        private Texture2D _texture;
        private SpriteFont _texte;
        private Rectangle _rectangle;
        private Vector2 _position;
        private Color _fontColor;
        private Vector2 _size;

        
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }


        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        public SpriteFont Texte
        {
            get { return _texte; }
            set { _texte = value; }
        }

        public Rectangle Rectangle
        {
            get { return _rectangle; }
            set { _rectangle = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Bouton(Texture2D texture, Vector2 position, Vector2 size)
        {
            this._texture = texture;
            this._position = position;
            this._size = size;
        }
    }
}

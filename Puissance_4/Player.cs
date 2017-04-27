using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puissance_4
{
    class Player
    {
        private int nb;
        private int score;

        public Player(int nb)
        {
            this.nb = nb;
            this.score = 0;
        }

        public int Nb
        {
            get { return nb; }
            set { nb = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

    }
}

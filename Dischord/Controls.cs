using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dischord {
    public class Controls {
        private int direction;
        public int Direction {
            get {
                return direction;
            }
            set {
                direction = value;
            }
        }
        private bool jump;
        public bool Jump {
            get {
                return jump;
            }
            set {
                jump = value;
            }
        }

        public Controls() {
            direction = 0;
            jump = false;
        }
    }
}

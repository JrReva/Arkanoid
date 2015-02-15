using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Arkanoid
{
    public interface ITexturable
    {
        void SetTexture(ContentManager contentManager, String nom);
    }
}

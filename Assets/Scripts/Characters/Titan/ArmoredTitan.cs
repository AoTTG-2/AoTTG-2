﻿using Assets.Scripts.Characters.Titan.Configuration;

namespace Assets.Scripts.Characters.Titan
{
    public class ArmoredTitan : TitanBase
    {
        public override void Initialize(TitanConfiguration configuration)
        {
            EntityService.Register(this);
        }
    }
}

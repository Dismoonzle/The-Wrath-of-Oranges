using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace dododo.Content.Items
{

    public class RedRevolution : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Melee;
            Item.width = 80;
            Item.height = 80;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.shoot = ProjectileID.SolarWhipSwordExplosion;
            Item.shootSpeed = 14f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 20;
            Item.value = Item.buyPrice(gold: 100);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        
    }
}

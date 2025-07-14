using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace dododo.Content.Items
{ 
	
	public class WhatIsThis : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 5000;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.shoot = ProjectileID.MonkStaffT3_AltShot;
			Item.shootSpeed = 14f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 20;
			Item.value = Item.buyPrice(gold:10);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 1000);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}

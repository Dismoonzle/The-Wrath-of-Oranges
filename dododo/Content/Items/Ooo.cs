using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using dododo.NPCs;

namespace dododo.Content.Items.Consumables
{
    public class Ooo : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 20;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                int type = ModContent.NPCType<Oranger>();

                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    // 单人模式直接生成
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }
                else
                {
                    // 多人模式使用新的网络同步方式
                    if (Main.netMode == NetmodeID.Server) // 服务器端
                    {
                        NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent,
                            number: player.whoAmI,
                            number2: type);
                    }
                    else // 客户端
                    {
                        // 客户端需要发送自定义网络包
                        var packet = Mod.GetPacket();
                        packet.Write((byte)YourModMessageType.SpawnOranger);
                        packet.Write((byte)player.whoAmI);
                        packet.Write(type);
                        packet.Send();
                    }
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofNight, 6)
                .AddIngredient(ItemID.DemoniteBar, 3)
                .AddTile(TileID.DemonAltar)
                .Register();

            // 可选：添加猩红世界配方
            CreateRecipe()
                .AddIngredient(ItemID.SoulofNight, 6)
                .AddIngredient(ItemID.CrimtaneBar, 3)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
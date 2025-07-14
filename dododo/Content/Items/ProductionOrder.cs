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
    public class ProductionOrder : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
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

                int type = ModContent.NPCType<B29>();

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
                .AddIngredient(ItemID.IronBar, 6)
                .AddTile(TileID.WorkBenches)
                .Register();
            
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 6)
                .AddTile(TileID.WorkBenches)
                .Register();

        }
    }
}
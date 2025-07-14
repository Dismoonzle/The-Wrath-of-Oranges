using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace dododo.NPCs
{
    [AutoloadBossHead] // 这会让游戏自动加载 Boss 头部图标
    public class B29 : ModNPC
    {
        // 阶段变量
        private int phase = 1;

        // 攻击计时器
        private int attackTimer = 0;

        // 弹幕发射频率
        private const int flameBombRate = 90;
        private const int bulletRate = 45;

       

        public override void SetDefaults()
        {
            NPC.width = 700;
            NPC.height = 175;
            NPC.damage = 80;
            NPC.defense = 80;
            NPC.lifeMax = 200000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(0, 10, 0, 0);
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 10f;
            Music = MusicID.Boss1; // 使用默认的Boss音乐
        }

        public override void AI()
        {
            // 目标玩家
            Player player = Main.player[NPC.target];

            // 确保目标有效
            if (NPC.target < 0 || NPC.target == 255 || player.dead || !player.active)
            {
                NPC.TargetClosest();
                player = Main.player[NPC.target];
                if (player.dead || !player.active)
                {
                    NPC.velocity = new Vector2(0f, -10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }

            // 检查进入第二阶段
            if (phase == 1 && NPC.life < NPC.lifeMax * 0.5f)
            {
                phase = 2;
                NPC.netUpdate = true; // 同步到多人游戏

            }

            // 攻击模式
            attackTimer++;
          
           

            // 第一阶段攻击模式
            if (phase == 1)
            {
                // 移动逻辑 - 简单跟随玩家
                Vector2 direction = player.Center - NPC.Center;
                direction.Normalize();
                NPC.velocity = direction * 12f;

                // 发射火焰炸弹
                if (attackTimer % flameBombRate == 0)
                {
                    ShootFlameBomb(player);
                }
            }
            // 第二阶段攻击模式
            else
            {
                // 更快的移动
                Vector2 direction = player.Center - NPC.Center;
                direction.Normalize();
                NPC.velocity = direction * 15f;

                // 发射高速子弹
                if (attackTimer % bulletRate == 0)
                {
                    ShootHighVelocityBullet(player);
                }

                // 偶尔发射火焰炸弹
                if (attackTimer % (flameBombRate * 2) == 0)
                {
                    ShootFlameBomb(player);
                }
            }
        }

        private void ShootFlameBomb(Player player)
        {
            Vector2 direction = player.Center - NPC.Center;
            direction.Normalize();

            // 创建弹幕
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 8f,
                                    ModContent.ProjectileType<NPCs.FlameBomb>(),
                                    30, 2f, Main.myPlayer);

           
        }

        private void ShootHighVelocityBullet(Player player)
        {
            Vector2 direction = player.Center - NPC.Center;
            direction.Normalize();

            // 创建弹幕
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 15f,
                                    ModContent.ProjectileType<NPCs.BulletHighVelocity>(),
                                    20, 1f, Main.myPlayer);

            // 发射音效
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item11, NPC.position);
        }

       
    }

        

          
      
}

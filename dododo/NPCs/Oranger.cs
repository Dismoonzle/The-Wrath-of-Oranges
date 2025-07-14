using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace dododo.NPCs
{
    [AutoloadBossHead]
   
    public class Oranger : ModNPC
    {
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // 基础掉落物
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.RedRevolution>(),1)); // 100%掉落赤色革命
            npcLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 1, 5, 10)); // 100%掉落5-10金币
        }
        // AI阶段枚举
        private enum AIPhase
        {
            Phase1,
            Phase2,
            Death
        }

        // AI状态变量
        private AIPhase CurrentPhase
        {
            get => (AIPhase)(int)NPC.ai[0];
            set => NPC.ai[0] = (float)value;
        }

        private float AttackTimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        private float MovementTimer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        private Vector2 TargetCenter => Main.player[NPC.target].Center;

        public override void SetDefaults()
        {
            NPC.width = 600;
            NPC.height = 600;
            NPC.lifeMax = 22000;
            NPC.defense = 30;
            NPC.damage = 80;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.npcSlots = 10f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = Item.buyPrice(0, 10000000, 0, 0);
            NPC.aiStyle = -1; // 使用自定义AI
        }

        public override void AI()
        {
            // 确保有目标
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            if (Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.velocity = new Vector2(0f, -10f);
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
                return;
            }

            // 检查阶段转换
            if (CurrentPhase != AIPhase.Death && NPC.life < NPC.lifeMax * 0.5f)
            {
                CurrentPhase = AIPhase.Phase2;
            }

            // 更新计时器
            AttackTimer++;
            MovementTimer++;

            // 根据阶段执行不同行为
            switch (CurrentPhase)
            {
                case AIPhase.Phase1:
                    Phase1Behavior();
                    break;
                case AIPhase.Phase2:
                    Phase2Behavior();
                    break;
                case AIPhase.Death:
                    DeathBehavior();
                    break;
            }
            if (CurrentPhase != AIPhase.Death && NPC.life <= 0)
            {
                CurrentPhase = AIPhase.Death;
            }
        }

        private void Phase1Behavior()
        {
            // 第一阶段移动模式：围绕玩家飞行
            float circleRadius = 300f;
            float circleSpeed = 0.02f;

            Vector2 circleCenter = TargetCenter + new Vector2(
                (float)Math.Cos(MovementTimer * circleSpeed) * circleRadius,
                (float)Math.Sin(MovementTimer * circleSpeed) * circleRadius);

            Vector2 direction = circleCenter - NPC.Center;
            float distance = direction.Length();
            direction.Normalize();

            // 平滑移动
            if (distance > 10f)
            {
                float speed = MathHelper.Clamp(distance / 50f, 5f, 15f);
                NPC.velocity = direction * speed;
            }
            else
            {
                NPC.velocity *= 0.95f;
            }

            // 第一阶段攻击：周期性发射3方向弹幕
            if (AttackTimer % 120 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 velocity = (TargetCenter - NPC.Center).SafeNormalize(Vector2.UnitY) * 8f;

                    for (int i = -1; i <= 1; i++)
                    {
                        Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(15 * i));
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, newVelocity,
                            ModContent.ProjectileType<As>(), 30, 2f, Main.myPlayer);
                    }
                }
            }
        }

        private void Phase2Behavior()
        {
            // 第二阶段移动模式：更激进的冲刺攻击
            if (MovementTimer % 180 < 60)
            {
                // 冲刺阶段
                if (MovementTimer % 180 == 0)
                {
                    Vector2 dashDirection = (TargetCenter - NPC.Center).SafeNormalize(Vector2.UnitY);
                    NPC.velocity = dashDirection * 20f;
                }
            }
            else
            {
                // 减速阶段
                NPC.velocity *= 0.95f;
            }

            // 第二阶段攻击：快速发射旋转弹幕
            if (AttackTimer % 40 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int projectileCount = 8;
                    for (int i = 0; i < projectileCount; i++)
                    {
                        Vector2 velocity = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / projectileCount * i) * 5f;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<Qw>(), 25, 2f, Main.myPlayer);
                    }
                }
            }
        }

        private void DeathBehavior()
        {
            // 死亡行为：爆炸并消失
            NPC.velocity = Vector2.Zero;
            NPC.dontTakeDamage = true;
            NPC.alpha += 5;

            if (NPC.alpha >= 255)
            {
                NPC.active = false;

                // 创建爆炸效果
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame,
                        Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-10f, 10f), 0, default, 2f);
                }
            }
        }

      

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }
    }

    // 弹幕1：瞎写
    public class As : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            // 简单的直线运动
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // 创建尾迹效果
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Shadowflame, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
            }
        }
    }

    // 弹幕2：懒得搞
    public class Qw: ModProjectile
    {
        private float RotationSpeed => Projectile.ai[0] == 0 ? 0.05f : -0.05f;

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            // 旋转运动
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                Projectile.ai[0] = Main.rand.Next(2);
            }

            Projectile.velocity = Projectile.velocity.RotatedBy(RotationSpeed);
            Projectile.rotation += 0.1f;

            // 创建光环效果
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Shadowflame, 0f, 0f, 150, default, 1.5f);
            }
        }
    }
}




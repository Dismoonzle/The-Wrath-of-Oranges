using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace dododo.NPCs
{
    [AutoloadBossHead] // �������Ϸ�Զ����� Boss ͷ��ͼ��
    public class B29 : ModNPC
    {
        // �׶α���
        private int phase = 1;

        // ������ʱ��
        private int attackTimer = 0;

        // ��Ļ����Ƶ��
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
            Music = MusicID.Boss1; // ʹ��Ĭ�ϵ�Boss����
        }

        public override void AI()
        {
            // Ŀ�����
            Player player = Main.player[NPC.target];

            // ȷ��Ŀ����Ч
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

            // ������ڶ��׶�
            if (phase == 1 && NPC.life < NPC.lifeMax * 0.5f)
            {
                phase = 2;
                NPC.netUpdate = true; // ͬ����������Ϸ

            }

            // ����ģʽ
            attackTimer++;
          
           

            // ��һ�׶ι���ģʽ
            if (phase == 1)
            {
                // �ƶ��߼� - �򵥸������
                Vector2 direction = player.Center - NPC.Center;
                direction.Normalize();
                NPC.velocity = direction * 12f;

                // �������ը��
                if (attackTimer % flameBombRate == 0)
                {
                    ShootFlameBomb(player);
                }
            }
            // �ڶ��׶ι���ģʽ
            else
            {
                // ������ƶ�
                Vector2 direction = player.Center - NPC.Center;
                direction.Normalize();
                NPC.velocity = direction * 15f;

                // ��������ӵ�
                if (attackTimer % bulletRate == 0)
                {
                    ShootHighVelocityBullet(player);
                }

                // ż���������ը��
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

            // ������Ļ
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 8f,
                                    ModContent.ProjectileType<NPCs.FlameBomb>(),
                                    30, 2f, Main.myPlayer);

           
        }

        private void ShootHighVelocityBullet(Player player)
        {
            Vector2 direction = player.Center - NPC.Center;
            direction.Normalize();

            // ������Ļ
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 15f,
                                    ModContent.ProjectileType<NPCs.BulletHighVelocity>(),
                                    20, 1f, Main.myPlayer);

            // ������Ч
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item11, NPC.position);
        }

       
    }

        

          
      
}

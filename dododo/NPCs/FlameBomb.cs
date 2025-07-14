using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace dododo.NPCs
{
    public class FlameBomb : ModProjectile
    {
    

        public override void SetDefaults()
        {
            Projectile.width = 314;
            Projectile.height = 94;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            

            // ����Ч��
            Projectile.velocity.Y += 0.1f;

            // ��תЧ��
            Projectile.rotation += 0.1f;
        }

        public override void Kill(int timeLeft)
        {
            

           
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace dododo.NPCs
{
    public class BulletHighVelocity : ModProjectile
    {
       

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 1; // 使弹幕更快
        }

        public override void AI()
        {
         

            // 使子弹指向移动方向
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}
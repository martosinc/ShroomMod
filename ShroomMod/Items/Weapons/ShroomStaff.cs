using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace ShroomMod.Items.Weapons
{
    public class ShroomStaff : ModItem
    {
        public static Vector2 direction;
        Random rand = new Random();
        public override void SetDefaults() {
			item.damage = 30;
			item.knockBack = 3f;
			item.mana = 10;
			item.width = 32;
			item.height = 32;
			item.useTime = 20;
            item.autoReuse = true;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item44;

            item.noMelee = true;
            item.magic = true;
            item.shoot = ModContent.ProjectileType<ShroomStaffProjectile>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            var spawnDir = new Vector2(-1, -1);
            if (Main.MouseWorld.X < player.position.X) {
                spawnDir = new Vector2(1, -1);
            }
            position = Main.MouseWorld + spawnDir * 1000;
            direction = spawnDir * -1;
            return true;
        }
    }
    public class ShroomStaffProjectile : ModProjectile
    {
        bool start = true;
        Vector2 direction;
        Random rand = new Random();
        public override void SetDefaults() {
            projectile.height = 24;
            projectile.width = 22;

            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.melee = true;
            projectile.friendly = true;
        }

        public override void AI() {
            float speed = 7.5f;
            if (start) {
                direction = ShroomStaff.direction * speed;
                start = false;
            }
            projectile.velocity = direction * 2;
            // using (System.IO.StreamWriter file =
            //     new System.IO.StreamWriter("/home/martos/.local/share/Terraria/ModLoader/Mod Sources/ShroomMod/Items/Wepaons"))
            // {
            //     file.WriteLine(direction);
            // }
            projectile.rotation += (float)Math.PI / 20;
            // if ((float)Main.player[projectile.owner].position.Y+200 > (float)projectile.position.Y < (float)Main.player[projectile.owner].position.Y-200) {
            //     projectile.tileCollide = true;
            // }
        }
        public override bool PreKill(int timeLeft) {
            Main.PlaySound(SoundID.Item10, projectile.position);
            Projectile.NewProjectile(projectile.Center, new Vector2(0f, 0f), ModContent.ProjectileType<ShroomStaffCloud>(), 50, 3f, Main.myPlayer);
            Projectile.NewProjectile(projectile.Center + new Vector2(rand.Next(-10, 10), rand.Next(-10, 10)), new Vector2(0f, 0f), ModContent.ProjectileType<ShroomStaffCloud>(), 50, 3f, Main.myPlayer);
            Projectile.NewProjectile(projectile.Center + new Vector2(rand.Next(-10, 10), rand.Next(-10, 10)), new Vector2(0f, 0f), ModContent.ProjectileType<ShroomStaffCloud>(), 50, 3f, Main.myPlayer);
            return true;
        }
    }
    public class ShroomStaffCloud : ModProjectile
    {
        bool start = false;
        Random rand = new Random();
        public override void SetDefaults() {
            projectile.height = 35;
            projectile.width = 35;

            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.melee = true;
            projectile.friendly = true;
        }

        public override void AI() {
            if (start) {
                projectile.rotation = (float)rand.Next(0, 360);
                projectile.alpha = 150;
                projectile.scale = rand.Next(5, 8) / 10;
                start = false;
            }
            projectile.scale *= 1.03f;
            projectile.alpha += rand.Next(5, 15);
            if (projectile.alpha >= 210) {
                projectile.active = false;
            }
        }
    }
}
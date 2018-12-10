using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Juega.Characters;
using Juega.Managers;
using System.Collections.Generic;
using System;
using Juega.Graphics;
using Juega.Meta;
using Microsoft.Xna.Framework.Input;

namespace Juega.Screens
{
    public class GamePlayScreen : GameScreen
    {
        Life life;
        Score score;
        Player player;
        Background background;
        List<Skeleton> enemies = new List<Skeleton>();
        Random random = new Random();
        float elapsedTimeSinceLastEnemyCreation = 0;

        public GamePlayScreen()
        {
            player = new Player();
            life = new Life();
            score = new Score();
            background = new Background();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            background.LoadContent();
            player.LoadContent();
            life.LoadContent();
            score.LoadContent();

            CreateEnemies();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            background.UnloadContent();
            player.UnloadContent();
            life.UnloadContent();
            score.UnloadContent();

            foreach (Skeleton enemy in enemies)
            {
                enemy.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            background.Update(gameTime);

            player.Blood.IsActive = false;

            if (InputManager.Instance.KeyPressed(Keys.P))
                ScreenManager.Instance.PauseScreen();

            elapsedTimeSinceLastEnemyCreation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            List<int> indexesToRemove = indexesToRemove = new List<int>();

            // Borro los que fueron lastimados o salieron de pantalla
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (i < enemies.Count)
                {
                    if (enemies[i].Hitted || enemies[i].OutOfBounds)
                    {
                        enemies[i].UnloadContent();
                        enemies.RemoveAt(i);
                    }
                }
            }

            if (enemies.Count > 0)
            {
                foreach (Skeleton enemy in enemies)
                {
                    enemy.Update(gameTime);

                    // Si choca con el player y no ataco. Seteo el atributo que va a bajar la vida en true y el atributo que el enemigo ataco en true asi prevengo otro ataque del mismo enemigo
                    if (enemy.Image.Rectangle.Intersects(player.Image.Rectangle) && !enemy.Strike)
                    {
                        life.Hitted = true;
                        enemy.Strike = true;
                    }

                    // Chequeo los disparos del enemigo
                    foreach(Shoot eS in enemy.Shoots)
                    {
                        if (player.Image.Rectangle.Intersects(eS.Image.Rectangle) && !eS.Impacted)
                        {
                            player.Blood.IsActive = true;
                            life.Hitted = true;
                            life.Modifier = 0.10f;
                            eS.Impacted = true;
                        }
                    }

                    // Chequeo los disparos del player
                    foreach(Shoot pS in player.Shoots)
                    {
                        if (enemy.Image.Rectangle.Intersects(pS.Image.Rectangle) && !pS.Impacted)
                        {
                            score.EnemyHitted = true;
                            enemy.Hitted = true;
                            pS.Impacted = true;
                        }
                    }

                    // Si llega abajo de todo y no lo agregue cuando dispare lo guardo. Ademas le saco el doble de vida al player.
                    if (enemy.Image.Position.Y > ScreenManager.Instance.ViewportHeight && !enemy.Strike && !enemy.Hitted)
                    {
                        life.Hitted = true;
                        life.Modifier = 0.1f; // le quito 1 de vida por cada enemigo que se pasa de la linea
                        enemy.OutOfBounds = true;
                        enemy.Strike = true;
                    }
                }
            }

            if (enemies.Count < 30 && elapsedTimeSinceLastEnemyCreation > 1)// Si hay menos de 15 enemigos y hace mas de 1 segundo cree el anterior, voy de nuevo.
            {
                 CreateEnemies();
                // Actualizo el ultimo que cree
                // Ojo, que si se envia la cantidad por parametro, se debe actualizar esa misma cantidad a partir del ultimo en la lista 
                // (esto es porque ya actualice todos los que tenia mas arriba, pero si inserto uno nuevo sin actualizarlo se ve TODO el sprite de skeletos y no queremos eso)
                enemies[enemies.Count - 1].Update(gameTime);
                elapsedTimeSinceLastEnemyCreation -= 1;
            }

            player.Update(gameTime);
            life.Update(gameTime);
            score.Update(gameTime);

            if (life.PlayerLife <= 0)
                ScreenManager.Instance.ChangeScreens("GameOverScreen");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            background.Draw(spriteBatch);
            player.Draw(spriteBatch);
            life.Draw(spriteBatch);
            score.Draw(spriteBatch);

            foreach (Skeleton enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        void CreateEnemies(int max = 1)
        {
            for(int x = 0; x < max; x++)
            {
                int randomX = random.Next(100, 480 - 100);
                int randomY = random.Next(0, 640 - 460);

                Skeleton enemy = new Skeleton();
                enemy.Image.Position.X = randomX;
                enemy.Image.Position.Y = randomY;

                enemy.LoadContent();

                enemies.Add(enemy);
            }
        }
    }
}
